using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public partial class ThreatEventListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Threat Event List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_executionMode != ExecutionMode.Management)
                {
                    var addRemove = new List<IActionDefinition>();
                    addRemove.Add(new ActionDefinition(Id, "AddMitigation", "Add Mitigation",
                        Resources.mitigations_big_new,
                        Resources.mitigations_new, false));
                    if (_executionMode == ExecutionMode.Pioneer)
                    {
                        addRemove.Add(new ActionDefinition(Id, "AddScenario", "Add Scenario",
                            Resources.scenario_big_new,
                            Resources.scenario_new, false));
                    }

                    addRemove.Add(new ActionDefinition(Id, "RemoveMitigation", "Remove Selected Mitigation",
                        Resources.mitigations_big_delete,
                        Resources.mitigations_delete, false));
                    if (_executionMode == ExecutionMode.Pioneer)
                    {
                        addRemove.Add(new ActionDefinition(Id, "RemoveScenario", "Remove Selected Scenario",
                            Resources.scenario_big_delete,
                            Resources.scenario_delete, false));
                    }

                    result.Add(new CommandsBarDefinition("AddRemove", "Add/Remove", addRemove));
                }

                result.Add(new CommandsBarDefinition("Outlining", "Outlining", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "OpenAllNodes", "Full Expand",
                        Properties.Resources.elements_tree_big,
                        Properties.Resources.elements_tree, true),
                    new ActionDefinition(Id, "OpenFirstLevel", "Expand to Threat Events",
                        Properties.Resources.elements_two_big,
                        Properties.Resources.elements_two, true),
                    new ActionDefinition(Id, "OpenBranch", "Expand Branch",
                        Properties.Resources.elements_cascade_big,
                        Properties.Resources.elements_cascade, true),
                    new ActionDefinition(Id, "Collapse", "Collapse All",
                        Properties.Resources.element_big,
                        Properties.Resources.element, true),
                }));

                result.Add(new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Refresh", "Refresh List",
                        Resources.refresh_big,
                        Resources.refresh,
                        true, Shortcut.F5),
                }));

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
                    case "AddScenario":
                        if (_properties.Item is IThreatEvent threatEvent)
                        {
                            using (var dialog = new ThreatEventScenarioCreationDialog(threatEvent))
                            {
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Scenario creation";
                                }
                            }
                        }
                        break;
                    case "AddMitigation":
                        if (_properties.Item is IThreatEvent threatEvent2)
                        {
                            using (var dialog = new ThreatEventMitigationSelectionDialog(threatEvent2))
                            {
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Mitigation creation";
                                }
                            }
                        }
                        break;
                    case "RemoveScenario":
                        var selected = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IThreatEventScenario)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Scenarios. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Scenarios,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Scenarios", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IThreatEventScenario s)
                                            {
                                                r = s.ThreatEvent.RemoveScenario(s.Id);
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
                                            text = "Remove Scenarios";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Scenarios cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IThreatEventScenario s2)
                                        {
                                            if (s2.ThreatEvent.RemoveScenario(s2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Scenario";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Scenario cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow?.Tag is IThreatEventScenario scenario &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove scenario '{scenario.Name}' from the current Threat Event. Are you sure?",
                                         "Remove Scenario", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (scenario.ThreatEvent.RemoveScenario(scenario.Id))
                                {
                                    text = "Remove Scenario";
                                    _properties.Item = null;
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Scenario cannot be removed.";
                                }
                            }
                        }
                        break;
                    case "RemoveMitigation":
                        var selected2 = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IThreatEventMitigation)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected2?.Length ?? 0) > 1)
                            {
                                var name = (_currentRow.Tag as IThreatEventMitigation)?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected2.Length} Mitigation associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigation associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Mitigation association", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected2)
                                        {
                                            bool r = false;
                                            if (row.Tag is IThreatEventMitigation m)
                                            {
                                                r = m.ThreatEvent.RemoveMitigation(m.MitigationId);
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
                                            text = "Remove Mitigation association";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Mitigation associations cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IThreatEventMitigation m2)
                                        {
                                            if (m2.ThreatEvent.RemoveMitigation(m2.MitigationId))
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
                            else if (_currentRow?.Tag is IThreatEventMitigation mitigation &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove mitigation '{mitigation.Mitigation.Name}' from the current Threat Event. Are you sure?",
                                         "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (mitigation.ThreatEvent.RemoveMitigation(mitigation.MitigationId))
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
                    case "OpenFirstLevel":
                        try
                        {
                            _loading = true;
                            _grid.PrimaryGrid.CollapseAll();
                            _grid.PrimaryGrid.ExpandAll(0);
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