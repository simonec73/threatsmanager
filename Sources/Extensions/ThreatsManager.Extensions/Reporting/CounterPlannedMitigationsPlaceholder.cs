using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("3A4707AF-12F4-4B40-9159-A0F9B01117A1", "Planned Mitigations Counter Placeholder", 29, ExecutionMode.Business)]
    public class CounterPlannedMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterPlannedMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterPlannedMitigationsPlaceholder();
        }
    }

    public class CounterPlannedMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "PlannedMitigations";
        public string Label => "Planned Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountMitigationsByStatus(MitigationStatus.Planned);
        }
    }
}
