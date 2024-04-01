using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace kbProdProj
{
    internal class Road
    {
        public Line self = new();
        public Node[] path = new Node[2];
        private Road? partner; // "road" lines are in pairs, to indicate width. nullable as it won't have value until the partner is made

        public Road(Node n1, Node n2, Road? partner = null)
        {
            self.X1 = (int)n1.self.Margin.Left;
            self.X2 = (int)n2.self.Margin.Left;
            self.Y1 = (int)n1.self.Margin.Top;
            self.Y2 = (int)n2.self.Margin.Top;
            this.partner = partner;
            Initialise();
        }
        private void Initialise()
        {
            if (partner != null)
            {
                partner.partner = this;
            }
            self.StrokeThickness = 0.3;
            self.Stroke = new SolidColorBrush(Colors.Black);
            self.HorizontalAlignment = HorizontalAlignment.Center;
            self.VerticalAlignment = VerticalAlignment.Center;

        }
    }
}
