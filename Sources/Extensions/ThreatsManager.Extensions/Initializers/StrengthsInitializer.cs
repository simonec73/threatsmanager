using System.ComponentModel.Composition;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "C7EE5AE4-3DD0-4CD1-8CAD-AC3D21BF8F5D")]
    [ExportMetadata("Label", "Standard Strengths Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class StrengthInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            model.InitializeStandardStrengths();
        }
    }
}