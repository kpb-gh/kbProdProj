using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace kbProdProj
{
    class Node
    {
        public long id { get; } // id=0 is reserved for dead ends
        private long x { get; }
        private long y { get; }
        public bool active { get; set; }
        public List<Node> targets { get; } = new List<Node>();
        public Node(Node self, Node target)
        {
            id = self.id;
            x = self.x;
            y = self.y;
            addTarget(target);
        }
        public Node(Node self, Node[] target)
        {
            id = self.id;
            x = self.x;
            y = self.y;
            foreach (Node t in target)
            {
                addTarget(t);
            }
        }

        public void addTarget(Node target)
        {
            if (target.id == id) { throw new ArgumentException($"Failure to add id={id}. Cannot add self."); }
            targets.Add(target);
        }
    }
}
