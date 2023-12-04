using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbProdProj
{
    internal class VehicleHelper
    {
        const double PI = Math.PI;
        public static int ConvertAngle(float rads)
        {
            return (int)(180 * rads / PI);
        }
        public static double ConvertAngle(int degs)
        {
            return PI * degs / 180;
        }
    }
}
