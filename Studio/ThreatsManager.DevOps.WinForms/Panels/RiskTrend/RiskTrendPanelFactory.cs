using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.RiskTrend
{
    [Extension("A107AF8C-F969-4532-BB34-F8BAD3CE600C", "Risk Trend Panel", 100, ExecutionMode.Pioneer)]
    public partial class RiskTrendPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new RiskTrendPanel();
            action = new ActionDefinition(result.Id, "RiskTrendPanel", "Risk Trend", 
                Properties.Resources.chart_line_big, Properties.Resources.chart_line);
            return result;
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            return new RiskTrendPanel();
        }
    }
}
