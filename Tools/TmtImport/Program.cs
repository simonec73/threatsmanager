using System;
using System.IO;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Packaging;
using ThreatsManager.Utilities;

namespace TmtImport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (Directory.Exists(args[0]))
                {
                    Console.WriteLine("Threats Manager Platform - MS TMT Files Converter");

                    var loader = new ModelLoader();

                    Analyze(loader, args[0]);
                }
                else
                {
                    throw new DirectoryNotFoundException();
                }
            }
        }

        static void Analyze(ModelLoader loader, string directory)
        {
            Console.WriteLine();
            Console.WriteLine($"- Analyzing folder {directory}");

            var docs = Directory.GetFiles(directory, "*.tm7")?.ToArray();
            if (docs?.Any() ?? false)
            {
                foreach (var doc in docs)
                    ConvertModel(loader, doc);
            }

            var templates = Directory.GetFiles(directory, "*.tb7")?.ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var template in templates)
                {
                    GenerateTemplate(template, ConvertModel(loader, template));
                }
            }

            var folders = Directory.GetDirectories(directory)?.ToArray();
            if (folders?.Any() ?? false)
            {
                foreach (var folder in folders)
                    Analyze(loader, folder);
            }
        }

        static IThreatModel ConvertModel(ModelLoader loader, string fileName)
        {
            Console.Write($"--- Converting file {Path.GetFileName(fileName)}");

            var model = loader.ConvertModel(fileName);

            if (model != null)
            {
                var tmSerialized = ThreatModelManager.Serialize(model);
                var dest = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}.tm");
                var package = Package.Create(dest);
                package.Add("threatmodel.json", tmSerialized);
                package.Save();
            }

            Console.WriteLine(" - done.");

            return model;
        }

        static void GenerateTemplate(string pathName, IThreatModel model)
        {
            var dest = Path.Combine(Path.GetDirectoryName(pathName), $"{Path.GetFileNameWithoutExtension(pathName)}.tmt");
            model?.SaveTemplate(new DuplicationDefinition()
            {
                AllEntityTemplates = true,
                AllFlowTemplates = true,
                AllMitigations = true,
                AllProperties = true,
                AllPropertySchemas = true,
                AllSeverities = true,
                AllStrengths = true,
                AllThreatTypes = true,
                AllTrustBoundaryTemplates = true
            }, model.Name ?? Path.GetFileNameWithoutExtension(pathName), model.Description, dest);

            Console.WriteLine($"--- Generated template {Path.GetFileName(dest)}");
        }
    }
}
