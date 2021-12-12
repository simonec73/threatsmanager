using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Panels.CalculatedSeverityList
{
    [Extension("B0EEA312-DF27-47A3-A0D9-6648307E77DA", "Calculated Severity List", 100, ExecutionMode.Simplified)]
    public partial class CalculatedSeverityListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, 
        IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new CalculatedSeverityListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "CalculatedSeverityList", "Calculated Severity\nList", 
                Properties.Resources.threat_event_big_sum,
                Properties.Resources.threat_event_sum);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new CalculatedSeverityListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}