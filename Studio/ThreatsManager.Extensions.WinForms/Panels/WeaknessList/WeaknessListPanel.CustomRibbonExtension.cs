using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.WeaknessList
{
    public partial class WeaknessListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Weakness List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddWeakness", "Add Weakness",
                            Resources.weakness_big_new,
                            Resources.weakness_new,
                            true, Shortcut.CtrlShiftT),
                        new ActionDefinition(Id, "AddMitigation", "Add Standard Mitigation",
                            Resources.standard_mitigations_big_new,
                            Resources.standard_mitigations_delete, false),
                        new ActionDefinition(Id, "RemoveWeakness", "Remove Weakness",
                            Resources.weakness_big_delete,
                            Resources.weakness_delete, false),
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

            try
            {
                var selectedW = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                    .Select(x => x.GridRow)
                    .Where(x => x.Tag is IWeakness)
                    .Distinct()
                    .ToArray();

                var selectedWM = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                    .Select(x => x.GridRow)
                    .Where(x => x.Tag is IWeaknessMitigation)
                    .Distinct()
                    .ToArray();

                switch (action.Name)
                {
                    case "AddWeakness":
                        using (var dialog = new WeaknessCreationDialog(_model))
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Add Weakness";
                            }
                        }
                        break;
                    case "AddMitigation":
                        if (_currentRow.Tag is IWeakness weakness2)
                        {
                            using (var dialog2 = new WeaknessMitigationSelectionDialog(weakness2))
                            {
                                if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Add Mitigation";
                                }
                            }
                        }

                        break;
                    case "RemoveWeakness":
                        if (_currentRow != null)
                        {
                            if ((selectedW?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedW.Length} Weaknesses. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Weaknesses,\nNo to remove only the last one you selected, '{_currentRow?.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Weaknesses", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selectedW)
                                        {
                                            bool r = false;
                                            if (row.Tag is IWeakness w)
                                            {
                                                r = _model.RemoveWeakness(w.Id);
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
                                            text = "Remove Weaknesses";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Weaknesses cannot be removed.";
                                        }
                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IWeakness w2)
                                        {
                                            if (_model.RemoveWeakness(w2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Weakness";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Weakness cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow != null && _currentRow.Tag is IWeakness w3 &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Weakness '{w3.Name}'. Are you sure?",
                                         "Remove Weakness", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveWeakness(w3.Id))
                                {
                                    text = "Weakness removal";
                                    _properties.Item = null;
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Weakness cannot be removed.";
                                }
                            }
                        }
                        break;
                    case "RemoveMitigation":
                        if (_currentRow != null)
                        {
                            if ((selectedWM?.Length ?? 0) > 1)
                            {
                                var name = (_currentRow.Tag as IWeaknessMitigation)?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedWM.Length} Mitigations associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigations associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Mitigations associations", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selectedWM)
                                        {
                                            bool r = false;
                                            if (row.Tag is IWeaknessMitigation m && 
                                                m.Weakness is IWeakness mWeakness)
                                            {
                                                r = mWeakness.RemoveMitigation(m.MitigationId);
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
                                        if (_currentRow != null && _currentRow.Tag is IWeaknessMitigation m2 &&
                                            m2.Weakness is IWeakness m2Weakness)
                                        {
                                            if (m2Weakness.RemoveMitigation(m2.MitigationId))
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
                            else if (_currentRow != null && _currentRow.Tag is IWeaknessMitigation mitigation &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to disassociate Mitigation '{mitigation.Mitigation.Name}'. Are you sure?",
                                         "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (mitigation.Weakness.RemoveMitigation(mitigation.MitigationId))
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
                    default:
                        if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                        {
                            if ((selectedW?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Interfaces.Scope.Weakness) != 0)
                            {
                                var identities = selectedW.Select(x => x.Tag as IIdentity)
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
                            else if ((selectedWM?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Interfaces.Scope.WeaknessMitigation) != 0)
                            {
                                var identities = selectedWM.Select(x => x.Tag as IIdentity)
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