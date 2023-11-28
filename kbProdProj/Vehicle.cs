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
        internal const double PI = Math.PI;
        internal int[] velocity = new int[] { 0, 0 };

        public Vehicle(int[] c, Brush col, int type) 
        {
            int[] size = new int[2];
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
            self.HorizontalAlignment = HorizontalAlignment.Left;
            self.VerticalAlignment = VerticalAlignment.Top;

        }
    }
}
