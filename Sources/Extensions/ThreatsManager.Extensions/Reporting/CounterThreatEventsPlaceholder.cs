using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("090FDDF2-DCF4-4F37-9E57-196966C0593B", "Threat Events Counter Placeholder", 17, ExecutionMode.Business)]
    public class CounterThreatEventsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterThreatEvents";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterThreatEventsPlaceholder();
        }
    }

    public class CounterThreatEventsPlaceholder : ICounterPlaceholder
    {
        public string Name => "ThreatEvents";
        public string Label => "Threat Events";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.TotalThreatEvents;
        }
    }
}
