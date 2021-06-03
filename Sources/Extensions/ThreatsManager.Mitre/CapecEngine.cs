using System;
using System.Linq;
using ThreatsManager.Mitre.Capec;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection.MethodBody;
using ThreatsManager.Interfaces;
using ThreatsManager.Mitre.Graph;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Mitre
{
    public class CapecEngine : IInitializableObject
    {
        private readonly Attack_Pattern_Catalog _catalog;

        public CapecEngine([Required] string xml)
        {
            _catalog = Attack_Pattern_Catalog.Deserialize(xml);
        }

        public bool IsInitialized => _catalog != null;

        [InitializationRequired]
        public string Version => _catalog.Version;

        [InitializationRequired]
        public void EnrichGraph([NotNull] MitreGraph graph)
        {
            
        }
    }
}