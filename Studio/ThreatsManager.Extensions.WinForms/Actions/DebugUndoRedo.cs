using PostSharp.Patterns.Recording;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("8EA0E3EB-3935-4086-AE37-D65053B17B3B", "Show Debug Undo Redo dialog", 1000, ExecutionMode.Pioneer)]
    public class DebugUndoRedo : IMainRibbonExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Home;
        public string Bar => "Debug";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "DebugUndoRedo", "Show Undo/Redo Operations", Icons.Resources.vulnerability_big,
                Icons.Resources.vulnerability)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            switch (action.Name)
            {
                case "DebugUndoRedo":
                    var dialog = new UndoRedoOperationsDialog();
                    dialog.Show(Form.ActiveForm);

                    break;
            }
        }
    }
}