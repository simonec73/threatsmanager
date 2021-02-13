using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.DataFlowList
{
    [Extension("46BBE705-A84C-444D-8872-0E0402F2DC00", "Flow List", 24, ExecutionMode.Simplified)]
    public partial class DataFlowListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, 
        IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Flow List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new DataFlowListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "DataFlowList", "Flow List", Resources.flow_big, Resources.flow);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new DataFlowListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            return result;
        }
        #endregion
    }
}