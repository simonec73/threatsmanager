using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Troubleshooting
{
    [Extension("C12DAFC1-763F-46E6-8222-46393B6DE3E8", "Troubleshooting Panel", 35, ExecutionMode.Business)]
    public partial class TroubleshootingPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Help Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new TroubleshootingPanel();
            action = new ActionDefinition(result.Id, "Troubleshooting", "Troubleshooting", Properties.Resources.lifebelt_big,
                Properties.Resources.lifebelt);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new TroubleshootingPanel();
            return result;
        }
        #endregion
    }
}