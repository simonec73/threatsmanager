using System.ComponentModel.Composition;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "389FA3FA-C263-4B4C-8B8F-0266E553D261")]
    [ExportMetadata("Label", "Standard Severities Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class SeveritiesInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            model.InitializeStandardSeverities();
        }
    }
}