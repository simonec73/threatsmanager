using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps.RoadmapProviders
{
    [Extension("4AF3DDCB-DFBD-400E-B05B-90C36E979F1A", "Iteration Roadmap Property Provider", 30, ExecutionMode.Management)]
    public class IterationRoadmapProvider : IRoadmapPropertyProvider
    {
        public string Name => "Iteration";

        public string GetValue([NotNull] IMitigation mitigation)
        {
            string result = null;

            // ReSharper disable once PatternAlwaysOfType
            if (mitigation.Model is IThreatModel model)
            {
                var schemaManager = new DevOpsPropertySchemaManager(model);
                result = schemaManager.GetFirstSeenOn(mitigation)?.GetIteration(model)?.Name;
            }

            return result;
        }
    }
}
