using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("6DB42458-187B-47B0-B231-E8F8ACD854EB", "Model Owner Placeholder", 12, ExecutionMode.Business)]
    public class ModelOwnerPlaceholder : ITextPlaceholder
    {
        public string Qualifier => "ModelOwner";

        public string GetText(IThreatModel model)
        {
            return model?.Owner ?? string.Empty;
        }
    }
}