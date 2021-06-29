using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("9503C7D7-110D-4060-8185-24FAEC5D1602", "Proposed Mitigations Counter Placeholder", 24, ExecutionMode.Business)]
    public class CounterProposedMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterProposedMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Proposed);
        }
    }
}
