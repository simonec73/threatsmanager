using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Actions
{
    [Extension("EF25DA12-4E06-43B9-BB7F-D94068E08522", "Manage Iterations Action", 63, ExecutionMode.Management)]
    public class ManageIterations : IMainRibbonExtension
    {
        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            (new DevOpsManageIterationsDialog(threatModel)).ShowDialog(Form.ActiveForm);
        }

        private Guid _id = new Guid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "DevOps";
        public IEnumerable<IActionDefinition> RibbonActions  => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ManageIterations", "Manage Iterations", Properties.Resources.iteration_big,
                Properties.Resources.iteration),
        };

        public string PanelsListRibbonAction => null;
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
    }
}
