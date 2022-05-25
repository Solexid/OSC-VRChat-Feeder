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
        Timer hrTicker;
        public delegate void HeartBeatHandler(int beat);
        public event HeartBeatHandler HeartBeatRecieved;

        public delegate void StepsData(int steps,int calories,int distance);
        public event StepsData StepsDataRecieved;


        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();
        private IGattCharacteristic HrControl;
         ConfigService configService => DependencyService.Get<ConfigService>();
        public delegate void ScanHandler(List<IPeripheral> devices);
        public event ScanHandler ScanDone;
        public static Guid GuidHeartRateControl = new Guid("00002A39-0000-1000-8000-00805f9b34fb");
        public static Guid GuidHeartrate = new Guid("00002A37-0000-1000-8000-00805f9b34fb");
        public static Guid GuidStepsData = new Guid("00000007-0000-3512-2118-0009af100700");
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

                                    hrTicker = new Timer(((dt) =>
                                    {

                                        HrControl.Write(new byte[] { 0x16 });
                                    }), null, 1, 2000);
                                    await characteristic.Write(new byte[] { 0x15, 1, 1 });


                                }
                                else
                                if (characteristic.Uuid == GuidHeartrate.ToString())
                                {

                                    characteristic.Notify().Subscribe(res =>
                                    {
                                        HeartBeatRecieved?.Invoke(res.Data[1]);
                                    });
                                }
                                else
                                if (characteristic.Uuid == GuidStepsData.ToString())
                                {
                                    characteristic.Notify().Subscribe(res =>
                                    {
                                       var steps = ((((res.Data[1] & 255) | ((res.Data[2] & 255) << 8))));
                                       var distance = ((((res.Data[5] & 255) | ((res.Data[6] & 255) << 8)) | (res.Data[7] & 16711680)) | ((res.Data[8] & 255) << 24));
                                       var calories = ((((res.Data[9] & 255) | ((res.Data[10] & 255) << 8)) | (res.Data[11] & 16711680)) | ((res.Data[12] & 255) << 24));


                                        StepsDataRecieved?.Invoke(steps,calories,distance);
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
