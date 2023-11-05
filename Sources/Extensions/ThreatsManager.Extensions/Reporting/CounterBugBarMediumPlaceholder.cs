using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F02B8A31-92FB-4832-B1ED-C6C41F41A5F2", "Bug Bar Residual Risk Estimator Medium Threats Placeholder", 102, ExecutionMode.Business)]
    public class CounterBugBarMediumPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterBugBarMedium";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterBugBarMediumPlaceholder();
        }
    }

    public class CounterBugBarMediumPlaceholder : ICounterPlaceholder
    {
        public string Name => "BugBarMedium";
        public string Label => "Bug Bar Risk Estimator Medium Threats parameter";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var configured = schemaManager?.Parameters?.ToArray();

            var value = configured?
               .FirstOrDefault(x => x.Name.Contains("Medium"))?
               .Value ?? 0.0f;
            if (value < 0.0f)
                value = 100.0f;

            return (int)value;
        }
    }
}
