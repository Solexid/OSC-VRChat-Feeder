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
using OscVrcMaui.Views;

namespace OscVrcMaui.ViewModels
{
    public class OscViewModel : BaseViewModel
    {
       
        public OSCService osc => DependencyService.Get<OSCService>();
        public DeviceSensorsService deviceSensors => DependencyService.Get<DeviceSensorsService>();
       
        public ObservableLimited<String> Messages { get; set; }

        public string ip = "";

        public string Ip
        {
            get => ip;
            set => SetProperty(ref ip, value);
        }

        public Command SelectDevice { get; }
        public Command StartCommand { get; }
        public Command ResetSensors { get; }
        public Command StopCommand { get; }


        public OscViewModel()
        {
            Messages = new ObservableLimited<string>();
            Messages.Enqueue("Application started...");
           
             Ip = configService.LoadConfig().VRCHostIp;
            SelectDevice = new Command(async () => await Shell.Current.GoToAsync(nameof(DeviceSelectPage)));

            StartCommand = new Command(async () => await StartSendingBandData());
       
            StopCommand = new Command(async () => {

                DependencyService.Get<IForegroundWorker>().StopWorker();

            });
            ResetSensors = new Command(async () => { deviceSensors.Reset(); });
    
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
