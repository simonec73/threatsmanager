using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("0FE42486-93D7-4155-9228-482D9C984FA7", "Mid Term Mitigations Counter Placeholder", 31, ExecutionMode.Business)]
    public class CounterMidTermMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterMidTermMitigations";

        public int GetCounter(IThreatModel model)
        {
            var mitigations = model.Mitigations?
                .Select(x => new KeyValuePair<Guid, RoadmapStatus>(x.Id, x.GetStatus()))
                .ToArray();

            return mitigations?.Count(x => x.Value == RoadmapStatus.MidTerm) ?? 0;
        }
    }
}
