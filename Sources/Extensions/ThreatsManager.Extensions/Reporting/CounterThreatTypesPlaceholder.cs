using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B65E44E2-9F7C-43BF-81BB-E0ACC773167A", "Threat Types Counter Placeholder", 16, ExecutionMode.Business)]
    public class CounterThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterThreatTypesPlaceholder();
        }
    }

    public class CounterThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "ThreatTypes";
        public string Label => "Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.AssignedThreatTypes;
        }
    }
}
