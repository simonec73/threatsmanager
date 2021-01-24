using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.RiskTrend
{
    [Extension("A107AF8C-F969-4532-BB34-F8BAD3CE600C", "Risk Trend Panel", 100, ExecutionMode.Business)]
    public class RiskTrendPanelFactory : IPanelFactory<Form>
    {
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new RiskTrendPanel();
            action = new ActionDefinition(result.Id, "RiskTrendPanel", "Risk Trend", Resources.flow_big, Resources.flow);

        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            throw new NotImplementedException();
        }
    }
}
