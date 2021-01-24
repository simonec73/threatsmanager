using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Panels.MitigationsKanban
{
    [Extension("68D6E6B3-FEE0-4236-AB44-EFCD0C15FBAA", "Kanban for managing Mitigations stored in DevOps", 70, ExecutionMode.Management)]
    public partial class MitigationsKanbanPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new MitigationsKanbanPanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new MitigationsKanbanPanel();
            //if (_actions != null)
            //    result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}