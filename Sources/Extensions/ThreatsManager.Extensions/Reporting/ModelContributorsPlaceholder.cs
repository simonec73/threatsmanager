using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("BA14B407-C2D5-4B38-BAEB-28F4A7D7BF57", "Model Contributors Placeholder", 13, ExecutionMode.Business)]
    public class ModelContributorsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelContributors";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelContributorsPlaceholder();
        }
    }

    public class ModelContributorsPlaceholder : ITextEnumerationPlaceholder
    {
        public string Name => "Contributors";
        public string Label => "Contributors";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.Contributors;
        }
    }
}