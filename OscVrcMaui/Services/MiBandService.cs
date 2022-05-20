using System;
using System.Collections.Generic;
using System.Text;

namespace OscVrcMaui.Services
{
   public class MiBandService
    {
        public delegate void HeartBeatHandler(int beat);
        public event HeartBeatHandler HeartBeatRecieved;

        public delegate void StepsHandler(int steps);
        public event StepsHandler StepsRecieved;

        public delegate void UserStatusHandler(bool isSleep);
        public event UserStatusHandler SleepStatusChanged;
        public bool isUserSleeping { get; set; }
        public int heartBeat { get; set; } = 0;
        public int steps { get; set; } = 0;
        
        public MiBandService()
        {

        }
        public void SetSteps(int _steps)
        {
            StepsRecieved?.Invoke(_steps);
            steps = _steps;
        }
        public void SetHeartBeat(int beat) {
            HeartBeatRecieved?.Invoke(beat);
            heartBeat = beat;
        }
        public void SetSleepStatus(bool isSleep)
        {
           SleepStatusChanged?.Invoke(isSleep);
            isUserSleeping = isSleep;
        }
    }
}
