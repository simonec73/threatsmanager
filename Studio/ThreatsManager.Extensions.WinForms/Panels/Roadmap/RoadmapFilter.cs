using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public class RoadmapFilter
    {
        private readonly List<IRoadmapFilter> _filters = new List<IRoadmapFilter>();
        private readonly List<string> _enabled = new List<string>();

        public RoadmapFilter([NotNull] IThreatModel model)
        {
            var providers = ExtensionUtils.GetExtensions<IRoadmapFilterProvider>()?.ToArray();

            if (providers?.Any() ?? false)
            {
                foreach (var provider in providers)
                {
                    var filters = provider.GetFilters(model)?.ToArray();
                    if (filters?.Any() ?? false)
                    {
                        _filters.AddRange(filters);
                    }
                }
            }
        }

        /// <summary>
        /// Filters are applied with AND or OR.
        /// </summary>
        public bool Or { get; set; }

        public IEnumerable<string> Filters => _filters.Select(x => x.Name);

        public IEnumerable<string> Enabled => _enabled.AsReadOnly();

        public void Enable(string name)
        {
            if (_filters.Any(x => string.CompareOrdinal(x.Name, name) == 0) &&
                !_enabled.Contains(name))
            {
                _enabled.Add(name);
            }
        }

        public void Disable(string name)
        {
            if (_filters.Any(x => string.CompareOrdinal(x.Name, name) == 0) &&
                _enabled.Contains(name))
            {
                _enabled.Remove(name);
            }
        }

        public IEnumerable<IMitigation> Filter(IEnumerable<IMitigation> mitigations)
        {
            IEnumerable<IMitigation> result = null;

            var filters = _filters.Where(x => _enabled.Contains(x.Name)).ToArray();
            if (filters.Any())
            {
                if (Or)
                {
                    var list = new List<IMitigation>();

                    foreach (var filter in filters)
                    {
                        var filterResult = filter.Filter(mitigations)?.ToArray();
                        if (filterResult?.Any() ?? false)
                        {
                            foreach (var item in filterResult)
                            {
                                if (!list.Contains(item))
                                    list.Add(item);
                            }
                        }
                    }

                    if (list.Any())
                        result = list.AsReadOnly();
                }
                else
                {
                    result = mitigations;

                    foreach (var filter in filters)
                    {
                        result = filter.Filter(result);
                    }
                }
            }
            else
            {
                result = mitigations;
            }

            return result;
        }
    }
}
