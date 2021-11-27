using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("058E30D0-8193-43FF-A3A0-E233FDFCABB2", "Model Description Placeholder", 11, ExecutionMode.Business)]
    public class ModelDescriptionPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelDescription";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelDescriptionPlaceholder();
        }
    }

    public class ModelDescriptionPlaceholder : ITextPlaceholder
    {
        public string Name => "Description";
        public string Label => "Description";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public string GetText(IThreatModel model)
        {
            return model?.Description ?? string.Empty;
        }
    }
}