using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace SampleWinFormExtensions.Panels.Validate
{
    [Extension("A53B0FCF-A888-4737-8BE0-5F38CB769A51",
        "Threat Model Validation", 1000, ExecutionMode.Management)]
    public partial class ValidatePanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ValidatePanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            return new ValidatePanel();
        }
        #endregion
    }
}