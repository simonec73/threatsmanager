using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps.RoadmapProviders
{
    public class IterationFilter : IRoadmapFilter
    {
        private readonly Iteration _iteration;

        public IterationFilter([NotNull] Iteration iteration)
        {
            _iteration = iteration;
        }

        public string Name => $"Show only Mitigations belonging to {_iteration.Name}";

        public IEnumerable<IMitigation> Filter(IEnumerable<IMitigation> mitigations)
        {
            IEnumerable<IMitigation> result = null;

            var items = mitigations?.ToArray();
            if (items?.Any() ?? false)
            {
                var model = items.First().Model;
                if (model != null)
                {
                    var schemaManager = new DevOpsPropertySchemaManager(model);

                    var list = new List<IMitigation>();
                    foreach (var item in items)
                    {
                        if (string.CompareOrdinal(schemaManager.GetFirstSeenOn(item)?.IterationId, _iteration.Id) == 0)
                        {
                            list.Add(item);
                        }
                    }

                    if (list.Any())
                        result = list.AsReadOnly();
                }
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
