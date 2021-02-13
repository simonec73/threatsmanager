using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.RoadmapProviders
{
    [Extension("D4BB488D-0622-470F-9D9E-6E7E65273E6F", "Iterations Roadmap Filter Provider", 
        50, ExecutionMode.Business)]
    public class IterationsFilterProvider : IRoadmapFilterProvider
    {
        public IEnumerable<IRoadmapFilter> GetFilters([NotNull] IThreatModel model)
        { 
            var schemaManager = new DevOpsConfigPropertySchemaManager(model);

            return schemaManager.GetIterations()?.Select(x => new IterationFilter(x));
        }
    }
}
