using ThreatsManager.DevOps.Actions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Initializers
{
    [Extension("620BB4D5-B76A-4F75-9EFB-6F41B43DD71F", "New Instances Initializer", 50, ExecutionMode.Management)]
    public class NewInstanceInitializer : IInitializer
    {
        public void Initialize(IThreatModel model)
        {
            Connect.ChangeDisconnectButtonStatus(null, false);
        }
    }
}
