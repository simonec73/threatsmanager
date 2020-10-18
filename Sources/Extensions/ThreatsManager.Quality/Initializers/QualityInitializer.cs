using System.ComponentModel.Composition;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Export(typeof(IExtensionInitializer))]
    [ExportMetadata("Id", "1069F3AA-BD5E-41C6-B681-0D37FCA12937")]
    [ExportMetadata("Label", "Quality Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class QualityInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(FalsePositiveList));
            KnownTypesBinder.AddKnownType(typeof(FalsePositiveInfo));
        }
    }
}