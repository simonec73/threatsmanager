using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.ThreatSources.Capec;

#if CWE
using ThreatModeling.ThreatsManager.ThreatSources.Cwe;
#endif

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public class ThreatSource
    {
        private readonly Dictionary<string, string> _views = new Dictionary<string, string>();
        private readonly Dictionary<string, ThreatSourceNode> _nodes = new Dictionary<string, ThreatSourceNode>();
        private readonly Dictionary<string, Dictionary<string, List<string>>> _parentChild =
            new Dictionary<string, Dictionary<string, List<string>>>();

        public ThreatSource([NotNull] Attack_Pattern_Catalog catalog)
        {
            CatalogName = catalog.Catalog_Name;
            CatalogVersion = catalog.Catalog_Version;
            CatalogDate = catalog.Catalog_Date;

            LoadNodesList(catalog);
        }

#if CWE
        public ThreatSource([NotNull] Weakness_Catalog catalog)
        {
            CatalogName = catalog.Catalog_Name;
            CatalogVersion = catalog.Catalog_Version;
            CatalogDate = catalog.Catalog_Date;

            LoadNodesList(catalog);
        }
#endif

        public string CatalogName { get; private set; }
        public string CatalogVersion { get; private set; }
        public DateTime CatalogDate { get; private set; }
        public IEnumerable<string> Views => _views.Keys.Where(x => _views.ContainsKey(x) && _parentChild.ContainsKey(_views[x]));

        public IEnumerable<ThreatSourceNodeParentChild> GetNodes([Required] string viewName)
        {
            IEnumerable<ThreatSourceNodeParentChild> result = null;

            if (_views.ContainsKey(viewName))
            {
                var parents = _nodes.Where(x => x.Value.IsRoot).Select(x => x.Key);
                var viewId = _views[viewName];
                if (_parentChild.ContainsKey(viewId))
                {
                    var parentChild = _parentChild[viewId];
                    var nodes = parentChild.Where(x => parents.Contains(x.Key)).Select(x => x.Key);
                    result =
                        nodes.Where(x => _nodes.ContainsKey(x))
                            .Select(x => new ThreatSourceNodeParentChild(this, viewId, _nodes[x]));
                }
            }

            return result;
        }

        internal void AddParentChild([Required] string viewId, [Required] string parent, [Required] string child)
        {
            Dictionary<string, List<string>> parentChild;

            if (_parentChild.ContainsKey(viewId))
            {
                parentChild = _parentChild[viewId];
            }
            else
            {
                parentChild = new Dictionary<string, List<string>>();
                _parentChild.Add(viewId, parentChild);
            }

            List<string> children;
            if (parentChild.ContainsKey(parent))
            {
                children = parentChild[parent];
            }
            else
            {
                children = new List<string>();
                parentChild.Add(parent, children);
            }

            if (!children.Contains(child))
                children.Add(child);
        }

        internal IEnumerable<ThreatSourceNode> GetChildren([Required] string viewId, [NotNull] ThreatSourceNode node)
        {
            IEnumerable<ThreatSourceNode> result = null;

            if (_parentChild.ContainsKey(viewId))
            {
                var parentChild = _parentChild[viewId];
                if (parentChild != null && parentChild.ContainsKey(node.Id))
                {
                    result = parentChild[node.Id].Select(x => _nodes[x]);
                }
            }

            return result;
        }

        private void LoadNodesList([NotNull] Attack_Pattern_Catalog catalog)
        {
            foreach (var view in catalog.Views)
            {
                _views.Add(view.Name, view.ID);
            }

            foreach (var category in catalog.Categories)
            {         
                _nodes.Add(category.ID, new ThreatSourceNode(this, category));
            }

            foreach (var attack in catalog.Attack_Patterns)
            {
                try
                {
                    _nodes.Add(attack.ID, new ThreatSourceNode(this, attack));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

#if CWE
        private void LoadNodesList([NotNull] Weakness_Catalog catalog)
        {
            foreach (var view in catalog.Views)
            {
                _views.Add(view.Name, view.ID);
            }

            foreach (var category in catalog.Categories)
            {
                _nodes.Add(category.ID, new ThreatSourceNode(this, category));
            }

            //foreach (var attack in catalog.Attack_Patterns)
            //{
            //    try
            //    {
            //        _nodes.Add(attack.ID, new ThreatSourceNode(this, attack));
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //        throw;
            //    }
            //}
        }
#endif
    }
}