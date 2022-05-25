using System;
using System.Collections.Generic;
using System.Text;

namespace OscVrcMaui.Models
{
   public class Profile
    {
        public String Id { get; set; }
        public bool isActive { get; set; }
        public String ProfileName { get; set; }
        public InputType Input { get; set; }
        public String RootPath { get; set; }
        public String ParameterName { get; set; }
        public bool Normalize{ get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public string LastValue { get; set; }
        public DateTime CreationDate { get; set; }

    }
    public enum InputType
    {
        None,//StaticValue,for a sample
        HeartRate,
        SleepStatus,
        StepCount,
        DeviceRotationX,
        DeviceRotationY,
        DeviceRotationZ,
        TouchButtonControl,
        RadialTouchControl,
        Calories,
        Distance
    }
}
