using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return (v.Angle - result) / v.TurnRate;
        }
        internal static int Time_Brake(Vehicle v)
        {
            int final = (int)(Math.Sqrt((v.velocity[0] * v.velocity[0]) + v.velocity[1] * v.velocity[1]));
            return (final - 2) / v.PwrRate;
        }

        internal static int Time_ReachNode(Node tn, Vehicle v)
        {
            return (int)(v.velocity[0] / Math.Abs(v.self.Margin.Left - tn.self.Margin.Left));
        }

        internal static List<Node>? FindRoute(Node tn, Node cn, List<Node>? route = null, List<Node>? deadNodes = null)
        {
            if (route == null) { route = new List<Node> { cn }; }
            else { route.Add(cn); }
            if (deadNodes == null) { deadNodes = new List<Node> { }; }
            else if (deadNodes.Contains(cn)) { return null; }
            if (cn == tn) { return route; }
            else
            {
                foreach (var node in cn.targets)
                {
                    Debug.WriteLine($"Investigating {node.id}");
                    route = FindRoute(tn, node, route, deadNodes);
                    if (route == null)
                    {
                        deadNodes.Add(node);
                        continue;
                    } 
                }
                return route;
            }
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
            int velo = (int)Math.Sqrt((vehicle.velocity[0] * vehicle.velocity[0]) + (vehicle.velocity[1] * vehicle.velocity[1]));
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
                if (DriverMath.Time_ReachNode(tn, vehicle) - (vehicle.MaxSpeed / vehicle.PwrRate) < 5 && Math.Abs(velo - vehicle.MaxSpeed) < vehicle.MaxSpeed / 10)
                {
                    vehicle.Brake();
                }
                else if (DriverMath.Time_Brake(vehicle) > 60 && vehicle.flags[1])
                {
                    vehicle.Neutral();
                }
                int t = DriverMath.Time_TurnToTarget(tn, vehicle);
                if (Math.Abs(t) > 2)
                {
                    if (t < 0) { vehicle.TurnLeft(); }
                    else { vehicle.TurnRight(); }
                }
            }
        }
    }
}
