using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("42B06427-97A1-4C72-A8FC-253E7BF8EC0B", "Bug Bar Residual Risk Estimator High Threats Placeholder", 101, ExecutionMode.Business)]
    public class CounterBugBarHighPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterBugBarHigh";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterBugBarHighPlaceholder();
        }
    }

    public class CounterBugBarHighPlaceholder : ICounterPlaceholder
    {
        public string Name => "BugBarHigh";
        public string Label => "Bug Bar Risk Estimator High Threats parameter";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var configured = schemaManager?.Parameters?.ToArray();

            var value = configured?
               .FirstOrDefault(x => x.Name.Contains("High"))?
               .Value ?? 0.0f;
            if (value < 0.0f)
                value = 100.0f;

            return (int)value;
        }
    }
}
