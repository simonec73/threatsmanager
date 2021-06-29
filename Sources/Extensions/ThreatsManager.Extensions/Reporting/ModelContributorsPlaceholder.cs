using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("BA14B407-C2D5-4B38-BAEB-28F4A7D7BF57", "Model Contributors Placeholder", 13, ExecutionMode.Business)]
    public class ModelContributorsPlaceholder : ITextEnumerationPlaceholder
    {
        public string Qualifier => "ModelContributors";

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.Contributors;
        }
    }
}