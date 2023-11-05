using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("9A66A7EE-9DB5-4CB4-B143-7056C3DB9BE7", "Bug Bar Residual Risk Estimator Info Threats Placeholder", 104, ExecutionMode.Business)]
    public class CounterBugBarInfoPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "CounterBugBarInfo";

        public IPlaceholder Create(string parameters = null)
        {
            return new CounterBugBarInfoPlaceholder();
        }
    }

    public class CounterBugBarInfoPlaceholder : ICounterPlaceholder
    {
        public string Name => "BugBarInfo";
        public string Label => "Bug Bar Risk Estimator Info Threats parameter";
        public PlaceholderSection Section => PlaceholderSection.Counter;
        public Bitmap Image => null;

        public int GetCounter(IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var configured = schemaManager?.Parameters?.ToArray();

            var value = configured?
               .FirstOrDefault(x => x.Name.Contains("Info"))?
               .Value ?? 0.0f;
            if (value < 0.0f)
                value = 100.0f;

            return (int)value;
        }
    }
}
