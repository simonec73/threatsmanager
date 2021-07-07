using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F84BDC3E-507A-46CF-A172-3AD5565FE476", "Existing + Implemented Mitigations Counter Placeholder", 27, ExecutionMode.Business)]
    public class CounterExistingImplementedMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterExistingImplementedMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterExistingImplementedMitigationsPlaceholder();
        }
    }

    public class CounterExistingImplementedMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "Existing+ImplementedMitigations";
        public string Label => "Existing + Implemented Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Implemented) + model.CountMitigationsByStatus(MitigationStatus.Existing);
        }
    }
}
