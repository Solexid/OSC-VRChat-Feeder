using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace OscVrcMaui.Utils
{
   public class MathUtils
    {
        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = Vector3.Zero;

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)(Math.Sign(sinp)*(Math.PI / 2 ));
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }
        public static float Normalize(float max, float value) {

          
            return value/max;
        }

    }
}
