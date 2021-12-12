using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.RoadmapProviders
{
    [Extension("3A5FBD28-EE02-4E60-AB45-4782D4E8FBA2", "DevOps Info Roadmap Property Provider", 35, ExecutionMode.Management)]
    public class DevOpsInfoRoadmapProvider : IRoadmapPropertyProvider
    {
        public string Name => "DevOps Status";

        public string GetValue([NotNull] IMitigation mitigation)
        {
            string result = null;

            // ReSharper disable once PatternAlwaysOfType
            if (mitigation.Model is IThreatModel model)
            {
                var connector = DevOpsManager.GetConnector(model);
                if (connector != null)
                {
                    var schemaManager = new DevOpsPropertySchemaManager(model);
                    var status = schemaManager.GetDevOpsInfo(mitigation, connector);
                    result = (status?.Status ?? WorkItemStatus.Unknown).GetEnumLabel();
                }
            }

            return result;
        }
    }
}
