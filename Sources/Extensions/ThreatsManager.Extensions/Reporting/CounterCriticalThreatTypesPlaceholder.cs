using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A96E1A4A-F2EB-4014-9601-2C78543C1A0F", "Critical Threat Types Counter Placeholder", 19, ExecutionMode.Business)]
    public class CounterCriticalThreatTypesPlaceholder : ICounter
    {
        public string Qualifier => "CounterCriticalThreatTypes";

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int) DefaultSeverity.Critical);
        }
    }
}
