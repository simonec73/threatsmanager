using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F514378E-CFEF-45CE-A158-198C821EFF1B", "Implemented Mitigations Counter Placeholder", 26, ExecutionMode.Business)]
    public class CounterImplementedMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterImplementedMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Implemented);
        }
    }
}
