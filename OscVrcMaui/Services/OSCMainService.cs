using OscVrcMaui.Models;
using OscVrcMaui.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OscVrcMaui.Services
{
    //Main service that notified by sensors data and sends to host
    public class OSCMainService
    {
        public ProfileDataStore ProfileStore => DependencyService.Get<ProfileDataStore>();
        public ConfigService configService => DependencyService.Get<ConfigService>();
        public OSCService osc => DependencyService.Get<OSCService>();
        public DeviceSensorsService deviceSensors => DependencyService.Get<DeviceSensorsService>();
        public MiBandService band => DependencyService.Get<MiBandService>();
        public BLEService bleHR => DependencyService.Get<BLEService>();
        public ObservableLimited<String> UpdatesLogQueue = new ObservableLimited<string>();

        public OSCMainService()
        {
           // osc.MessageReceived += MessageAdd;
            band.HeartBeatRecieved += HeartBeat;
            bleHR.HeartBeatRecieved += HeartBeat2;
            band.StepsRecieved += Steps;
            deviceSensors.RotationReceived += RotationSend;
            band.SleepStatusChanged += SleepChange;


        }


        async void RotationSend(Vector3 rot)
        {



            var normalizedRot = new Vector3(rot.X / MathF.PI, rot.Y / MathF.PI, rot.Z / MathF.PI);
            
            var profilesX = await ProfileStore.GetItemsByTypeAsync(InputType.DeviceRotationX);
            var profilesY = await ProfileStore.GetItemsByTypeAsync(InputType.DeviceRotationY);
            var profilesZ = await ProfileStore.GetItemsByTypeAsync(InputType.DeviceRotationZ);

            foreach (var item in profilesX)
            {

                await osc.SendFloatAsync(item.RootPath + item.ParameterName, normalizedRot.X);


            }
            foreach (var item in profilesY)
            {

                await osc.SendFloatAsync(item.RootPath + item.ParameterName, normalizedRot.Y);

            }
            foreach (var item in profilesZ)
            {
                await osc.SendFloatAsync(item.RootPath + item.ParameterName, normalizedRot.Z);


            }




        }


        async void HeartBeat(int _beat)
        {
            var profiles = await ProfileStore.GetItemsByTypeAsync(InputType.HeartRate);
            foreach (var item in profiles)
            {
                if (item.Normalize)
                {
                    var newbeat = _beat - item.MinValue;
                    if (item.MaxValue < item.MinValue) item.MaxValue = item.MinValue + 1;
                    float result = Math.Clamp((float)newbeat, 0, item.MaxValue - item.MinValue);
                    result = result / (item.MaxValue - item.MinValue);
                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, result);
                }
                else { await osc.SendIntAsync(item.RootPath + item.ParameterName, _beat); }


            }

            AddUpdate("HR:" + _beat);
        }


        void AddUpdate(string log) {


            UpdatesLogQueue?.Enqueue(log);



        }
        async void HeartBeat2(int _beat)
        {
            var profiles = await ProfileStore.GetItemsByTypeAsync(InputType.HeartRate);
            foreach (var item in profiles)
            {
                if (item.Normalize)
                {
                    var newbeat = _beat - item.MinValue;
                    if (item.MaxValue < item.MinValue) item.MaxValue = item.MinValue + 1;
                    float result = Math.Clamp((float)newbeat, 0, item.MaxValue - item.MinValue);
                    result = result / (item.MaxValue - item.MinValue);
                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, result);
                }
                else { await osc.SendIntAsync(item.RootPath + item.ParameterName, _beat); }


            }



            AddUpdate("HR BLE:" + _beat);

        }

        async void Steps(int steps)
        {
            var profiles = await ProfileStore.GetItemsByTypeAsync(InputType.StepCount);
            foreach (var item in profiles)
            {
                if (item.Normalize)
                {
                    var newbeat = steps - item.MinValue;
                    if (item.MaxValue < item.MinValue) item.MaxValue = item.MinValue + 1;
                    float result = Math.Clamp((float)newbeat, 0, item.MaxValue - item.MinValue);
                    result = result / (item.MaxValue - item.MinValue);
                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, result);
                }
                else
                {
                    await osc.SendIntAsync(item.RootPath + item.ParameterName, steps);
                }


            }
            AddUpdate("Steps:" + steps);

        }

        async void SleepChange(bool isSleep)
        {

            var profiles = await ProfileStore.GetItemsByTypeAsync(InputType.SleepStatus);
            foreach (var item in profiles)
            {

                await osc.SendBoolAsync(item.RootPath + item.ParameterName, isSleep);
            }
            if (isSleep)
                AddUpdate( "You fell asleep");
            else
                AddUpdate( "You woke up");


        }
      public  async Task StartSendingBandDate()
        {
            deviceSensors.SetRotationSensorStatus(await ProfileStore.HasActiveInputs(InputType.DeviceRotationX, InputType.DeviceRotationY, InputType.DeviceRotationZ));


            var config = configService.LoadConfig();

            bleHR.InitConfig();
            osc.SetIpAndRestart(config.VRCHostIp);
            AddUpdate("Sending Band data is started...");

        }

    }
}
