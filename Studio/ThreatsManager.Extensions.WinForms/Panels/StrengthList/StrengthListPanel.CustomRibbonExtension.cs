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

namespace ThreatsManager.Extensions.Panels.StrengthList
{
    public partial class StrengthListPanel
    {
        
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Strength List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddStrength", "Add Strength",
                            Resources.strength_big_new,
                            Resources.strength_new,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "RemoveStrength", "Remove Strength",
                            Resources.strength_big_delete,
                            Resources.strength_delete, false),
                    }),
                    new CommandsBarDefinition("Restore", "Restore", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Restore", "Restore Standard Strengths",
                            Properties.Resources.undo_big,
                            Properties.Resources.undo),
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
                    case "AddStrength":
                        using (var dialog = new StrengthCreationDialog(_model))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Add Strength";
                            }
                        }
                        break;
                    case "RemoveStrength":
                        var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Strengths. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Strengths,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Strengths", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IStrength strength)
                                            {
                                                r = _model.RemoveStrength(strength.Id);
                                            }

                                            removed &= r;

                                            if (r && row == _currentRow)
                                            {
                                                _currentRow = null;
                                            }
                                        }

                                        if (removed)
                                        {
                                            text = "Remove Strengths";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Strengths cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IStrength strength2)
                                        {
                                            if (_model.RemoveStrength(strength2.Id))
                                            {
                                                _currentRow = null;
                                                text = "Remove Strength";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Strength cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow?.Tag is IStrength strength &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Strength '{strength.Name}'. Are you sure?",
                                         "Remove Strength", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveStrength(strength.Id))
                                {
                                    text = "Remove Strength";
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Strength cannot be removed.";
                                }
                            }
                        }

                        break;
                    case "Restore":
                        var initializer = new StrengthInitializer();
                        initializer.Initialize(_model);
                        text = "Strengths restore";
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