using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace kbProdProj
{
    internal static class DriverMath
    {
        internal static double Angle_ToNode(Node tn, Vehicle v)
        {
            if (tn == null || v == null) return 0;
            Point[] points = new Point[3] { 
                new Point((int)v.self.Margin.Left, (int)v.self.Margin.Top), 
                new Point((int)v.self.Margin.Left, (int)v.self.Margin.Top - 10), 
                new Point((int)tn.self.Margin.Left, (int)tn.self.Margin.Top) };
            double result = -180 * (
                Math.Atan2(points[2].Y - points[0].Y, points[2].X - points[0].X) -
                    Math.Atan2(points[1].Y - points[0].Y, points[1].X - points[0].X)) / Math.PI;
            while (result <= 0) { 
                result += 360; 
            }
            return result;
        }

        internal static double Time_TurnToTarget(Node tn, Vehicle v)
        {
            double diff = v.Angle - Angle_ToNode(tn, v);
            bool turnLeft = (diff + 360) % 360 < 180;
            if (turnLeft) { return Math.Abs(diff / v.TurnRate); }
            else { return -Math.Abs(diff / v.TurnRate); }
        }
        internal static double Time_Brake(Vehicle v)
        {
            double final = Math.Sqrt((v.velocity[0] * v.velocity[0]) + v.velocity[1] * v.velocity[1]);
            return (final / v.PwrRate) - 1;
        }

        internal static double Time_MaxBrake(Vehicle v)
        {
            return (v.MaxSpeed / v.PwrRate) - 1;
        }

        internal static double Time_ReachNode(Node tn, Vehicle v)
        {
            double tv = v.velocity[0] / Math.Abs(v.self.Margin.Left - tn.self.Margin.Left);
            double ty = v.velocity[1] / Math.Abs(v.self.Margin.Top - tn.self.Margin.Top);
            return Math.Sqrt((tv * tv) + (ty * ty));
        }

        internal static List<Node>? FindRoute(Node tn, Node cn, in List<Node> map, List<Node>? deadNodes = null, List<Node> ? route = null)
        {
            ///<summary>
            ///Returns a route that connects nodes via their targets. Map is only passed in to correct inconsistencies, and is ref to reduce strain of passing a large variable
            ///</summary>
            if (route == null) { route = new List<Node> { cn }; }
            else { route.Add(cn); }
            if (deadNodes == null) { deadNodes = new List<Node>(); }
            else if (deadNodes.Contains(cn)) { return null; }
            if (cn == tn) { return route; }
            else
            {
                cn = map[cn.id];
                foreach (var node in cn.targets)
                {
                    bool flag = false;
                    foreach (Node visited in route)
                    {
                        if (visited.id == node.id) { flag = true; continue; }
                    }
                    if (flag) { continue; }
                    Debug.WriteLine($"Investigating {node.id}");
                    route = FindRoute(tn, node, map, deadNodes, route);
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

        public Driver(Vehicle vehicle, Node sn, Node tn, in List<Node> map)
        {
            this.vehicle = vehicle;
            route = DriverMath.FindRoute(tn, sn, map);
            if (route == null)
            {
                route = new List<Node> { sn };
            } else { route.Add(tn); }
            this.tn = tn;
            Debug.Write($"DriveAI_{GetHashCode()}: Initialising. Route: ");
            for (int i = 0; i < route.Count; i++)
            {
                Debug.Write($"{route[i].id}, ");
            }
            Debug.Write("\n");
        }

        private void TurnLeft()
        {
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Left."); 
            vehicle.TurnLeft();
        }
        private void TurnRight()
        {
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Right.");
            vehicle.TurnRight();
        }
        private void Brake()
        {
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Braking.");
            vehicle.Brake();
        }

        private void Accel()
        {
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Accelerating.");
            vehicle.Accel();
        }

        private void Neutral()
        {
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Neutral.");
            vehicle.Neutral();
        }

        public void DieSafely()
        {
            /// <summary>
            ///     Neutralises vehicle steering and forces it to stop. To be used before Driver is killed.
            /// </summary>
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Dying safely.");
            vehicle.Neutral();
            vehicle.Brake();
        }

        public bool DriveAI()
        {
            /// <summary>
            ///     Controls the Driver's assigned Vehicle automatically. Returns true when done, otherwise returns false.
            /// </summary>
            tn = route[0];
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Navigation to {tn.id}. Final: {route[^1].id}");
            if (route.Count == 0) 
            {
                Debug.WriteLine($"DriveAI_{GetHashCode()}: Done arrival.");
                return true; 
            }
            double velo = Math.Sqrt((vehicle.velocity[0] * vehicle.velocity[0]) + (vehicle.velocity[1] * vehicle.velocity[1]));
            if (Math.Abs(vehicle.self.Margin.Top - tn.self.Margin.Top) < 10 && (Math.Abs(vehicle.self.Margin.Left - tn.self.Margin.Left) < 10))
            {
                vehicle.CurrentLocation = route[0];
                route.RemoveAt(0);
                if (route.Count > 0) 
                {
                    tn = route[0];
                    Debug.WriteLine($"DriveAI_{GetHashCode()}: New target: {tn.id}");
                }
                else 
                {
                    Debug.WriteLine(
                        $"DriveAI_{GetHashCode()}: Done proximity.\n" +
                        $"Coords (v): {vehicle.self.Margin.Left}/{vehicle.self.Margin.Top}\n" +
                        $"Coords (t): {tn.self.Margin.Left}/{tn.self.Margin.Top}");
                    vehicle.Brake();
                    return true;
                }
            }
            else
            {
                double t = DriverMath.Time_TurnToTarget(tn, vehicle);
                if (Math.Abs(t) > 2)
                {
                    if (t < 0) { TurnLeft(); }
                    else { TurnRight(); }
                }
                else
                {
                    Neutral();
                }
                if (DriverMath.Time_ReachNode(tn, vehicle) - (vehicle.MaxSpeed / vehicle.PwrRate) < 5 && Math.Abs(velo - vehicle.MaxSpeed) < vehicle.MaxSpeed / 10)
                {
                    Brake();
                }
                else if (DriverMath.Time_MaxBrake(vehicle) > DriverMath.Time_ReachNode(tn, vehicle) + 1)
                {
                    Accel();
                }
            }
            Debug.WriteLine($"DriveAI_{GetHashCode()}: Ongoing.");
            return false;
        }
    }
}
