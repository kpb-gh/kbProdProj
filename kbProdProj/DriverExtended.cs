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

        public DriverExtended(Vehicle vehicle, Node sn, Node tn, in List<Node> map) : base(vehicle, sn, tn, map)
        {
            /// <summary>
            ///     The constructor for a regular Driver. Should not be changed without knowledge on how the driver will be altered.
            /// </summary>
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

        public new bool DriveAI()
        {
            bool value = base.DriveAI();
            if (value) { vehicle.Accel(); } // only to show the vehicle behaves differently, as it drives off into the middle of nowhere
            return value;
        }
    }
}
