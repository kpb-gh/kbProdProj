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
            int numTargets;
            List<string> data = new List<string>();
            List<Node> nodes = new List<Node> { new Node(0, 0, 0) };
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    for (int i = 1; !sr.EndOfStream; i++)
                    {
                        string line = sr.ReadLine();
                        if (line != null && line.Length > 0)
                        {
                            data.Add(line);
                            nodes.Add(new Node(i, 0, 0));
                        }
                    }
                }
            } catch (FileNotFoundException ex) { throw new FileNotFoundException($"File not found: {path}.", ex); }
            for (int i = 0; i < data.Count; i++)
            {
                string[] line = data[i].Split(",");
                if (line != null && line.Length > 0)
                {
                    nodes[i+1] = CreateNode(i+1, line, nodes);
                }
            }
            nodes[0].addTarget(nodes[1]);
            return nodes;
        }

        private static Node CreateNode(int id, string[] line, List<Node> nodes)
        {
            int x = int.Parse(line[0]); int y = int.Parse(line[1]);
            return new Node(id, x*3, y*3, GetTargetsFromLine(line, nodes));
        }

        private static List<Node> GetTargetsFromLine(string[] line, List<Node> nodes)
        {
            // ignore 0, 1
            List<Node> targets = new List<Node>();
            for (int i = 2; i < line.Length; i++)
            {
                int tgt = int.Parse(line[i]);
                var b = nodes.Find(a => a.id == tgt);
                if (b != null) {  targets.Add(b); }
            }
            if (targets.Count == 0) { throw new ArgumentException($"Failure to get targets. No matching targets. Line:{line}"); }
            return targets;
        }
    }
}
