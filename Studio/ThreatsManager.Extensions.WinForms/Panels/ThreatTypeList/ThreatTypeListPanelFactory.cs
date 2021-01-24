using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    [Extension("976AA76E-ECFB-4770-9113-C81F40D87092", "Threat Type List", 10, ExecutionMode.Simplified)]
    public partial class ThreatTypeListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The External Interactors List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ThreatTypeListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "ThreatTypeList", "Threat Type\nList", Resources.threat_types_big,
                Resources.threat_types);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new ThreatTypeListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}