using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("E6A884C5-42C8-4060-B9CB-3015238E21B9", "Bug Bar Residual Risk Estimator Critical Threats Placeholder", 100, ExecutionMode.Business)]
    public class CounterBugBarCriticalPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterBugBarCritical";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterBugBarCriticalPlaceholder();
        }
    }

    public class CounterBugBarCriticalPlaceholder : ICounterPlaceholder
    {
        public string Name => "BugBarCritical";
        public string Label => "Bug Bar Risk Estimator Critical Threats parameter";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var configured = schemaManager?.Parameters?.ToArray();

            var value = configured?
               .FirstOrDefault(x => x.Name.Contains("Critical"))?
               .Value ?? 0.0f;
            if (value < 0.0f)
                value = 100.0f;

            return (int)value;
        }
    }
}
