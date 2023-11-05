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
    [Extension("EB8C6DF4-8D5A-4637-AE99-FD982A8D22AF", "Export Knowledge Base Action", 50, ExecutionMode.Expert)]
    public class ExportKnowledgeBase : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Export;
        public string Bar => "Knowledge Base";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ExportKB", "Export Knowledge Base", Properties.Resources.export_template_big,
                Properties.Resources.export_template)
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
                    case "ExportKB":
                        using (var dialog = new ExportKnowledgeBaseDialog(threatModel))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                ShowMessage?.Invoke("Export Knowledge Base succeeded.");
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Export Knowledge Base failed.");
                throw;
            }
        }
    }
}