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

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public partial class KnownMitigationListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Known Mitigation List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddMitigation", "Add Mitigation",
                            Resources.standard_mitigations_big_new,
                            Resources.standard_mitigations_new,
                            true, Shortcut.CtrlShiftE),
                        new ActionDefinition(Id, "AddThreatType", "Associate Threat Type",
                            Resources.threat_type_big_new,
                            Resources.threat_type_new,
                            false),
                        new ActionDefinition(Id, "RemoveMitigation", "Remove Mitigation",
                            Resources.standard_mitigations_big_delete,
                            Resources.standard_mitigations_delete, false),
                        new ActionDefinition(Id, "RemoveThreatType", "Remove Threat Type association",
                            Resources.threat_type_big_delete,
                            Resources.threat_type_delete, false),
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
                    case "AddMitigation":
                        using (var dialog = new MitigationCreationDialog(_model))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                text = "Add Mitigation";
                        }
                        break;
                    case "AddThreatType":
                        if (_currentRow != null && _currentRow.Tag is IMitigation mitigation2)
                        {
                            using (var dialog2 = new MitigationThreatTypeSelectionDialog())
                            {
                                dialog2.Initialize(mitigation2);
                                if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    dialog2.ThreatType?.AddMitigation(mitigation2, dialog2.Strength);
                                }
                            }
                        }

                        break;
                    case "RemoveMitigation":
                        var selected = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IMitigation)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Mitigations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigations,\nNo to remove only the last one you selected, '{_currentRow?.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Mitigations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IMitigation m)
                                            {
                                                r = _model.RemoveMitigation(m.Id);
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
                                            text = "Remove Mitigations";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Mitigations cannot be removed.";
                                        }
                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IMitigation m2)
                                        {
                                            if (_model.RemoveMitigation(m2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Mitigation";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Mitigation cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow != null && _currentRow.Tag is IMitigation mitigation3 &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Mitigation '{mitigation3.Name}'. Are you sure?",
                                         "Remove Mitigation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveMitigation(mitigation3.Id))
                                {
                                    _properties.Item = null;
                                    text = "Remove Mitigation";
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Mitigation cannot be removed.";
                                }
                            }
                            else
                            {
                                text = null;
                            }
                        }
                        break;
                    case "RemoveThreatType":
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
                                    $"You have selected {selected2.Length} Threat Types associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Type associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Threat Type associations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
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
                                            text = "Remove Threat Type associations";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Threat Type associations cannot be removed.";
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
                                                text = "Remove Threat Type association";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Type association cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow != null &&
                                     _currentRow.Tag is IThreatTypeMitigation threatTypeMitigation &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove '{threatTypeMitigation.Mitigation.Name}' as Standard Mitigation from Threat Type '{threatTypeMitigation.ThreatType.Name}'. Are you sure?",
                                         "Remove Threat Type association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (threatTypeMitigation.ThreatType.RemoveMitigation(threatTypeMitigation.MitigationId))
                                {
                                    text = "Remove Threat Type association";
                                    _properties.Item = null;
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Threat Type association cannot be removed.";
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