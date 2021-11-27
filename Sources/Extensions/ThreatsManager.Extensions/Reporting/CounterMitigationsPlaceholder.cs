using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("C1426A54-7841-4493-A29B-73332E7965A0", "Mitigations Counter Placeholder", 18, ExecutionMode.Business)]
    public class CounterMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterMitigationsPlaceholder();
        }
    }

    public class CounterMitigationsPlaceholder : ICounterPlaceholder
    {
        public string Name => "Mitigations";
        public string Label => "Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.UniqueMitigations;
        }
    }
}
