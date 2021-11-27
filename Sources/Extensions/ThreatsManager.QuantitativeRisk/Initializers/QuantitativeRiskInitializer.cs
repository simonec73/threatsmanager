using ThreatsManager.Interfaces.Extensions;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.QuantitativeRisk.Initializers
{
    [Extension("1F84A8C7-6EBE-46D1-BE84-8B1D1FC33087", "Quantitative Risk Initializer", 10, ExecutionMode.Pioneer)]
    public class QuantitativeRiskInitializer : BaseInitializer, IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            DoInitialize(model);
        }
    }
}