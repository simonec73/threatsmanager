using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Reporting;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("058E30D0-8193-43FF-A3A0-E233FDFCABB2", "Model Description Placeholder", 11, ExecutionMode.Business)]
    public class ModelDescriptionPlaceholder : ITextPlaceholder
    {
        public string Suffix => "ModelDescription";

        public string GetText(IThreatModel model)
        {
            return model?.Description ?? string.Empty;
        }
    }
}