using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Diagram
{
#pragma warning disable CS0067
    public partial class ModelPanelFactory
    {
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        
        public event Action<IPanelFactory, IIdentity> PanelCreationRequired;
        
        public event Action<IPanelFactory, IPanel> PanelDeletionRequired;
        
        public event Action<IPanelFactory, IPanel> PanelShowRequired;
        
        public event Action<IMainRibbonExtension> IteratePanels;
        
        public event Action<IMainRibbonExtension> RefreshPanels;
 
        public event Action<IPanelFactory> ClosePanels;

        public Guid Id => new Guid(this.GetExtensionId());
        public Ribbon Ribbon => Ribbon.Home;
        public string Bar => "Diagrams";

        public IEnumerable<IActionDefinition> RibbonActions
        {
            get
            {
                var result = new List<IActionDefinition>();

                result.Add(new ActionDefinition(Id, "IterateDiagrams", "Iterate Diagrams",
                    Resources.model_big,
                    Resources.model, false, Shortcut.CtrlShiftD));
                if (_executionMode != ExecutionMode.Business && _executionMode != ExecutionMode.Management)
                {
                    result.Add(new ActionDefinition(Id, "CreateDiagram", "Create Diagram",
                        Resources.model_big_new,
                        Resources.model_new));
                    result.Add(new ActionDefinition(Id, "ReorderDiagrams", "Reorder Diagrams", Properties.Resources.model_big_up,
                        Properties.Resources.model_up));
                }

                return result;
            }
        }

        public string PanelsListRibbonAction => "IterateDiagrams";

        public IEnumerable<IActionDefinition> GetStartPanelsList([NotNull] IThreatModel model)
        {
            IEnumerable<IActionDefinition> result = null;

            var diagrams = model.Diagrams?.ToArray();
            if (diagrams?.Any() ?? false)
            {
                result = diagrams.OrderBy(x => x.Order).Select(x =>
                    new ActionDefinition(x.Id, x.Name, x.Name, Resources.model,
                        Resources.model_small)
                    {
                        Tag = x
                    });
            }

            return result;
        }

        [InitializationRequired]
        public void ExecuteRibbonAction(IThreatModel threatModel, [NotNull] IActionDefinition action)
        {
            switch (action.Name)
            {
                case "CreateDiagram":
                    var diagram = threatModel.AddDiagram();
                    PanelCreationRequired?.Invoke(this, diagram);
                    ChangeRibbonActionStatus?.Invoke(this, "IterateDiagrams", true);
                    break;
                case "IterateDiagrams":
                    IteratePanels?.Invoke(this);
                    break;
                case "ReorderDiagrams":
                    using (var dialog = new DiagramSortDialog(threatModel))
                    {
                        dialog.ShowDialog(Form.ActiveForm);
                    }
                    break;
                default:
                    PanelShowRequired?.Invoke(this, null);
                    break;
            }
        }
    }
}