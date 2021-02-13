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
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.MitigationList
{
    public partial class MitigationListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Mitigation List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_executionMode != ExecutionMode.Management)
                {
                    result.Add(new CommandsBarDefinition("Remove", "Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "RemoveThreatEvent", "Disassociate Selected Threat Event",
                            Resources.threat_event_big_delete,
                            Resources.threat_event_delete, false),
                    }));
                }

                result.Add(new CommandsBarDefinition("Outlining", "Outlining", new IActionDefinition[]
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
                    case "RemoveThreatEvent":
                        var selected = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Where(x => x.Tag is IThreatEventMitigation)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var name = (_currentRow?.Tag as IThreatEventMitigation)?.ThreatEvent.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Threat Event associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Event associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Threat Event associations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IThreatEventMitigation mitigation)
                                            {
                                                r = mitigation.ThreatEvent.RemoveMitigation(mitigation.MitigationId);
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
                                            text = "Remove Threat Event associations";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Threat Event associations cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IThreatEventMitigation mitigation2)
                                        {
                                            if (mitigation2.ThreatEvent.RemoveMitigation(mitigation2.MitigationId))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Threat Event association";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Event association cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow?.Tag is IThreatEventMitigation mitigation &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to disassociate mitigation '{mitigation.Mitigation.Name}' from the Threat Event '{mitigation.ThreatEvent.Name}'. Are you sure?",
                                         "Remove Threat Event association", MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (mitigation.ThreatEvent.RemoveMitigation(mitigation.MitigationId))
                                {
                                    text = "Remove Threat Event association";
                                    _properties.Item = null;
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Threat Event association cannot be removed.";
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