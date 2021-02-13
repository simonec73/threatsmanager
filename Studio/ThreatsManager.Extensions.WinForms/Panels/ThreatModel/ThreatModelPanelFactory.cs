using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ThreatModel
{
    [Extension("245F5E56-2BE9-4482-B2C2-FDFEC46A997F", "Threat Model Properties", 20, ExecutionMode.Simplified)]
    public partial class ThreatModelPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Threat Model List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ThreatModelPanel();
            action = new ActionDefinition(result.Id, "ThreatModelList", "Threat Model\nProperties", Resources.threat_model_big,
                Resources.threat_model);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new ThreatModelPanel();
            return result;
        }
        #endregion
    }
}