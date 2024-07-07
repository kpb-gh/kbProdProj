using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace kbProdProj
{
    internal class DriverExtended : Driver {
        private Vehicle vehicle;
        private List<Node> route { get; set; }
        private Node tn { get; set; }

        private StreamWriter? SWriter { get; }

        public DriverExtended(Vehicle vehicle, Node sn, Node tn, in List<Node> map) : base(vehicle, sn, tn, map)
        {
            this.vehicle = vehicle;
            route = DriverMath.FindRoute(tn, sn, map);
            if (route == null)
            {
                route = new List<Node> { sn };
            }
            this.tn = tn;
            SWriter = SetupWriter();
            Debug.Write($"mDriveAI_{GetHashCode()}: Initialising. Route: ");
            for (int i = 0; i < route.Count; i++)
            {
                Debug.Write($"{route[i].id}, ");
            }
            DWrite("Note: This DriveAI is the EXTENDED version");
        }

        public new StreamWriter? SetupWriter()
        {
            return base.SetupWriter();
        }

        public void DWrite(string s)
        {
            SWriter?.WriteLine(s);
            Debug.WriteLine($"mDriveAI_{GetHashCode()}: {s}");
        }

        private void TurnLeft()
        {
            // DWrite("Left."); 
            vehicle.TurnLeft();
        }
        private void TurnRight()
        {
            // DWrite("Right.");
            vehicle.TurnRight();
        }
        private void Brake()
        {
            // DWrite("Braking.");
            vehicle.Brake();
        }

        private void Accel()
        {
            // DWrite("Accelerating.");
            vehicle.Accel();
        }

        private void Neutral()
        {
            // DWrite("Neutral.");
            vehicle.Neutral();
        }

        public void DieSafely()
        {
            base.DieSafely();
        }

        public new bool DriveAI()
        {
            bool value = base.DriveAI();
            if (
                value
            ) 
            {
                vehicle.Accel();
            }
            return value;
        }
    }
}
