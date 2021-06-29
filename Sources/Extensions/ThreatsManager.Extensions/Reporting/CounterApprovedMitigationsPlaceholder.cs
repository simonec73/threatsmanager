using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("4912B903-4995-4818-98F8-EA8D78565A71", "Approved Mitigations Counter Placeholder", 28, ExecutionMode.Business)]
    public class CounterApprovedMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterApprovedMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Approved);
        }
    }
}
