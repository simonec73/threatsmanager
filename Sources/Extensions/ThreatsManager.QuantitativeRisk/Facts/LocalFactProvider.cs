using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [Interfaces.Extension("A89D9762-C1F8-41BA-B6C5-44A52E511303", "Local Fact Provider", 10, ExecutionMode.Pioneer)]
    public class LocalFactProvider : IFactProvider
    {
        private const string cLocalFactsFileName = "localfacts.json";
        private FactContainer _container;

        public bool RegisterFact(Fact fact)
        {
            var result = false;

            try
            {
                if (_container == null)
                {
                    _container = LoadFacts();
                    if (_container == null)
                        _container = new FactContainer();
                }

                _container.Add(fact);
                SaveFacts(_container);
                result = true;
            }
            catch
            {
            }

            return result;
        }

        public bool RemoveFact(Fact fact)
        {
            return RemoveFact(fact.Id);
        }

        public bool RemoveFact(Guid factId)
        {
            var result = false;

            try
            {
                if (_container == null)
                    _container = LoadFacts();

                if (_container != null)
                {
                    result = _container.Remove(factId);
                    if (result)
                    {
                        SaveFacts(_container);
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        public IEnumerable<string> Contexts { get; }

        public IEnumerable<Fact> GetFacts(string context, string filter = null)
        {
            if (_container == null)
                _container = LoadFacts();

            return _container?.Facts?
                .Where(x => string.Compare(context, x.Context, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                            (string.IsNullOrWhiteSpace(filter) ||
                             (x.Text?.ToLower().Contains(filter.ToLower()) ?? false) ||
                             (x.Source?.ToLower().Contains(filter.ToLower()) ?? false)));
        }

        private static FactContainer LoadFacts()
        {
            FactContainer result = null;

            var fileName = Path.Combine(ExtensionUtils.ExtensionConfigurationFolder, cLocalFactsFileName);
            var json = File.ReadAllBytes(fileName);
            string jsonText = null;

            if (json[0] == 0xFF)
                jsonText = Encoding.Unicode.GetString(json, 2, json.Length - 2);
            else
                jsonText = Encoding.Unicode.GetString(json);

            using (var textReader = new StringReader(jsonText))
            using (var reader = new JsonTextReader(textReader))
            {
                var serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.None
                };
                result = serializer.Deserialize<FactContainer>(reader);
            }

            return result;
        }

        private static void SaveFacts([NotNull] FactContainer container)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.None, Formatting = Formatting.Indented};
                serializer.Serialize(writer, container);
            }

            var buf = Encoding.Unicode.GetBytes(sb.ToString());

            var result = new byte[buf.Length + 2];
            result[0] = 0xFF;
            result[1] = 0xFE;
            buf.CopyTo(result, 2);

            var fileName = Path.Combine(ExtensionUtils.ExtensionConfigurationFolder, cLocalFactsFileName);
            File.WriteAllBytes(fileName, result);
        }
    }
}
