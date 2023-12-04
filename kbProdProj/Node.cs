using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbProdProj
{
    class Node
    {
        private long id { get; set; } // id=0 is reserved for dead ends
        private List<long> targets { get; }
        private bool oneWay { get; set; }
        public Node(long id, long target)
        {
            this.id = id;
            targets = new List<long>();
            addTarget(target);
        }
        public Node(long id, long[] target)
        {
            this.id = id;
            targets = new List<long>();
            foreach (long tid in target)
            {
                addTarget(tid);
            }
        }

        public void addTarget(long tid)
        {
            if (tid == id) { throw new ArgumentException($"Failure to add id={id}. Cannot add self."); }
            targets.Add(tid);
        }
    }
}
