using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A08C4A7B-BD0D-4D46-B6A4-23C45CFBC36E", "Model Name Placeholder", 10, ExecutionMode.Business)]
    public class ModelNamePlaceholder : ITextPlaceholder
    {
        public string Qualifier => "ModelName";

        public string GetText(IThreatModel model)
        {
            return model?.Name ?? string.Empty;
        }
    }
}
