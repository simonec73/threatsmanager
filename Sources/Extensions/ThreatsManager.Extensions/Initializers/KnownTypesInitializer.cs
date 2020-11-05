using System.ComponentModel.Composition;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IExtensionInitializer))]
    [ExportMetadata("Id", "1B861078-A088-4368-8A19-B96585BF7158")]
    [ExportMetadata("Label", "Known Types Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Business)]
    public class KnownTypesInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(ResidualRiskEstimatorConfiguration));
            KnownTypesBinder.AddKnownType(typeof(ResidualRiskEstimatorParameter));
        }
    }
}