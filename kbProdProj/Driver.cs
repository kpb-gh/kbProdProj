using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbProdProj
{
    internal static class Driver
    {
        internal static int Time_TurnToTarget(Node tn, Vehicle v)
        {
            if (tn == null || v == null) return 0;
            Point[] points = new Point[3] { 
                new Point((int)v.self.Margin.Left, (int)v.self.Margin.Top), 
                new Point((int)v.self.Margin.Left, (int)v.self.Margin.Top - 10), 
                new Point((int)tn.self.Margin.Left, (int)tn.self.Margin.Top) };
            int result = (int)(
                Math.Atan2(points[2].Y - points[0].Y, points[2].X - points[0].X) -
                    Math.Atan2(points[1].Y - points[0].Y, points[1].X - points[0].X)
                * Math.PI);
            if (result < 0) { result += 360; }
            return (result / v.TurnRate);
        }
        internal static int Time_Brake(Node tn, Vehicle v)
        {
            int final = (int)(Math.Sqrt((v.velocity[0] * v.velocity[0]) + v.velocity[1] * v.velocity[1]));
            return (final - 2) / v.PwrRate;
        }
    }
}
