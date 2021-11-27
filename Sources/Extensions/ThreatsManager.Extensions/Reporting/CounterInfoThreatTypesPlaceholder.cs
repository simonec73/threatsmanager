using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("B0AC44D5-3659-45DD-9562-19874AB45D31", "Info Severity Threat Types Counter Placeholder", 23, ExecutionMode.Business)]
    public class CounterInfoThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterInfoThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterInfoThreatTypesPlaceholder();
        }
    }

    public class CounterInfoThreatTypesPlaceholder : ICounterPlaceholder
    {
        public string Name => "InfoSeverityThreatTypes";
        public string Label => "Info Severity Threat Types";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            return model.CountThreatEventsByType((int)DefaultSeverity.Info);
        }
    }
}
