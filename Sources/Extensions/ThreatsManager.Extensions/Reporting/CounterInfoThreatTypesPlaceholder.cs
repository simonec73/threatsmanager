using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B0AC44D5-3659-45DD-9562-19874AB45D31", "Info Severity Threat Types Counter Placeholder", 23, ExecutionMode.Business)]
    public class CounterInfoThreatTypesPlaceholder : ICounter
    {
        public string Qualifier => "CounterInfoThreatTypes";

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.Info);
        }
    }
}
