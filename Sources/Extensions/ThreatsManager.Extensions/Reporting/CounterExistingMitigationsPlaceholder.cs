using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("CD777AF2-F1D7-4C8F-B6D4-53E3ED8BBC1E", "Existing Mitigations Counter Placeholder", 25, ExecutionMode.Business)]
    public class CounterExistingMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterExistingMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Existing);
        }
    }
}
