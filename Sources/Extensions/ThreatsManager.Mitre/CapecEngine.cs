using System.Linq;
using ThreatsManager.Mitre.Capec;
using PostSharp.Patterns.Contracts;
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
            graph.RegisterSource(_catalog.Name, _catalog.Version, _catalog.Date);

            var views = _catalog.Views?
                .Where(x => x.Type == ViewTypeEnumeration.Graph && x.Status != StatusEnumeration.Deprecated && x.Status != StatusEnumeration.Obsolete)
                .ToArray();
            if (views?.Any() ?? false)
            {
                foreach (var v in views)
                    graph.CreateNode(v);
            }

            var categories = _catalog.Categories?
                .Where(x => x.Status != StatusEnumeration.Deprecated && x.Status != StatusEnumeration.Obsolete)
                .ToArray();
            if (categories?.Any() ?? false)
            {
                foreach (var c in categories)
                    graph.CreateNode(c);
            }

            var attackPatterns = _catalog.Attack_Patterns?
                .Where(x => x.Status != StatusEnumeration.Deprecated && x.Status != StatusEnumeration.Obsolete)
                .ToArray();
            if (attackPatterns?.Any() ?? false)
            {
                foreach (var a in attackPatterns)
                    graph.CreateNode(a);
            }
        }
    }
}