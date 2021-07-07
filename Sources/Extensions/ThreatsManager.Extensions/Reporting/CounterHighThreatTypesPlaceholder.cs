using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B4CB10AF-C820-4FCD-A05D-9EE5DDB3BB3D", "High Severity Threat Types Counter Placeholder", 20, ExecutionMode.Business)]
    public class CounterHighThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterHighThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterHighThreatTypesPlaceholder();
        }
    }

    public class CounterHighThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "HighSeverityThreatTypes";
        public string Label => "High Severity Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.High);
        }
    }
}
