using OscVrcMaui.Models;
using OscVrcMaui.Utils;
using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace OscVrcMaui.Services
{
    public class BLEService
    {
        List<IPeripheral> scanResults = new List<IPeripheral>();
        public delegate void HeartBeatHandler(int beat);
        Timer hrTicker;
        public event HeartBeatHandler HeartBeatRecieved;
        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();
        private IGattCharacteristic HrControl;
         ConfigService configService => DependencyService.Get<ConfigService>();
        public delegate void ScanHandler(List<IPeripheral> devices);
        public event ScanHandler ScanDone;
        public static Guid GuidHeartRateControl = new Guid("00002A39-0000-1000-8000-00805f9b34fb");
        public static Guid GuidHeartrate = new Guid("00002A37-0000-1000-8000-00805f9b34fb");
        IPeripheral DeviceSelected;
        public BLEService()
        {
          
            


        }


        public void InitConfig() {

            var conf = configService.LoadConfig();
            var bleManager = ShinyHost.Resolve<IBleManager>();
            bleManager.GetKnownPeripheral(conf.SelectedDeviceId).Subscribe(x => 
            {
                DeviceSelected = x;
                Connect(); 
            });



        }
        public void ScanBle() {
            scanResults.Clear();
            try
            {
                var bleManager = ShinyHost.Resolve<IBleManager>();


                bleManager.GetConnectedPeripherals().Subscribe(scanResult =>
                {
               
                    foreach (var item in scanResult)
                    {
                        scanResults.Add(item);
                    }
                    if (ScanDone != null) ScanDone.Invoke(scanResults);
                 
                });
            }
            catch (Exception e)
            {


                Console.WriteLine(e.SerializeToString());

            }





        }
        public void Scan()
        {
            
            scanResults.Clear();
            try
            {
                var bleManager = ShinyHost.Resolve<IBleManager>();


                bleManager.GetConnectedPeripherals().Subscribe(scanResult =>
                {
                 
                    foreach (var item in scanResult)
                    {
                        if (item.Name.Contains("Band") || item.Name.Contains("AmazFit"))
                        {
                            DeviceSelected = item;

                        }
                    }
                    Connect();
                });
            }
            catch (Exception e) {


                Console.WriteLine(e.SerializeToString());
            
            }


        }
        public void SetDevice(IPeripheral device)
        {
            
            DeviceSelected = device;

            var conf = configService.LoadConfig();
            conf.SelectedDeviceId = device.Uuid;
            configService.WriteConfig(conf);

        }
        public void Connect()
        {

            if (DeviceSelected != null)
                try
                {
                    DeviceSelected.Connect();
                    DeviceSelected.GetAllCharacteristics().Subscribe(async characteristicz =>
                    {
                        foreach (var characteristic in characteristicz)
                        {
                            if (characteristic != null && !Characteristics.Contains(characteristic))
                            {
                                Characteristics.Add(characteristic);
                                if (characteristic.Uuid == GuidHeartRateControl.ToString())
                                {
                                    HrControl = characteristic;
                                    //Magic bytes, trying to activate endless scanning 
                                    await characteristic.Write(new byte[] { 0x14, 1 });
                                    await characteristic.Write(new byte[] { 0x15, 2, 0 });
                                    await characteristic.Write(new byte[] { 0x15, 1, 0 });
                                    hrTicker = new Timer(((dt) => {

                                        HrControl.Write(new byte[] { 0x16 });
                                    }), null, 1, 2000);
                                    await characteristic.Write(new byte[] { 0x15, 1, 1 });


                                }
                                else
                                if (characteristic.Uuid == GuidHeartrate.ToString())
                                {




                                    characteristic.Notify().Subscribe(res => {
                                        HeartBeatRecieved?.Invoke(res.Data[1]);
                                    });
                                }

                            }


                        }
                    });

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.SerializeToString());

                }
        }


    }
}
