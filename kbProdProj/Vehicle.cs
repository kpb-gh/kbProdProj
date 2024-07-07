using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace kbProdProj
{
    internal class Vehicle
    {
        public Rectangle self = new();
        internal double[] velocity = new double[] { 0, 0 };
        internal bool[] flags = new bool[8] { false, false, false, false, false, false, false, false }; // reversing, accel, brake, left, right, hazard, l_ind, r_ind
        public double TurnRate { get; } = 3;
        public double PwrRate { get; } = 2;
        public double MaxSpeed { get; } = 6;
        public bool Ovrrd { get; }
        public double Angle { get; set; } = 0;
        public Node? CurrentLocation { get; set; }

        public Vehicle(int[] c, Brush col, int type, bool o = false)
        {
            Ovrrd = o;
            int[] size;
            switch (type)
            {
                case 0:
                    size = new int[] { 6, 16 }; // car
                    break;
                case 1:
                    size = new int[] { 4, 10 }; // bike
                    break;
                case 2:
                    throw new NotImplementedException();
                    size = new int[] { };
                    break;
                case 3:
                    throw new NotImplementedException();
                    size = new int[] { };
                    break;
                default:
                    throw new ArgumentException("new Vehicle(..) needs valid type.");
            }
            Initialise(c, col, size);
        }

        private void Initialise(int[] c, Brush col, int[] s)
        {
            self.Margin = new Thickness(c[0], c[1], 0, 0); // top left if c ~= {0,0}
            self.Width = s[0];
            self.Height = s[1];
            self.Fill = col;
            self.StrokeThickness = 0;
            self.HorizontalAlignment = HorizontalAlignment.Center;
            self.VerticalAlignment = VerticalAlignment.Center;

        }

        public void Update()
        {
            double final = Math.Sqrt((velocity[0] * velocity[0]) + velocity[1] * velocity[1]);
            double dAngle = 0;
            // engine
            if (flags[1] && final < MaxSpeed)
            {
                final += PwrRate;
            }
            else if (flags[2])
            {
                if (Math.Abs(final) <= PwrRate) { final = 0; }
                else { final -= PwrRate; }
            }
            // reduce final due to extra drag on turn
            if ((flags[3] || flags[4]) && Math.Abs(final) > 0.2)
            {
                final /= 1.01;
                // rotate vehicle
                if (flags[3])
                {
                    dAngle = -TurnRate;
                }
                if (flags[4])
                {
                    dAngle = TurnRate;
                }
            }
            // reduce final due to overall drag
            final /= 1.05;
            // transform angle
            Angle += dAngle;
            if (Angle < 0) { Angle += 360; }
            else if (Angle > 360) { Angle -= 360; }
            var tf = new RotateTransform(Angle, self.Width / 2, self.Height / 2);
            // calc velocity using final
            velocity[0] = 0 - Math.Round(final * Math.Sin(VehicleHelper.ConvertAngle(Angle)));
            velocity[1] = Math.Round(final * Math.Cos(VehicleHelper.ConvertAngle(Angle)));
            // transform velocity
            self.Margin = new Thickness(self.Margin.Left + velocity[0], self.Margin.Top + velocity[1], 0, 0);
            // transform final
            self.RenderTransform = tf;
        }

        public void TurnLeft() { flags[3] = true; flags[4] = false; }
        public void TurnRight() { flags[4] = true; flags[3] = false; }
        public void Accel() { flags[1] = true; flags[2] = false; }
        public void Brake() { flags[2] = true; flags[1] = false; }
        public void Neutral() { flags[1] = flags[2] = flags[3] = flags[4] = false; }
    }
}
