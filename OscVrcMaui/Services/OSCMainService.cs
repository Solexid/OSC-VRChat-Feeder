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
        public BLEService bleService => DependencyService.Get<BLEService>();
        public ObservableLimited<String> UpdatesLogQueue = new ObservableLimited<string>();

        public OSCMainService()
        {
           //osc.MessageReceived += MessageAdd;
            bleService.HeartBeatRecieved += HeartBeat;
            bleService.StepsDataRecieved += Steps;

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


      async  void AddUpdate(string log) {


             Device.InvokeOnMainThreadAsync(async () => { UpdatesLogQueue?.Enqueue(log); }); 



        }

        async void Steps(int steps,int calories=-1,int distance=-1)
        {
            var profiles = await ProfileStore.GetItemsByTypeAsync(InputType.StepCount);
            var profiles_calories = await ProfileStore.GetItemsByTypeAsync(InputType.Calories);
            var profiles_distance = await ProfileStore.GetItemsByTypeAsync(InputType.Distance);

            foreach (var item in profiles)
            {
                if (item.Normalize)
                {
                
                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, ClampAndNormalize(item.MinValue,item.MaxValue,steps));
                }
                else
                {
                    await osc.SendIntAsync(item.RootPath + item.ParameterName, steps);
                }


            }
            foreach (var item in profiles_calories)
            {
                if (item.Normalize)
                {

                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, ClampAndNormalize(item.MinValue, item.MaxValue, calories));
                }
                else
                {
                    await osc.SendIntAsync(item.RootPath + item.ParameterName, steps);
                }


            }
            foreach (var item in profiles_distance)
            {
                if (item.Normalize)
                {

                    await osc.SendFloatAsync(item.RootPath + item.ParameterName, ClampAndNormalize(item.MinValue, item.MaxValue, distance));
                }
                else
                {
                    await osc.SendIntAsync(item.RootPath + item.ParameterName, steps);
                }


            }
            AddUpdate("Steps:" + steps);
            AddUpdate("Calories:" + calories);
            AddUpdate("Distance:" + distance);

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
        float ClampAndNormalize(int min, int max, float value) {

            var newres = value - min;
            if (max < min) max = min + 1;
            float result = Math.Clamp((float)newres, 0, max - min);
            result = result / (max - min);
            return result;

        }
      public  async Task StartSendingBandDate()
        {
            deviceSensors.SetRotationSensorStatus(await ProfileStore.HasActiveInputs(InputType.DeviceRotationX, InputType.DeviceRotationY, InputType.DeviceRotationZ));


            var config = configService.LoadConfig();

            bleService.InitConfig();
            osc.SetIpAndRestart(config.VRCHostIp);
            AddUpdate("Sending Band data is started...");

        }

    }
}
