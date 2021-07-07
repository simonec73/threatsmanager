using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A018E44C-F3F0-4E35-9F02-B0A2CE30B5F7", "Model Dependencies Placeholder", 15, ExecutionMode.Business)]
    public class ModelDependenciesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelDependencies";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelDependenciesPlaceholder();
        }
    }

    public class ModelDependenciesPlaceholder : ITextEnumerationPlaceholder
    {
        public string Name => "Dependencies";
        public string Label => "Dependencies";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.ExternalDependencies;
        }
    }
}