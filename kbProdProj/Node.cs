using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using System.Windows;

namespace kbProdProj
{
    class Node
    {
        public Rectangle self = new();
        public int id { get; } // id=0 is reserved for root node
        private int x { get; }
        private int y { get; }
        public bool active { get; set; }
        public List<Node> targets { get; } = new List<Node>();

        private void Initialise()
        {
            self.Margin = new Thickness(x,y, 0, 0);
            self.Width = 30; self.Height = 30;
            self.Fill = new SolidColorBrush(Colors.Transparent);
            self.StrokeThickness = 0.5;
            self.Stroke = new SolidColorBrush(Colors.Black);
            self.HorizontalAlignment = HorizontalAlignment.Center;
            self.VerticalAlignment = VerticalAlignment.Center;

        }

        public Node(int id, int x, int y, List<Node> target)
        {
            this.id = id;
            this.x = (x * 4); this.y = (y * 4);
            Initialise();
            foreach (Node t in target)
            {
                addTarget(t);
            }
        }
        public Node(int id, int x, int y)
        {
            this.id = id;
            this.x = (x * 4); this.y = (y * 4);
            Initialise();
        }

        public void addTarget(Node target)
        {
            if (target.id == id) { throw new ArgumentException($"Failure to add id={id}. Cannot add self."); }
            targets.Add(target);
        }
    }
}
