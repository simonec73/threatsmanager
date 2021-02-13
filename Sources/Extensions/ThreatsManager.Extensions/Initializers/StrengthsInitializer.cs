using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("C7EE5AE4-3DD0-4CD1-8CAD-AC3D21BF8F5D", "Standard Strengths Initializer", 10, ExecutionMode.Simplified)]
    public class StrengthInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            model.InitializeStandardStrengths();
        }
    }
}