using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Actions
{
#pragma warning disable CS0067
    [Extension("A9D601F2-E25A-4466-8A39-A56AC109CB87", "Configure DevOps Action", 62, ExecutionMode.Management)]
    public class Configure : IMainRibbonExtension
    {
        private static Configure _instance;

        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "DevOps";

        public Configure()
        {
            _instance = this;
        }
        
        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "Configure", "Configure", Properties.Resources.kanban_config_big,
                Properties.Resources.kanban_config, false),
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public async void ExecuteRibbonAction([NotNull] IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "Configure":
                        var dialog = new DevOpsConfigurationDialog();
                        await dialog.Initialize(threatModel);
                        dialog.ShowDialog(Form.ActiveForm);
                        var connector = DevOpsManager.GetConnector(threatModel);
                        Connect.ChangeDisconnectButtonStatus(connector, true);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        internal static void ChangeConfigureButtonStatus(bool status)
        {
            _instance?.ChangeRibbonActionStatus?.Invoke(_instance, "Configure", status);
        }
    }
}