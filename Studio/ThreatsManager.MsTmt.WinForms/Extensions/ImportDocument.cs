﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
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
                            int diagrams = 0;
                            int externalInteractors = 0;
                            int processes = 0;
                            int dataStores = 0;
                            int flows = 0;
                            int trustBoundaries = 0;
                            int entityTypes = 0;
                            int threatTypes = 0;
                            int customThreatTypes = 0;
                            int threats = 0;
                            int missingThreats = 0;

                            bool success = false;

                            try
                            {
                                using (var scope = UndoRedoManager.OpenScope("Import TMT Document"))
                                {
                                    var importer = new Importer();
                                    importer.Import(threatModel, dialog.FileName, 1f, HandleUnassignedThreat,
                                        out diagrams, out externalInteractors, out processes, out dataStores,
                                        out flows, out trustBoundaries, out entityTypes, out threatTypes,
                                        out customThreatTypes, out threats, out missingThreats);
                                    scope?.Complete();
                                }

                                success = true;
                            }
                            catch (XmlException e)
                            {
                                ShowWarning?.Invoke($"TMT document is malformed or has an unsupported structure: {e.Message}");
                            }
                            catch (Exception e)
                            {
                                ShowWarning?.Invoke($"TMT document import failed: {e.Message}");
                            }

                            if (success)
                            {
                                RefreshPanels?.Invoke(this);
                                ShowMessage?.Invoke("TMT file imported successfully.");

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