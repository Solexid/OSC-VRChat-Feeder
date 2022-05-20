using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OscVrcMaui.Models
{
   public class Config
    {
       public string TaskerPassword { get; set; }
       public string VRCHostIp { get; set; }
        public string SelectedDeviceId { get; set; }
        public List<Profile> Profiles { get; set; } 

        public Config()
        {
            Profiles  = new List<Profile>();

        }

    }
}
