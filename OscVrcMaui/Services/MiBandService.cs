using System;
using System.Collections.Generic;
using System.Text;

namespace OscVrcMaui.Services
{
   public class MiBandService
    {


   

        public delegate void UserStatusHandler(bool isSleep);
        public event UserStatusHandler SleepStatusChanged;
        public bool isUserSleeping { get; set; }
        
        public MiBandService()
        {

        }
       
        public void SetSleepStatus(bool isSleep)
        {
           SleepStatusChanged?.Invoke(isSleep);
            isUserSleeping = isSleep;
        }
    }
}
