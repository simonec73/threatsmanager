using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("090FDDF2-DCF4-4F37-9E57-196966C0593B", "Threat Events Counter Placeholder", 17, ExecutionMode.Business)]
    public class CounterThreatEventsPlaceholder : ICounter
    {
        public string Qualifier => "CounterThreatEvents";

        public int GetCounter(IThreatModel model)
        {
            return model.TotalThreatEvents;
        }
    }
}
