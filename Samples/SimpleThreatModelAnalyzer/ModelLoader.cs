using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThreatsManager.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Packaging;
using ThreatsManager.Utilities;

namespace SimpleThreatModelAnalyzer
{
    class ModelLoader
    {
        private List<string> _missingTypes = new List<string>();

        public ModelLoader()
        {
            KnownTypesBinder.TypeNotFound += OnTypeNotFound;
            ExtensionsConfigurationManager.SetConfigurationUserLevel(ConfigurationUserLevel.None);
            Manager.Instance.LoadExtensions(ExecutionMode.Simplified);
            Manager.Instance.ApplyExtensionInitializers();
        }

        private void OnTypeNotFound(string assemblyName, string typeName)
        {
            if (string.CompareOrdinal(assemblyName, "mscorlib") == 0)
            {
                var regex = new Regex(@"\[\[(?<class>[.\w]*), (?<assembly>[.\w]*)\]\]");
                var match = regex.Match(typeName);
                if (match.Success)
                {
                    assemblyName = match.Groups["assembly"].Value;
                    typeName = match.Groups["class"].Value;
                }
            }
            var name = $"{assemblyName}#{typeName}";

            if (!_missingTypes.Contains(name))
            {
                _missingTypes.Add(name);
                var parts = typeName.Split('.');
                Console.WriteLine($"Document uses type {parts.Last()} from {assemblyName}, which is unknown.\nThe document will be loaded but some information may be missing.");
            }
        }

        public IThreatModel OpenModel(string fileName)
        {
            IThreatModel result = null;

            var package = new Package(fileName);

            var threatModelContent = package.Read("threatmodel.json");
            if (threatModelContent != null)
            {
                try
                {
                    result = ThreatModelManager.Deserialize(threatModelContent, true);
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine($"Some required extension was missing.\nMore details: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception {e.ToString()}");
                }
            }

            return result;
        }
    }
}
