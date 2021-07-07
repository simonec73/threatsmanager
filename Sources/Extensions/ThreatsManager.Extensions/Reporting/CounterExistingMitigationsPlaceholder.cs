using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("CD777AF2-F1D7-4C8F-B6D4-53E3ED8BBC1E", "Existing Mitigations Counter Placeholder", 25, ExecutionMode.Business)]
    public class CounterExistingMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterExistingMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterExistingMitigationsPlaceholder();
        }
    }

    public class CounterExistingMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "ExistingMitigations";
        public string Label => "Existing Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Existing);
        }
    }
}
