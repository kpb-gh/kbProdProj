using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbProdProj
{
    internal static class DriverMath
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
        internal static int Time_Brake(Vehicle v)
        {
            int final = (int)(Math.Sqrt((v.velocity[0] * v.velocity[0]) + v.velocity[1] * v.velocity[1]));
            return (final - 2) / v.PwrRate;
        }
    }

    internal class Driver {
        private Vehicle vehicle;
        private List<Node> route { get; set; }
        private Node tn { get; set; }

        public Driver(Vehicle vehicle, List<Node> route, Node tn)
        {
            this.vehicle = vehicle;
            this.route = route;
            this.tn = tn;
        }

        public void DriveAI()
        {
            if (Math.Abs(vehicle.self.Margin.Top - tn.self.Margin.Top) > 10 || (Math.Abs(vehicle.self.Margin.Left - tn.self.Margin.Left) > 10))
            {
                route.RemoveAt(0);
                if (route.Count > 0) { tn = route[0]; }
                else 
                {
                    vehicle.Brake();
                    return;
                }
            }
            else
            {
                if (true) // temp
                {
                    vehicle.Brake();
                }
                else if (DriverMath.Time_Brake(vehicle) > 60 && vehicle.flags[1])
                {
                    vehicle.Neutral();
                }
            }
        }
    }
}
