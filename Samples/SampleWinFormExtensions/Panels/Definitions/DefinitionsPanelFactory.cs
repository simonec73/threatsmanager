using System.ComponentModel.Composition;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace SampleWinFormExtensions.Panels.Definitions
{
    [Extension("2ED5A24F-1666-4171-B4AB-07FF00AAD2D6",
        "Definitions Editor Panel", 1000, ExecutionMode.Business)]
    public partial class DefinitionsPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new DefinitionsPanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            return new DefinitionsPanel();
        }
        #endregion
    }
}