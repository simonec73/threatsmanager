using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Reporting;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("2826057D-B1EB-43D5-B076-76E1014A9AF0", "Model Assumptions Placeholder", 14, ExecutionMode.Business)]
    public class ModelAssumptionsPlaceholder : ITextEnumerationPlaceholder
    {
        public string Suffix => "ModelAssumptions";

        public IEnumerable<string> GetTextEnumeration(IThreatModel model)
        {
            return model.Assumptions;
        }
    }
}