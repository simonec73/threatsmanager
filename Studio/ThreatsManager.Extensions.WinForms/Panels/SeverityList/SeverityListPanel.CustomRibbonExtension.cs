using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Initializers;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.SeverityList
{
    public partial class SeverityListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Severity List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddSeverity", "Add Severity",
                            Resources.severity_big_new,
                            Resources.severity_new,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "RemoveSeverity", "Remove Severity",
                            Resources.severity_big_delete,
                            Resources.severity_delete, false),
                    }),
                    new CommandsBarDefinition("Restore", "Restore", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Restore", "Restore Standard Severities",
                            Properties.Resources.undo_big,
                            Properties.Resources.undo, false),
                    }),
                    new CommandsBarDefinition("Promote", "Promote", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Promote", "Get Full Rights",
                            Properties.Resources.caesar_big,
                            Properties.Resources.caesar,
                            true, Shortcut.None), 
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
                    case "AddSeverity":
                        using (var dialog = new SeverityCreationDialog(_model))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Add Severity";
                            }
                        }
                        break;
                    case "RemoveSeverity":
                        var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Severities. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Severities,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Severities", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is ISeverity severity)
                                            {
                                                r = _model.RemoveSeverity(severity.Id);
                                            }

                                            removed &= r;

                                            if (r && row == _currentRow)
                                            {
                                                _currentRow = null;
                                            }
                                        }

                                        if (removed)
                                        {
                                            text = "Remove Severities";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Severities cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow?.Tag is ISeverity severity2)
                                        {
                                            if (_model.RemoveSeverity(severity2.Id))
                                            {
                                                _currentRow = null;
                                                text = "Remove Severity";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Severity cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow?.Tag is ISeverity severity &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Severity '{severity.Name}'. Are you sure?",
                                         "Remove Severity", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveSeverity(severity.Id))
                                {
                                    text = "Remove Severity";
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Severity cannot be removed.";
                                }
                            }
                        }
                        break;
                    case "Restore":
                        var initializer = new SeveritiesInitializer();
                        initializer.Initialize(_model);
                        text = "Severities restore";
                        break;
                    case "Promote":
                        if (MessageBox.Show(Form.ActiveForm, Properties.Resources.MessagePromoteSeverities,
                            "ATTENTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            _promoted = true;
                            _grid.PrimaryGrid.Columns["Id"].AllowEdit = true;
                            ChangeCustomActionStatus?.Invoke("AddSeverity", true);
                            ChangeCustomActionStatus?.Invoke("RemoveSeverity", _currentRow?.Tag is ISeverity);
                            ChangeCustomActionStatus?.Invoke("Restore", true);
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