using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace SampleWinFormExtensions.Panels.AzureDevOps
{
    [Extension("9A97643C-278A-4F39-BCCE-6D447203C12A",
        "Simulated Azure DevOps Integration", 1000, ExecutionMode.Management)]
    public partial class AzureDevOpsPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new AzureDevOpsPanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            return new AzureDevOpsPanel();
        }
        #endregion
    }
}