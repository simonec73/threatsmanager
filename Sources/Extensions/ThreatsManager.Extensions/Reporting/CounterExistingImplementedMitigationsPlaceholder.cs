using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F84BDC3E-507A-46CF-A172-3AD5565FE476", "Existing + Implemented Mitigations Counter Placeholder", 27, ExecutionMode.Business)]
    public class CounterExistingImplementedMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterExistingImplementedMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Implemented) + model.CountMitigationsByStatus(MitigationStatus.Existing);
        }
    }
}
