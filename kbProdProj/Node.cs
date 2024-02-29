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
        public long id { get; } // id=0 is reserved for root node
        private long x { get; }
        private long y { get; }
        public bool active { get; set; }
        public List<Node> targets { get; } = new List<Node>();
        public Node(long id, long x, long y, Node target)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            addTarget(target);
        }
        public Node(long id, long x, long y, List<Node> target)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            foreach (Node t in target)
            {
                addTarget(t);
            }
        }
        public Node(long id, long x, long y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public void addTarget(Node target)
        {
            if (target.id == id) { throw new ArgumentException($"Failure to add id={id}. Cannot add self."); }
            targets.Add(target);
        }
    }
}
