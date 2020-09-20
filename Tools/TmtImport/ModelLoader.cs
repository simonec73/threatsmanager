using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ThreatsManager.AutoThreatGeneration.Initializers;
using ThreatsManager.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.MsTmt;
using ThreatsManager.Packaging;
using ThreatsManager.Utilities;

namespace TmtImport
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

            var autoGenRuleInitializer = new AutoGenRuleInitializer();
            autoGenRuleInitializer.Initialize();
        }

        private void OnTypeNotFound(string assemblyName, string typeName)
        {
            if (string.CompareOrdinal(assemblyName, "mscorlib") == 0 || string.CompareOrdinal(assemblyName, "System.Private.CoreLib") == 0)
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

        public IThreatModel ConvertModel(string fileName)
        {
            var importer = new Importer();

            var result = ThreatModelManager.GetDefaultInstance();
            result.InitializeStandardSeverities();
            result.InitializeStandardStrengths();

            importer.Import(result, fileName, 1.0f, null, out var diagrams, out var externalInteractors,
                out var processes, out var dataStores, out var flows, out var trustBoundaries, out var itemTypes,
                out var threatTypes, out var customThreatTypes, out var threats, out var missingThreats);

            return result;
        }
    }
}
