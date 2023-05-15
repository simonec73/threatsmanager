using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MicroSvcsThreatsMapper.RibbonExtensions
{
    class MicroSvcsLoader
    {
        public class MSvcProject
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("nodes")]
            public List<MSvcNode> Nodes { get; set; }
        }

        public class MSvcNode
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("pods")]
            public List<string> Pods { get; set; }
        }

        public void PrettyPrint(List<MSvcProject> microsvcs)
        {
            //Console.WriteLine($"Projects: {microsvcs}");
            foreach (var project in microsvcs)
            {
                Console.WriteLine($"Project Name: {project.Name}");
                //Console.WriteLine($" Nods: {project.Nodes}");
                foreach (var node in project.Nodes)
                {
                    Console.WriteLine($"  Node Name: {node.Name}");
                    //Console.WriteLine($"  Pods: {node.Pods}");
                    foreach (var pod in node.Pods)
                    {
                        Console.WriteLine($"   Pod name: {pod}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public List<MSvcProject> LoadJSONFile(string input_file)
        {
            using (StreamReader file = File.OpenText(input_file))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<MSvcProject> microsvcs = (List<MSvcProject>)serializer.Deserialize(file, typeof(List<MSvcProject>));
                return microsvcs;
            }
        }
    }
}
