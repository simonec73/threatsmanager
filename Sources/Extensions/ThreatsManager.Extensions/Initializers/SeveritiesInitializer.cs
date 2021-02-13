using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("389FA3FA-C263-4B4C-8B8F-0266E553D261", "Standard Severities Initializer", 10, ExecutionMode.Simplified)]
    public class SeveritiesInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            model.InitializeStandardSeverities();
        }
    }
}