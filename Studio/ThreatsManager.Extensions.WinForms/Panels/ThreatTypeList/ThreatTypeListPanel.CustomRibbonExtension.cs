using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public partial class ThreatTypeListPanel
    {
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
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh List",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

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
                        if (_currentRow.Tag is IThreatType threatType2)
                        {
                            using (var dialog2 = new ThreatTypeMitigationSelectionDialog(threatType2))
                            {
                                if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Add Mitigation";
                                }
                            }
                        }

                        break;
                    case "RemoveThreatType":
                        var selected = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IThreatType)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Threat Types. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Types,\nNo to remove only the last one you selected, '{_currentRow?.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Threat Types", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
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
                                        if (_currentRow != null && _currentRow.Tag is IThreatType tt2)
                                        {
                                            if (_model.RemoveThreatType(tt2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Threat Type";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Type cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow != null && _currentRow.Tag is IThreatType threatType3 &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Threat Type '{threatType3.Name}'. Are you sure?",
                                         "Remove Threat Type", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveThreatType(threatType3.Id))
                                {
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
                        break;
                    case "RemoveMitigation":
                        var selected2 = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IThreatTypeMitigation)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected2?.Length ?? 0) > 1)
                            {
                                var name = (_currentRow.Tag as IThreatTypeMitigation)?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected2.Length} Mitigations associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigations associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Mitigations associations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected2)
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
                                        if (_currentRow != null && _currentRow.Tag is IThreatTypeMitigation m2 &&
                                            m2.ThreatType is IThreatType m2ThreatType)
                                        {
                                            if (m2ThreatType.RemoveMitigation(m2.MitigationId))
                                            {
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
                            else if (_currentRow != null && _currentRow.Tag is IThreatTypeMitigation mitigation &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to disassociate Mitigation '{mitigation.Mitigation.Name}'. Are you sure?",
                                         "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (mitigation.ThreatType.RemoveMitigation(mitigation.MitigationId))
                                {
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