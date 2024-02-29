using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Xml.Linq;

namespace kbProdProj
{
    internal static class MapLoader
    {
        public static List<Node> GetNodesFromFile(string path = "list.nodes")
        {
            List<Node> nodes = new List<Node>();
            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    string[] line = sr.ReadLine().Split(",");
                    if (line != null && line.Length > 0) {
                        nodes.Add(CreateNode(i, line, nodes));
                    }
                }
            }
            return nodes;
        }

        private static Node CreateNode(long id, string[] line, List<Node> nodes)
        {
            long x = long.Parse(line[0]); long y = long.Parse(line[1]);
            return new Node(id, x, y, GetTargetsFromLine(line, nodes));
        }

        private static List<Node> GetTargetsFromLine(string[] line, List<Node> nodes)
        {
            // ignore 0, 1
            List<Node> targets = new List<Node>();
            for (int i = 2; i < line.Length; i++)
            {
                long tgt = long.Parse(line[i]);
                var b = nodes.Find(a => a.id == tgt);
                if (b != null) {  targets.Add(b); }
            }
            if (targets.Count == 0) { throw new ArgumentException($"Failure to get targets. No matching targets. Line:{line}"); }
            return targets;
        }
    }
}
