using System;
using System.IO;
using System.Linq;
using ThreatsManager.Mitre;
using ThreatsManager.Mitre.Graph;

namespace MitreImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (Directory.Exists(args[0]))
                {
                    Console.WriteLine("Threats Manager Platform - MITRE Importer");

                    var graph = new MitreGraph();

                    var cweFile = Directory.GetFiles(args[0], "cwe*.xml", SearchOption.TopDirectoryOnly)?
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
                    if (cweFile != null)
                    {
                        var cwe = File.ReadAllText(cweFile);
                        var cweEngine = new CweEngine(cwe);
                        cweEngine.EnrichGraph(graph);
                    }

                    var capecFile = Directory.GetFiles(args[0], "capec*.xml", SearchOption.TopDirectoryOnly)?
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
                    if (capecFile != null)
                    {
                        var capec = File.ReadAllText(capecFile);
                        var capecEngine = new CapecEngine(capec);
                        capecEngine.EnrichGraph(graph);
                    }

                    var attackFile = Directory.GetFiles(args[0], "enterprise-attack.json", SearchOption.TopDirectoryOnly)?
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
                    if (attackFile != null)
                    {
                        var attack = File.ReadAllText(attackFile);
                        var attackEngine = new AttackEngine("Enterprise ATT&CK", "1.0", attack);
                        attackEngine.EnrichGraph(graph);
                    }

                    graph.ReconcileRelationships();
                    Print(graph);

                    var path = Path.Combine(args[0], "MitreGraph.json");

                    graph.Serialize(path);

                    Console.WriteLine($"Created file {path}.");
                }
                else
                {
                    throw new DirectoryNotFoundException();
                }
            }
        }

        private static void Print(MitreGraph graph)
        {
            Console.WriteLine($"Sources: {graph.Sources.Count}.");
            Console.WriteLine($"Nodes: {graph.Nodes.Count}.");
            Console.WriteLine($"- Attack Patterns: {graph.Nodes.OfType<AttackPatternNode>().Count()}.");
            Console.WriteLine($"- Categories: {graph.Nodes.OfType<CategoryNode>().Count()}.");
            Console.WriteLine($"- Externals: {graph.Nodes.OfType<ExternalNode>().Count()}.");
            Console.WriteLine($"- Views: {graph.Nodes.OfType<ViewNode>().Count()}.");
            Console.WriteLine($"- Weaknesses: {graph.Nodes.OfType<WeaknessNode>().Count()}.");
            Console.WriteLine($"- Mitigations : {graph.Nodes.OfType<MitigationNode>().Count()}.");
        }
    }
}
