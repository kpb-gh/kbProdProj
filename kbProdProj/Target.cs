using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbProdProj
{
    internal class Target
    {
        ///<summary>Data class for Nodes to target other Nodes.</summary>
        ///
        public long id { get; }
        public long x { get; }
        public long y { get; }
        public bool active { get; set; } // determines if routable
        public Target(long id, long x, long y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }
    }
}
