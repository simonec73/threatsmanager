using System;
using System.IO;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Mitre.Attack;
using ThreatsManager.Mitre.Graph;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Mitre
{
    public class AttackEngine : IInitializableObject
    {
        private readonly Bundle _catalog;
        private readonly string _name;
        private readonly string _version;

        public AttackEngine([Required] string name, [Required] string version, [Required] string json)
        {
            _name = name;
            _version = version;

            if (json.Length > 0)
            {
                using (var textReader = new StringReader(json))
                using (var reader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer();
                    _catalog = serializer.Deserialize<Bundle>(reader);
                }
            }
        }

        public bool IsInitialized => _catalog != null;

        [InitializationRequired]
        public void EnrichGraph([NotNull] MitreGraph graph)
        {
            graph.RegisterSource(_name, _version, DateTime.MinValue);

        }
    }
}
