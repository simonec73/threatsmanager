using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A08C4A7B-BD0D-4D46-B6A4-23C45CFBC36E", "Model Name Placeholder", 10, ExecutionMode.Business)]
    public class ModelNamePlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelName";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelNamePlaceholder();
        }
    }

    public class ModelNamePlaceholder : ITextPlaceholder
    {
        public string Name => "Name";
        public string Label => "Name";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public string GetText(IThreatModel model)
        {
            return model?.Name ?? string.Empty;
        }
    }
}
