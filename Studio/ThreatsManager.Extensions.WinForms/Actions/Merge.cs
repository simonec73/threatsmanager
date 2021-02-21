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
    [Extension("8C93144F-3F6A-4023-A0BB-10298B6FC931", "Merge with Threat Model or Template", 15, ExecutionMode.Simplified)]
    public class Merge : IMainRibbonExtension, IStatusInfoProviderUpdateRequestor, 
        IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action UpdateStatusInfoProviders;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "Merge";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "Merge", "Merge Threat Models and Templates", Properties.Resources.arrows_merge_big,
                Properties.Resources.arrows_merge)
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
                    case "Merge":
                        var dialog = new OpenFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = true,
                            CheckPathExists = true,
                            DefaultExt = "tm",
                            DereferenceLinks = true,
                            Filter = "Threat Model (*.tm)|*.tm|Threat Model Template (*.tmt)|*.tmt|Threat Model Json Template (*.tmk)|*.tmk",
                            FilterIndex = 0,
                            Title = "Select file to be merged",
                            RestoreDirectory = true
                        };
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            using (var merge = new MergeDialog())
                            {
                                merge.SetExecutionMode(_executionMode);
                                if (merge.Initialize(threatModel, dialog.FileName))
                                {
                                    if (merge.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                    {
                                        RefreshPanels?.Invoke(this);
                                        var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
                                        if (factory != null)
                                        {
                                            ClosePanels?.Invoke(factory);
                                            UpdateStatusInfoProviders?.Invoke();
                                        }

                                        ShowMessage?.Invoke("Merge succeeded.");
                                    }
                                }
                            }
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Merge failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }
    }
}