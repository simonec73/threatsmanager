using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace SampleWinFormExtensions.Panels.AzureDevOps
{
    [Extension(typeof(IPanelFactory), "9A97643C-278A-4F39-BCCE-6D447203C12A",
        "Simulated Azure DevOps Integration", 1000, ExecutionMode.Management)]
    public partial class AzureDevOpsPanelFactory : IPanelFactory, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new AzureDevOpsPanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel Create(IActionDefinition action)
        {
            return new AzureDevOpsPanel();
        }
        #endregion
    }
}