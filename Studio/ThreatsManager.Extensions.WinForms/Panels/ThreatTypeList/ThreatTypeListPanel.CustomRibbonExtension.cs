using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public partial class ThreatTypeListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Threat Type List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddThreatType", "Add Threat Type",
                            Resources.threat_type_big_new,
                            Resources.threat_type_new,
                            true, Shortcut.CtrlShiftT),
                        new ActionDefinition(Id, "AddMitigation", "Add Standard Mitigation",
                            Resources.standard_mitigations_big_new,
                            Resources.standard_mitigations_delete, false),
                        new ActionDefinition(Id, "RemoveThreatType", "Remove Threat Type",
                            Resources.threat_type_delete,
                            Resources.threat_type_delete, false),
                        new ActionDefinition(Id, "RemoveMitigation", "Remove Standard Mitigation",
                            Resources.standard_mitigations_big_delete,
                            Resources.standard_mitigations_delete, false),
                    }),
                    new CommandsBarDefinition("Severities", "Severities", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ChangeSeverities", "Change Severities",
                            Resources.severity_big,
                            Resources.severity,
                            false)
                    }),
                    new CommandsBarDefinition("Outlining", "Outlining", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "OpenAllNodes", "Full Expand",
                            Properties.Resources.elements_tree_big,
                            Properties.Resources.elements_tree, true),
                        new ActionDefinition(Id, "OpenBranch", "Expand Branch",
                            Properties.Resources.elements_cascade_big,
                            Properties.Resources.elements_cascade, true),
                        new ActionDefinition(Id, "Collapse", "Collapse All",
                            Properties.Resources.element_big,
                            Properties.Resources.element, true),
                    })
                };

                if (_commandsBarContextAwareActions?.Any() ?? false)
                {
                    foreach (var definitions in _commandsBarContextAwareActions.Values)
                    {
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        result.Add(new CommandsBarDefinition(definitions[0].Name, definitions[0].Label, actions));
                    }
                }

                result.Add(new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Refresh", "Refresh List",
                        Resources.refresh_big,
                        Resources.refresh,
                        true, Shortcut.F5)
                }));

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

            var selectedTT = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatType)
                .Distinct()
                .ToArray();

            var currentTT = _currentRow?.Tag as IThreatType;

            var selectedM = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatTypeMitigation)
                .Distinct()
                .ToArray();

            var currentM = _currentRow?.Tag as IThreatTypeMitigation;

            try
            {
                switch (action.Name)
                {
                    case "AddThreatType":
                        using (var dialog = new ThreatTypeCreationDialog(_model))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Add Threat Type";
                            }
                        }
                        break;
                    case "AddMitigation":
                        if (currentTT != null)
                        {
                            using (var dialog2 = new ThreatTypeMitigationSelectionDialog(currentTT))
                            {
                                if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Add Mitigation";
                                }
                            }
                        }

                        break;
                    case "RemoveThreatType":
                        if (currentTT != null)
                        {
                            if ((selectedTT?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedTT.Length} Threat Types. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Types,\nNo to remove only the last one you selected, '{currentTT.Name}'.\nPress Cancel to abort.",
                                    "Remove Threat Types", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Threat Types"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedTT)
                                            {
                                                bool r = false;
                                                if (row.Tag is IThreatType tt)
                                                {
                                                    r = _model.RemoveThreatType(tt.Id);
                                                }

                                                removed &= r;

                                                if (r && row == _currentRow)
                                                {
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                }
                                            }

                                            scope?.Complete();

                                            if (removed)
                                            {
                                                text = "Remove Threat Types";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Threat Types cannot be removed.";
                                            }
                                            break;
                                        case DialogResult.No:
                                            if (_model.RemoveThreatType(currentTT.Id))
                                            {
                                                scope?.Complete();

                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Threat Type";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Type cannot be removed.";
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Threat Type '{currentTT.Name}'. Are you sure?",
                                         "Remove Threat Type", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Threat Type"))
                                {
                                    if (_model.RemoveThreatType(currentTT.Id))
                                    {
                                        scope?.Complete();

                                        text = "Threat Type removal";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Threat Type cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "RemoveMitigation":
                        if (currentM != null)
                        {
                            if ((selectedM?.Length ?? 0) > 1)
                            {
                                var name = currentM?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedM.Length} Mitigations associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigations associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Mitigations associations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedM)
                                            {
                                                bool r = false;
                                                if (row.Tag is IThreatTypeMitigation m &&
                                                    m.ThreatType is IThreatType mThreatType)
                                                {
                                                    r = mThreatType.RemoveMitigation(m.MitigationId);
                                                }

                                                removed &= r;

                                                if (r && row == _currentRow)
                                                {
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                }
                                            }
                                                    
                                            scope?.Complete();

                                            if (removed)
                                            {
                                                text = "Remove Mitigation associations";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Mitigation associations cannot be removed.";
                                            }
                                            break;
                                        case DialogResult.No:
                                            if (currentM.ThreatType is IThreatType m2ThreatType)
                                            {
                                                if (m2ThreatType.RemoveMitigation(currentM.MitigationId))
                                                {
                                                    scope?.Complete();

                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                    text = "Remove Mitigation association";
                                                }
                                                else
                                                {
                                                    warning = true;
                                                    text = "The Mitigation association cannot be removed.";
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to disassociate Mitigation '{currentM.Mitigation.Name}'. Are you sure?",
                                         "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation"))
                                {
                                    if (currentM.ThreatType.RemoveMitigation(currentM.MitigationId))
                                    {
                                        scope?.Complete();

                                        text = "Remove Mitigation association";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Mitigation association cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "ChangeSeverities":
                        if (currentTT != null && (selectedTT?.Length ?? 0) > 1)
                        {
                            var dialog = new SelectSeverityForItemsDialog(selectedTT?
                                .Select(x => x.Tag as IThreatType)
                                .Where(x => x != null), "Threat Types");
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                var severity = dialog.Severity;
                                if (severity != null)
                                {
                                    using (var scope = UndoRedoManager.OpenScope("Change Severities"))
                                    {
                                        foreach (var row in selectedTT)
                                        {
                                            bool ok = false;
                                            if (row.Tag is IThreatType tt)
                                            {
                                                tt.Severity = severity;
                                                ok = true;
                                            }

                                            if (ok && row == _currentRow)
                                            {
                                                _properties.Item = null;
                                                _properties.Item = _currentRow;
                                            }
                                        }

                                        scope?.Complete();
                                    }

                                    text = "Update Threat Types Severity";
                                }
                            }
                        }
                        break;
                    case "OpenAllNodes":
                        try
                        {
                            _loading = true;
                            _grid.PrimaryGrid.ExpandAll(10);
                            Application.DoEvents();
                        }
                        finally
                        {
                            _loading = false;
                        }
                        break;
                    case "OpenBranch":
                        try
                        {
                            _loading = true;
                            if (_currentRow != null)
                            {
                                _currentRow.ExpandAll(10);
                                _currentRow.Expanded = true;
                                Application.DoEvents();
                            }
                        }
                        finally
                        {
                            _loading = false;
                        }
                        break;
                    case "Collapse":
                        try
                        {
                            _loading = true;
                            _grid.PrimaryGrid.CollapseAll();
                            Application.DoEvents();
                        }
                        finally
                        {
                            _loading = false;
                        }
                        break;
                    case "Refresh":
                        LoadModel();
                        break;
                    default:
                        if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                        {
                            if ((selectedTT?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Interfaces.Scope.ThreatType) != 0)
                            {
                                var identities = selectedTT.Select(x => x.Tag as IIdentity)
                                    .Where(x => x != null)
                                    .ToArray();

                                if (identities.Any())
                                {
                                    if (identitiesContextAwareAction.Execute(identities))
                                    {
                                        text = identitiesContextAwareAction.Label;
                                        _properties.Item = null;
                                        _properties.Item = _currentRow?.Tag;
                                    }
                                    else
                                    {
                                        text = $"{identitiesContextAwareAction.Label} failed.";
                                        warning = true;
                                    }
                                }
                            }
                            else if ((selectedM?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Interfaces.Scope.ThreatTypeMitigation) != 0)
                            {
                                var identities = selectedM.Select(x => x.Tag as IIdentity)
                                    .Where(x => x != null)
                                    .ToArray();

                                if (identities.Any())
                                {
                                    if (identitiesContextAwareAction.Execute(identities))
                                    {
                                        text = identitiesContextAwareAction.Label;
                                        _properties.Item = null;
                                        _properties.Item = _currentRow?.Tag;
                                    }
                                    else
                                    {
                                        text = $"{identitiesContextAwareAction.Label} failed.";
                                        warning = true;
                                    }
                                }
                            }

                        }
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }
    }
}