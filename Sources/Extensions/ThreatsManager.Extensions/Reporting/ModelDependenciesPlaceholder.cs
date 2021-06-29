using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A018E44C-F3F0-4E35-9F02-B0A2CE30B5F7", "Model Dependencies Placeholder", 15, ExecutionMode.Business)]
    public class ModelDependenciesPlaceholder : ITextEnumerationPlaceholder
    {
        public string Qualifier => "ModelDependencies";

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.ExternalDependencies;
        }
    }
}