using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A96E1A4A-F2EB-4014-9601-2C78543C1A0F", "Critical Threat Types Counter Placeholder", 19, ExecutionMode.Business)]
    public class CounterCriticalThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterCriticalThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterCriticalThreatTypesPlaceholder();
        }
    }

    public class CounterCriticalThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "CriticalThreatTypes";
        public string Label => "Critical Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int) DefaultSeverity.Critical);
        }

    }
}
