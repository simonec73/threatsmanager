using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("CC0A4FD2-28D5-45D6-9520-33E54F853AB4", "Medium Severity Threat Types Counter Placeholder", 21, ExecutionMode.Business)]
    public class CounterMediumThreatTypesPlaceholder : ICounter
    {
        public string Qualifier => "CounterMediumThreatTypes";

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.Medium);
        }
    }
}
