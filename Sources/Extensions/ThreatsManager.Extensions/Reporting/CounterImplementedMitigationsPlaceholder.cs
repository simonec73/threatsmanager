using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F514378E-CFEF-45CE-A158-198C821EFF1B", "Implemented Mitigations Counter Placeholder", 26, ExecutionMode.Business)]
    public class CounterImplementedMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterImplementedMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterImplementedMitigationsPlaceholder();
        }
    }

    public class CounterImplementedMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "ImplementedMitigations";
        public string Label => "Implemented Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Implemented);
        }
    }
}
