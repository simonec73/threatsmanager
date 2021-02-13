using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
#pragma warning disable CS0067
    [Extension("B84C6DB6-F376-4C07-9001-6EAC0B9E7FFC", "Import Template Action", 10, ExecutionMode.Simplified)]
    public class ImportTemplate : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "Template";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportTemplate", "Import Template", Properties.Resources.import_template_big,
                Properties.Resources.import_template)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "ImportTemplate":
                        using (var dialog = new ImportTemplateDialog(threatModel))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                ShowMessage?.Invoke("Import Template succeeded.");
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Template failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
    }
}