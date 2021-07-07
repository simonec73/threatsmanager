using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("6DB42458-187B-47B0-B231-E8F8ACD854EB", "Model Owner Placeholder", 12, ExecutionMode.Business)]
    public class ModelOwnerPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ModelOwner";

        public IPlaceholder Create(string parameters = null)
        {
            return new ModelOwnerPlaceholder();
        }
    }

    public class ModelOwnerPlaceholder : ITextPlaceholder
    {
        public string Name => "Owner";
        public string Label => "Owner";
        public PlaceholderSection Section => PlaceholderSection.Model;
        public Bitmap Image => null;

        public string GetText(IThreatModel model)
        {
            return model?.Owner ?? string.Empty;
        }
    }
}