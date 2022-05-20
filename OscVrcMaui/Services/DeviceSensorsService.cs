using OscVrcMaui.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace OscVrcMaui.Services
{
  public  class DeviceSensorsService
    {
        public Vector3 lastRotation;
        private Quaternion lastQuat;
        private Quaternion resetQuat = Quaternion.Identity;
        private bool resetTime = true;
        SensorSpeed speed = SensorSpeed.UI;
        public delegate void RotationChangedHandler(Vector3 data);
        public event RotationChangedHandler RotationReceived;
        public DeviceSensorsService()
        {
            OrientationSensor.ReadingChanged += Rotationchanged;

        }
        public void SetRotationSensorStatus(bool isStart) {

            try
            {
                if (!isStart && OrientationSensor.IsMonitoring)
                    OrientationSensor.Stop();
                else
                    OrientationSensor.Start(speed);

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }



        }

        public void Reset() {

            resetQuat = lastQuat;
        }
        private void Rotationchanged(object sender, OrientationSensorChangedEventArgs e) {


            var data = e.Reading;
           // Console.WriteLine($"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");
            lastQuat = data.Orientation;
            if (resetTime)
            {


                resetTime = false;

                resetQuat = lastQuat;

            }

            var result_quat = resetQuat * Quaternion.Inverse(data.Orientation);
  
            lastRotation =MathUtils.ToEulerAngles(result_quat);
            
      
            RotationReceived?.Invoke(lastRotation);

        }



    }



}
