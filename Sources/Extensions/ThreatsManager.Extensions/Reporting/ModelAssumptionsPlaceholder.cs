using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("2826057D-B1EB-43D5-B076-76E1014A9AF0", "Model Assumptions Placeholder", 14, ExecutionMode.Business)]
    public class ModelAssumptionsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelAssumptions";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelAssumptionsPlaceholder();
        }
    }

    public class ModelAssumptionsPlaceholder : ITextEnumerationPlaceholder
    {
        public string Name => "Assumptions";
        public string Label => "Assumptions";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.Assumptions;
        }
    }
}