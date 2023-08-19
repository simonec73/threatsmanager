using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("3551913E-C074-4E29-AABF-F13BBC341151", "Bug Bar Residual Risk Estimator Low Threats Placeholder", 103, ExecutionMode.Business)]
    public class CounterBugBarLowPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterBugBarLow";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterBugBarLowPlaceholder();
        }
    }

    public class CounterBugBarLowPlaceholder : ICounterPlaceholder
    {
        public string Name => "BugBarLow";
        public string Label => "Bug Bar Risk Estimator Low Threats parameter";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var configured = schemaManager?.Parameters?.ToArray();

            var value = configured?
               .FirstOrDefault(x => x.Name.Contains("Low"))?
               .Value ?? 0.0f;
            if (value < 0.0f)
                value = 100.0f;

            return (int)value;
        }
    }
}
