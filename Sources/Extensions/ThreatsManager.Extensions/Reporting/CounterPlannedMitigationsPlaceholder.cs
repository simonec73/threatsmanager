using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("3A4707AF-12F4-4B40-9159-A0F9B01117A1", "Planned Mitigations Counter Placeholder", 29, ExecutionMode.Business)]
    public class CounterPlannedMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterPlannedMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Planned);
        }
    }
}
