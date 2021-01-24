using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Overview
{
    [Extension("88C93B44-F1B7-4874-ACBE-5D5BF2B0DF41", "Overview", 100, ExecutionMode.Business)]
    public partial class OverviewPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new OverviewPanel();

            action = new ActionDefinition(result.Id, "Overview", "Dashboard", Properties.Resources.gauge_big,
                Properties.Resources.gauge);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new OverviewPanel();
            //if (_actions != null)
            //    result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}