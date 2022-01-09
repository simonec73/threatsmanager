using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.DevOps.Panels.MitigationsKanban;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Actions
{
#pragma warning disable CS0067
    [Extension("C013544F-7794-464F-9040-36A7B03170E4", "Connect to DevOps Action", 60, ExecutionMode.Management)]
    public class Connect : IMainRibbonExtension, IStatusInfoProviderUpdateRequestor, IDesktopAlertAwareExtension
    {
        private static Connect _instance;

        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action UpdateStatusInfoProviders;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "DevOps";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "Connect", "Connect", Properties.Resources.devops_big,
                Properties.Resources.devops),
            new ActionDefinition(Id, "Disconnect", "Disconnect", Properties.Resources.devops_big_forbidden,
                Properties.Resources.devops_forbidden, false)
        };

        public string PanelsListRibbonAction => null;

        public Connect()
        {
            _instance = this;
        }

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                var connector = DevOpsManager.GetConnector(threatModel);

                switch (action.Name)
                {
                    case "Connect":
                        var dialog = new DevOpsConnectionDialog();
                        if (connector == null)
                        {
                            var schemaManager = new DevOpsConfigPropertySchemaManager(threatModel);
                            dialog.Connection = schemaManager.ConnectionInfo;
                        }
                        else
                        {
                            dialog.Connector = connector;
                        }
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            connector = dialog.Connector;
                            var project = dialog.ProjectName;
                            if (connector != null && !string.IsNullOrWhiteSpace(project))
                            {
                                connector.OpenProject(project);
                                DevOpsManager.Register(connector, threatModel);
                                ChangeDisconnectButtonStatus(connector, true);
                            }
                        }

                        break;
                    case "Disconnect":
                        if (connector != null && MessageBox.Show(Form.ActiveForm,
                                "You are about to disconnect the current DevOps Connector.\nIf you proceed, all the links between the Threat Model and the DevOps system may be lost.\nAre you sure?",
                                "DevOps Disconnect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                            DialogResult.Yes)
                        {
                            DevOpsManager.Unregister(threatModel);
                            ChangeDisconnectButtonStatus(null, false);
                        }
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Connection failed.");
                throw;
            }
        }

        internal static void ChangeDisconnectButtonStatus(IDevOpsConnector connector, bool status)
        {
            _instance?.ChangeRibbonActionStatus?.Invoke(_instance, "Disconnect", status);
            Configure.ChangeConfigureButtonStatus(status);

            var configured = connector?.IsConfigured() ?? false;
            MitigationsKanbanPanelFactory.ChangeConfigureButtonStatus(configured && status);
        }
    }
}