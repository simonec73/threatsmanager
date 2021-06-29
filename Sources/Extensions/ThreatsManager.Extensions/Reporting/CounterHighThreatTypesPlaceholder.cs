using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B4CB10AF-C820-4FCD-A05D-9EE5DDB3BB3D", "High Severity Threat Types Counter Placeholder", 20, ExecutionMode.Business)]
    public class CounterHighThreatTypesPlaceholder : ICounter
    {
        public string Qualifier => "CounterHighThreatTypes";

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.High);
        }
    }
}
