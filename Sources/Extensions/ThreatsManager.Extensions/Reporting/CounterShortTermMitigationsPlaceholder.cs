using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("BA53B048-C234-4AC0-A734-6D145D34C88B", "Short Term Mitigations Counter Placeholder", 30, ExecutionMode.Business)]
    public class CounterShortTermMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterShortTermMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterShortTermMitigationsPlaceholder();
        }
    }

    public class CounterShortTermMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "ShortTermMitigations";
        public string Label => "Short Term Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var mitigations = model.Mitigations?
                .Select(x => new KeyValuePair<Guid, RoadmapStatus>(x.Id, x.GetStatus()))
                .ToArray();

            return mitigations?.Count(x => x.Value == RoadmapStatus.ShortTerm) ?? 0;
        }
    }
}
