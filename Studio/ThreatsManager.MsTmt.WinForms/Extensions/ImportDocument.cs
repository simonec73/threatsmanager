using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.MsTmt.Dialogs;
using ThreatsManager.MsTmt.Model;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Extensions
{
#pragma warning disable CS0067
    [Extension("1F2635C9-843E-45A6-83C0-2434496F6595", "Threat Model Import", 25, ExecutionMode.Simplified)]
    public class ImportDocument : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "MS TMT";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportDocument", "Import Document", Properties.Resources.tmt_big,
                Properties.Resources.tmt)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList([NotNull] IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction([NotNull] IThreatModel threatModel, [NotNull] IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "ImportDocument":
                        var dialog = new OpenFileDialog()
                        {
                            AddExtension = true,
                            CheckFileExists = true,
                            CheckPathExists = true,
                            DefaultExt = "tm7",
                            Filter = "TMT documents (*.tm7)|*.tm7|TMT templates (*.tb7)|*.tb7",
                            FilterIndex = 1,
                            Multiselect = false,
                            RestoreDirectory = true
                        };
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var importer = new Importer();
                            importer.Import(threatModel, dialog.FileName, Dpi.Factor.Height, HandleUnassignedThreat,
                                out var diagrams, out var externalInteractors, out var processes, out var dataStores,
                                out var flows, out var trustBoundaries, out var entityTypes, out var threatTypes, out var customThreatTypes, 
                                out var threats, out var missingThreats);
                            RefreshPanels?.Invoke(this);
                            ShowMessage?.Invoke("TMT7 file imported successfully.");

                            using (var resultDialog = new ImportResultDialog()
                            {
                                Properties = new ImportStatus()
                                {
                                    Diagrams = diagrams,
                                    ExternalInteractors = externalInteractors,
                                    Processes = processes,
                                    DataStores = dataStores,
                                    DataFlows = flows,
                                    TrustBoundaries = trustBoundaries,
                                    EntityTypes = entityTypes,
                                    ThreatTypes = threatTypes,
                                    CustomThreatTypes = customThreatTypes,
                                    Threats = threats,
                                    MissingThreats = missingThreats
                                }
                            })
                            {
                                resultDialog.ShowDialog(Form.ActiveForm);
                            }
                        }
                        break;
                }
            }
            catch (XmlException)
            {
                ShowWarning?.Invoke("TMT file import failed, probably because the document is malformed.");
                throw;
            }
            catch
            {
                ShowWarning?.Invoke("TMT file import failed.\nPlease close the document without saving it.");
                throw;
            }
        }
        
        private bool HandleUnassignedThreat(IThreatModel target, Threat threat, IThreatType threatType, IPropertySchema schema)
        {
            bool result = false;

            using (var dialog = new ThreatAssignmentDialog(target))
            {
                dialog.Initialize(threat.ToString(), threat.GetValueFromLabel("Description"));
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                {
                    var identity = dialog.SelectedIdentity;
                    if (identity != null)
                    {
                        if (identity is IEntity entity)
                        {
                            var threatEvent = entity.AddThreatEvent(threatType);
                            if (threatEvent != null)
                            {
                                Importer.AddProperties(threatEvent, threat, schema);
                                result = true;
                            }
                        }
                        else if (identity is IDataFlow dataFlow)
                        {
                            var threatEvent = dataFlow.AddThreatEvent(threatType);
                            if (threatEvent != null)
                            {
                                Importer.AddProperties(threatEvent, threat, schema);
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}