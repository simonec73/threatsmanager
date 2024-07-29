using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
#pragma warning disable CS0067
    [Extension("69304948-2E3E-4885-9B30-C7654ABEA908", "Import multiple Knowledge Bases Action", 11, ExecutionMode.Simplified)]
    public class ImportKnowledgeBases : IMainRibbonExtension, IDesktopAlertAwareExtension
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
        public string Bar => "Knowledge Base";

        private ProgressDialog _progress;
        private int _count;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportKBs", "Import multiple Knowledge Bases", Properties.Resources.import_template_big,
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
                    case "ImportKBs":
                        var kbManagers = ExtensionUtils.GetExtensions<IKnowledgeBaseManager>()?
                            .Where(x => x.SupportedLocations.HasFlag(LocationType.FileSystem)).ToArray();

                        if (kbManagers?.Any() ?? false)
                        {
                            var openFile = new OpenFileDialog()
                            {
                                Filter = kbManagers.GetFilter(),
                                Title = "Select one or more Knowledge Base files to import",
                                Multiselect = true
                            };

                            if (openFile.ShowDialog(Form.ActiveForm) == DialogResult.OK &&
                                (openFile.FileNames?.Any() ?? false))
                            {
                                var fileNames = openFile.FileNames.OrderBy(x => x).ToArray();
                                _count = fileNames.Length;

                                if (MessageBox.Show(Form.ActiveForm, $"You have chosen to import {_count} Knowledge Bases in full.\nPlease click Cancel if you want to selectively import their content, or if you want to import other Knowledge Bases; otherwise, please click OK.",
                                        "Import Knowledge Bases", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                                {
                                    var duplicationDefinition = new DuplicationDefinition()
                                    {
                                        AllEntityTemplates = true,
                                        AllFlowTemplates = true,
                                        AllMitigations = true,
                                        AllProperties = true,
                                        AllPropertySchemas = true,
                                        AllThreatTypes = true,
                                        AllTrustBoundaryTemplates = true,
                                        AllWeaknesses = true,
                                        AllThreatActors = true,
                                        AllSeverities = true,
                                        AllStrengths = true
                                    };

                                    IThreatModel knowledgeBase;
                                    int index = 0;
                                    
                                    if (_count > 3)
                                        ShowProgress();

                                    using (var scope = UndoRedoManager.OpenScope("Import Knowledge Bases"))
                                    {
                                        foreach (var fileName in fileNames)
                                        {
                                            knowledgeBase = null;

                                            try
                                            {
                                                var kbManager = kbManagers?
                                                    .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, fileName));
                                                knowledgeBase = kbManager?.Load(LocationType.FileSystem, fileName, false);
                                                if (knowledgeBase != null)
                                                    threatModel.Merge(knowledgeBase, duplicationDefinition);

                                                index++;
                                                UpdateProgress(index);
                                            }
                                            finally
                                            {
                                                if (knowledgeBase != null)
                                                    ThreatModelManager.Remove(knowledgeBase.Id);
                                            }
                                        }

                                        scope?.Complete();
                                    }
                                    
                                    CloseProgress();
                                    ShowMessage?.Invoke("Import Knowledge Bases succeeded.");
                                }
                            }
                        }
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Knowledge Bases failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        private void ShowProgress()
        {
            _progress = new ProgressDialog();
            _progress.Label = "Import Knowledge Bases is in progress...";
            _progress.Show(Form.ActiveForm);
        }

        private void UpdateProgress(int index)
        {
            if (_progress != null)
                _progress.Value = (int)(((float)index) / _count * 100);
        }

        private void CloseProgress()
        {
            _progress?.Close();
            _progress = null;
        }
    }
}