using Rug.Osc;
using OscVrcMaui.Models;
using OscVrcMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;using Microsoft.Maui.Controls;
using OscVrcMaui.Utils;
using Shiny.BluetoothLE;

namespace OscVrcMaui.ViewModels
{
    public class OscViewModel : BaseViewModel
    {
       
        public OSCService osc => DependencyService.Get<OSCService>();
        public DeviceSensorsService deviceSensors => DependencyService.Get<DeviceSensorsService>();
        public BLEService bleService => DependencyService.Get<BLEService>();
        public ObservableLimited<String> Messages { get; set; }
        public ObservableCollection<IPeripheral> Devices { get; set; }
        public IPeripheral SelectedDevice { get; set; }

        public string ip = "";

        public string Ip
        {
            get => ip;
            set => SetProperty(ref ip, value);
        }

        public Command ScanCommand { get; }
        public Command SaveCommand { get; }
        public Command StartCommand { get; }
        public Command ResetSensors { get; }
        public Command StopCommand { get; }


        public OscViewModel()
        {
            Messages = new ObservableLimited<string>();
            Messages.Enqueue("Application started...");
            Devices = new ObservableCollection<IPeripheral>();
             Ip = configService.LoadConfig().VRCHostIp;

            ScanCommand = new Command(async () => { Devices.Clear(); bleService.ScanBle(); });
            bleService.ScanDone += BleService_ScanDone;
            StartCommand = new Command(async () => await StartSendingBandData());
            SaveCommand = new Command(async () => {

                var config = configService.LoadConfig();
                config.SelectedDeviceId = SelectedDevice.Uuid;
                configService.WriteConfig(config);


            });
            StopCommand = new Command(async () => {

                DependencyService.Get<IForegroundWorker>().StopWorker();

            });
            ResetSensors = new Command(async () => { deviceSensors.Reset(); });
    
        }

        private void BleService_ScanDone(List<IPeripheral> devices)
        {

            foreach (var item in devices)
            {
                Devices.Add(item);
            }
        }

        public void OnAppearing() { 
        
        
        
        
        
        }

    

        async Task StartSendingBandData()
        {
            Messages.Enqueue("Starting...");
              var config = configService.LoadConfig();
           config.VRCHostIp = Ip;
            configService.WriteConfig(config);
            DependencyService.Get<IForegroundWorker>().RestartWorker();
            DependencyService.Get<OSCMainService>().UpdatesLogQueue = Messages;

        }
    }
}
