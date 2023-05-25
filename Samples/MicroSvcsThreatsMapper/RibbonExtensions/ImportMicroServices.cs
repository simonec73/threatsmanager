using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using System.Windows.Forms;
using MicroSvcsThreatsMapper.Libraries;

namespace MicroSvcsThreatsMapper.RibbonExtensions
{
    [Extension("45636082-DB4B-4E9A-8D25-06384EFA104F",
        "Load Micro Services Context Aware Action", 100, ExecutionMode.Expert)]
    public class ImportMicroServices : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "Importing";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportMicroSvc", "Import Micro Services JSON", Properties.Resources.OpenShift_Clusters,
                Properties.Resources.OpenShift_Clusters)
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
                    case "ImportMicroSvc":
                        var dialog = new OpenFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = true,
                            CheckPathExists = true,
                            DefaultExt = "json",
                            DereferenceLinks = true,
                            Filter = "Micro Services JSON file (*.json)|*.json",
                            FilterIndex = 0,
                            Title = "Select Micro Services JSON file to load",
                            RestoreDirectory = true
                        };
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            threatModel = new ThreatModelLoader().LoadThreatModel(dialog.FileName,
                                                                                  threatModel);
                            ShowMessage?.Invoke("Import Micro Services ThreatModel succeeded.");
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Micro Services ThreatModel failed.");
            }
        }
    }
}
