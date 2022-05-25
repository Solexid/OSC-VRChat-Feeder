using OscVrcMaui.Services;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscVrcMaui.ViewModels
{
    class DeviceSelectViewModel : BaseViewModel
    {
        public ObservableCollection<IPeripheral> Devices { get; set; }
        public IPeripheral SelectedDevice { get; set; }
        public BLEService bleService => DependencyService.Get<BLEService>();
        public Command ScanCommand { get; }
        public Command SaveCommand { get; }
        public DeviceSelectViewModel()
        {
            Devices = new ObservableCollection<IPeripheral>();
            SaveCommand = new Command(async () => {

                var config = configService.LoadConfig();
                config.SelectedDeviceId = SelectedDevice.Uuid;
                configService.WriteConfig(config);
                await Shell.Current.GoToAsync("..");


            });
            ScanCommand = new Command(async () => 
            { 
                Devices.Clear();
                bleService.ScanBle();
                
            });

            bleService.ScanDone += (devices) => {

                foreach (var item in devices)
                {
                    Devices.Add(item);
                }

            };
        }
        public  void OnAppearing() {


            Devices.Clear();
            bleService.ScanBle();


        }
    }
}
