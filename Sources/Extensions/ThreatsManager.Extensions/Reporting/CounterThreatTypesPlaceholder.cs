using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B65E44E2-9F7C-43BF-81BB-E0ACC773167A", "Threat Types Counter Placeholder", 16, ExecutionMode.Business)]
    public class CounterThreatTypesPlaceholder : ICounter
    {
        public string Qualifier => "CounterThreatTypes";

        public int GetCounter(IThreatModel model)
        {
            return model.AssignedThreatTypes;
        }
    }
}
