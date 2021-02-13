using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.AutoThreatGeneration.Panels.ThreatTypeList
{
    [Extension("0101866E-8DDE-4CD3-89A9-0637B6280305", "Auto Gen Rules List", 40, ExecutionMode.Expert)]
    public partial class AutoThreatTypeListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// This Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new AutoThreatTypeListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new AutoThreatTypeListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}