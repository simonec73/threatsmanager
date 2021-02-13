using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.StrengthList
{
    [Extension("74CE0C61-F371-4976-91AC-313E8498C0C3", "Strength List", 32, ExecutionMode.Expert)]
    public partial class StrengthListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Strength List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new StrengthListPanel();
            action = new ActionDefinition(result.Id, "StrengthList", "Strength\nList", Resources.strength_big,
                Resources.strength);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new StrengthListPanel();
        }
        #endregion
    }
}