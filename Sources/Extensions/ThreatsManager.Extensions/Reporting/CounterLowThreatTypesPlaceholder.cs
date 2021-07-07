using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("DB465FCB-314D-4830-BE10-046B7F5BAF43", "Low Severity Threat Types Counter Placeholder", 22, ExecutionMode.Business)]
    public class CounterLowThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterLowThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterLowThreatTypesPlaceholder();
        }
    }

    public class CounterLowThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "LowSeverityThreatTypes";
        public string Label => "Low Severity Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.Low);
        }
    }
}
