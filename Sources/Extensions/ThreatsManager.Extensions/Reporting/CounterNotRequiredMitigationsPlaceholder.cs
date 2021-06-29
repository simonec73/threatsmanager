using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B7CD96CF-0D82-4F6F-9798-AE8085C0260B", "Not Required Mitigations Counter Placeholder", 33, ExecutionMode.Business)]
    public class CounterNotRequiredMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterNotRequiredMitigations";

        public int GetCounter(IThreatModel model)
        {
            var mitigations = model.Mitigations?
                .Select(x => new KeyValuePair<Guid, RoadmapStatus>(x.Id, x.GetStatus()))
                .ToArray();

            return mitigations?.Count(x => x.Value == RoadmapStatus.NoActionRequired) ?? 0;
        }
    }
}
