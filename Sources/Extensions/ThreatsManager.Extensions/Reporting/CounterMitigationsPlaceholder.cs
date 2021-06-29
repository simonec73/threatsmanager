using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("C1426A54-7841-4493-A29B-73332E7965A0", "Mitigations Counter Placeholder", 18, ExecutionMode.Business)]
    public class CounterMitigationsPlaceholder : ICounter
    {
        public string Qualifier => "CounterMitigations";

        public int GetCounter(IThreatModel model)
        {
            return model.UniqueMitigations;
        }
    }
}
