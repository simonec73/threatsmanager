using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.QuantitativeRisk.Initializers
{
    [Extension("49798631-1E7A-4C13-824B-969839A189E4", "Quantitative Risk Post Load Processor", 10, ExecutionMode.Pioneer)]
    public class QuantitativeRiskPostLoadProcessor : BaseInitializer, IPostLoadProcessor
    {
        public void Process(IThreatModel model)
        {
            DoInitialize(model);
        }
    }
}