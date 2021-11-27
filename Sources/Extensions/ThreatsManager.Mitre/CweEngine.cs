using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Mitre.Cwe;
using ThreatsManager.Mitre.Graph;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Mitre
{
    public class CweEngine : IInitializableObject
    {
        private readonly Weakness_Catalog _catalog;

        public CweEngine([Required] string xml)
        {
            _catalog = Weakness_Catalog.Deserialize(xml);
        }

        public bool IsInitialized => _catalog != null;

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

            var weaknesses = _catalog.Weaknesses?
                .Where(x => x.Status != StatusEnumeration.Deprecated && x.Status != StatusEnumeration.Obsolete)
                .ToArray();
            if (weaknesses?.Any() ?? false)
            {
                foreach (var w in weaknesses)
                    graph.CreateNode(w);
            }
        }
    }
}
