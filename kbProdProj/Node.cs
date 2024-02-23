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
        private long id { get; set; } // id=0 is reserved for dead ends
        private long x { get; }
        private long y { get; }
        private List<Target> targets { get; } = new List<Target>();
        public Node(Target self, Target target)
        {
            id = self.id;
            x = self.x;
            y = self.y;
            addTarget(target);
        }
        public Node(Target self, Target[] target)
        {
            id = self.id;
            x = self.x;
            y = self.y;
            foreach (Target t in target)
            {
                addTarget(t);
            }
        }

        public void addTarget(Target target)
        {
            if (target.id == id) { throw new ArgumentException($"Failure to add id={id}. Cannot add self."); }
            targets.Add(target);
        }
    }
}
