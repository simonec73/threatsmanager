using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("1B861078-A088-4368-8A19-B96585BF7158", "Known Types Initializer", 10, ExecutionMode.Business)]
    public class KnownTypesInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(ResidualRiskEstimatorConfiguration));
            KnownTypesBinder.AddKnownType(typeof(ResidualRiskEstimatorParameter));
        }
    }
}