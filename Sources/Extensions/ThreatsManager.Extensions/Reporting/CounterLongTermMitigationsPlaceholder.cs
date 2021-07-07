using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("DF8FC9A6-C193-4A92-87FC-A3C3C51D713A", "Long Term Mitigations Counter Placeholder", 32, ExecutionMode.Business)]
    public class CounterLongTermMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterLongTermMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterLongTermMitigationsPlaceholder();
        }
    }

    public class CounterLongTermMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "LongTermMitigations";
        public string Label => "Long Term Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var mitigations = model.Mitigations?
                .Select(x => new KeyValuePair<Guid, RoadmapStatus>(x.Id, x.GetStatus()))
                .ToArray();

            return mitigations?.Count(x => x.Value == RoadmapStatus.LongTerm) ?? 0;
        }
    }
}
