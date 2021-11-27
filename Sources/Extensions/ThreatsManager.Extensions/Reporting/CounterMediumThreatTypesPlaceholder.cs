using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("CC0A4FD2-28D5-45D6-9520-33E54F853AB4", "Medium Severity Threat Types Counter Placeholder", 21, ExecutionMode.Business)]
    public class CounterMediumThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterMediumThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterMediumThreatTypesPlaceholder();
        }
    }

    public class CounterMediumThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "MediumSeverityThreatTypes";
        public string Label => "Medium Severity Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.Medium);
        }
    }
}
