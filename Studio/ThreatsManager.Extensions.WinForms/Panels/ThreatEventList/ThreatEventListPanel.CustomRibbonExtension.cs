using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public partial class ThreatEventListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

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
                        addRemove.Add(new ActionDefinition(Id, "AddVulnerability", "Add Vulnerability",
                            Resources.vulnerability_big_new,
                            Resources.vulnerability_new, false));
                    }
                    addRemove.Add(new ActionDefinition(Id, "RemoveThreatEvent", "Remove Threat Events",
                        Resources.threat_events_big_delete,
                        Resources.threat_events_delete, false));
                    addRemove.Add(new ActionDefinition(Id, "RemoveMitigation", "Remove Mitigations",
                        Resources.mitigations_big_delete,
                        Resources.mitigations_delete, false));
                    if (_executionMode == ExecutionMode.Pioneer)
                    {
                        addRemove.Add(new ActionDefinition(Id, "RemoveScenario", "Remove Scenarios",
                            Resources.scenario_big_delete,
                            Resources.scenario_delete, false));
                        addRemove.Add(new ActionDefinition(Id, "RemoveVulnerability", "Remove Vulnerabilities",
                            Resources.vulnerability_big_delete,
                            Resources.vulnerability_delete, false));
                    }

                    result.Add(new CommandsBarDefinition("AddRemove", "Add/Remove", addRemove));
                }

                result.Add(new CommandsBarDefinition("Severities", "Severities", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "ChangeSeverities", "Change Severities",
                        Resources.severity_big,
                        Resources.severity,
                        false)
                }));
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

            var selectedS = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatEventScenario)
                .Distinct()
                .ToArray();

            var currentS = _currentRow?.Tag as IThreatEventScenario;

            var selectedTEM = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatEventMitigation)
                .Distinct()
                .ToArray();

            var currentTEM = _currentRow?.Tag as IThreatEventMitigation;

            var selectedTE = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatEvent)
                .Distinct()
                .ToArray();

            var currentTE = _currentRow?.Tag as IThreatEvent;

            var selectedTT = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IThreatType)
                .Distinct()
                .ToArray();

            var currentTT = _currentRow?.Tag as IThreatType;

            var selectedV = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IVulnerability)
                .Distinct()
                .ToArray();

            var currentV = _currentRow?.Tag as IVulnerability;

            var selectedVM = _currentRow?.GridPanel?.SelectedCells?.OfType<GridCell>()
                .Select(x => x.GridRow)
                .Where(x => x.Tag is IVulnerabilityMitigation)
                .Distinct()
                .ToArray();

            var currentVM = _currentRow?.Tag as IVulnerabilityMitigation;

            try
            {
                switch (action.Name)
                {
                    case "AddScenario":
                        if (currentTE != null)
                        {
                            using (var dialog = new ThreatEventScenarioCreationDialog(currentTE))
                            {
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Scenario creation";
                                }
                            }
                        }
                        break;
                    case "AddMitigation":
                        if (currentTE != null)
                        {
                            using (var dialog = new ThreatEventMitigationSelectionDialog(currentTE))
                            {
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Mitigation creation";
                                }
                            }
                        }
                        else if (currentV != null)
                        {
                            using (var dialog = new VulnerabilityMitigationSelectionDialog(currentV))
                            {
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Mitigation creation";
                                }
                            }
                        }
                        break;
                    case "AddVulnerability":
                        if (currentTE != null)
                        {
                            using (var dialog = new WeaknessSelectionDialog())
                            {
                                dialog.Initialize(_model, currentTE);
                                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Vulnerability creation";
                                }
                            }
                        }
                        break;
                    case "RemoveThreatEvent":
                        if (currentTE != null)
                        {
                            if ((selectedTE?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedTE.Length} Threat Events. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Events,\nNo to remove only the last one you selected, '{currentTE.Name}' on '{currentTE.Parent?.Name}'.\nPress Cancel to abort.",
                                    "Remove Threat Events", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Threat Events"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedTE)
                                            {
                                                bool r = false;
                                                if (row.Tag is IThreatEvent te && te.Parent is IThreatEventsContainer container2)
                                                {
                                                    r = container2.RemoveThreatEvent(te.Id);
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
                                                text = "Remove Threat Events";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Threat Events cannot be removed.";
                                            }
                                            break;
                                        case DialogResult.No:
                                            if (currentTE.Parent is IThreatEventsContainer container &&
                                                container.RemoveThreatEvent(currentTE.Id))
                                            {
                                                scope?.Complete();

                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Threat Event";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Event cannot be removed.";
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Threat Event '{currentTE.Name}'. Are you sure?",
                                         "Remove Threat Event", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Threat Event"))
                                {
                                    if (currentTE.Parent is IThreatEventsContainer container &&
                                                container.RemoveThreatEvent(currentTE.Id))
                                    {
                                        scope?.Complete();
                                        text = "Remove Threat Event";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Threat Event cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "RemoveScenario":
                        if (currentS != null)
                        {
                            if ((selectedS?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedS.Length} Scenarios. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Scenarios,\nNo to remove only the last one you selected, '{currentS.Name}'.\nPress Cancel to abort.",
                                    "Remove Scenarios", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Scenarios"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedS)
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

                                            scope?.Complete();

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
                                            if (currentS.ThreatEvent.RemoveScenario(currentS.Id))
                                            {
                                                scope?.Complete();

                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Scenario";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Scenario cannot be removed.";
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove scenario '{currentS.Name}' from the current Threat Event. Are you sure?",
                                         "Remove Scenario", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Scenario"))
                                {
                                    if (currentS.ThreatEvent.RemoveScenario(currentS.Id))
                                    {
                                        scope?.Complete();
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
                        }
                        break;
                    case "RemoveMitigation":
                        if (currentTEM != null)
                        {
                            if ((selectedTEM?.Length ?? 0) > 1)
                            {
                                var name = currentTEM?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedTEM.Length} Mitigation associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigation associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Mitigation association", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Mitigations"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedTEM)
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

                                            scope?.Complete();

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
                                            if (currentTEM.ThreatEvent.RemoveMitigation(currentTEM.MitigationId))
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
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove mitigation '{currentTEM.Mitigation.Name}' from the current Threat Event. Are you sure?",
                                         "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation"))
                                {
                                    if (currentTEM.ThreatEvent.RemoveMitigation(currentTEM.MitigationId))
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
                        else if (currentVM != null)
                        {
                            if ((selectedVM?.Length ?? 0) > 1)
                            {
                                var name = currentVM?.Mitigation.Name;
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedVM.Length} Vulnerability Mitigation associations. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Mitigation associations,\nNo to remove only the last one you selected, '{name}'.\nPress Cancel to abort.",
                                    "Remove Vulnerability Mitigation association", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Vulnerability Mitigations"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedVM)
                                            {
                                                bool r = false;
                                                if (row.Tag is IVulnerabilityMitigation m)
                                                {
                                                    r = m.Vulnerability.RemoveMitigation(m.MitigationId);
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
                                                text = "Remove Vulnerability Mitigation association";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Vulnerability Mitigation associations cannot be removed.";
                                            }
                                            break;
                                        case DialogResult.No:
                                            if (currentVM.Vulnerability.RemoveMitigation(currentVM.MitigationId))
                                            {
                                                scope?.Complete();

                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Vulnerability Mitigation association";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Vulnerability Mitigation association cannot be removed.";
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove mitigation '{currentVM.Mitigation.Name}' from the current Vulnerability. Are you sure?",
                                         "Remove Vulnerability Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Vulnerability Mitigation"))
                                {
                                    if (currentVM.Vulnerability.RemoveMitigation(currentVM.MitigationId))
                                    {
                                        scope?.Complete();

                                        text = "Remove Vulnerability Mitigation association";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Vulnerability Mitigation association cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "RemoveVulnerability":
                        if (currentV != null)
                        {
                            if ((selectedV?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selectedV.Length} Vulnerabilities. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Vulnerabilities,\nNo to remove only the last one you selected, '{currentV.Name}'.\nPress Cancel to abort.",
                                    "Remove Vulnerabilities", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);

                                using (var scope = UndoRedoManager.OpenScope("Remove Vulnerabilities"))
                                {
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selectedV)
                                            {
                                                bool r = false;
                                                if (row.Tag is IVulnerability v)
                                                {
                                                    r = v.Parent?.RemoveVulnerability(v.Id) ?? false;
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
                                                text = "Remove Vulnerabilies";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Vulnerabilities cannot be removed.";
                                            }

                                            break;
                                        case DialogResult.No:
                                            if (currentV.Parent?.RemoveVulnerability(currentV.Id) ?? false)
                                            {
                                                scope?.Complete();

                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Vulnerability";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Vulnerability cannot be removed.";
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Vulnerability '{currentV.Name}' from the current Threat Event. Are you sure?",
                                         "Remove Vulnerability", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Vulnerability"))
                                {
                                    if (currentV.Parent?.RemoveVulnerability(currentV.Id) ?? false)
                                    {
                                        scope?.Complete();

                                        _properties.Item = null;
                                        _currentRow = null;
                                        text = "Remove Vulnerability";
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Vulnerability cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "ChangeSeverities":
                        if (currentTE != null && (selectedTE?.Length ?? 0) > 1)
                        {
                            var dialog = new SelectSeverityForItemsDialog(selectedTE?
                                .Select(x => x.Tag as IThreatEvent)
                                .Where(x => x != null), "Threat Events");
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                var severity = dialog.Severity;
                                if (severity != null)
                                {
                                    using (var scope = UndoRedoManager.OpenScope("Change Severities for Threat Events"))
                                    {
                                        foreach (var row in selectedTE)
                                        {
                                            bool ok = false;
                                            if (row.Tag is IThreatEvent te)
                                            {
                                                te.Severity = severity;
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

                                    text = "Update Threat Events Severity";
                                }
                            }
                        }
                        else if (currentS != null && (selectedS?.Length ?? 0) > 1)
                        {
                            var dialog = new SelectSeverityForItemsDialog(selectedS?
                                .Select(x => x.Tag as IThreatEventScenario)
                                .Where(x => x != null), "Scenarios");
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                var severity = dialog.Severity;
                                if (severity != null)
                                {
                                    using (var scope = UndoRedoManager.OpenScope("Change Severities for Scenarios"))
                                    {
                                        foreach (var row in selectedS)
                                        {
                                            bool ok = false;
                                            if (row.Tag is IThreatEventScenario s)
                                            {
                                                s.Severity = severity;
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

                                    text = "Update Scenarios Severity";
                                }
                            }
                        }
                        else if (currentV != null && (selectedV?.Length ?? 0) > 1)
                        {
                            var dialog = new SelectSeverityForItemsDialog(selectedV?
                                .Select(x => x.Tag as IVulnerability)
                                .Where(x => x != null), "Vulnerabilities");
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                var severity = dialog.Severity;
                                if (severity != null)
                                {
                                    using (var scope = UndoRedoManager.OpenScope("Change Severities for Vulnerabilities"))
                                    {
                                        foreach (var row in selectedV)
                                        {
                                            bool ok = false;
                                            if (row.Tag is IVulnerability v)
                                            {
                                                v.Severity = severity;
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

                                    text = "Update Vulnerabilities Severity";
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
                    default:
                        if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                        {
                            if (currentS != null && (selectedS?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Scope.ThreatEventScenario) != 0)
                            {
                                ProcessIdentities(identitiesContextAwareAction, selectedS, ref text, ref warning);
                            }
                            else if (currentTE != null && (selectedTE?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Scope.ThreatEvent) != 0)
                            {
                                ProcessIdentities(identitiesContextAwareAction, selectedTE, ref text, ref warning);
                            }
                            else if (currentTT != null && (selectedTT?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Scope.ThreatType) != 0)
                            {
                                ProcessIdentities(identitiesContextAwareAction, selectedTT, ref text, ref warning);
                            }
                            else if (currentV != null && (selectedV?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & Scope.Vulnerability) != 0)
                            {
                                ProcessIdentities(identitiesContextAwareAction, selectedV, ref text, ref warning);
                            }
                        }
                        else if (action.Tag is IPropertiesContainersContextAwareAction pcContextAwareAction)
                        {
                            if (currentS != null && (selectedS?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.ThreatEventScenario) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedS, ref text, ref warning);
                            }
                            else if (currentTEM != null && (selectedTEM?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.ThreatEventMitigation) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedTEM, ref text, ref warning);
                            }
                            else if (currentTE != null && (selectedTE?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.ThreatEvent) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedTE, ref text, ref warning);
                            }
                            else if (currentTT != null && (selectedTT?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.ThreatType) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedTT, ref text, ref warning);
                            }
                            else if (currentV != null && (selectedV?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.Vulnerability) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedV, ref text, ref warning);
                            }
                            else if (currentVM != null && (selectedVM?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & Scope.VulnerabilityMitigation) != 0)
                            {
                                ProcessContainers(pcContextAwareAction, selectedVM, ref text, ref warning);
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

        private void ProcessIdentities(IIdentitiesContextAwareAction identitiesContextAwareAction,
                                       IEnumerable<GridRow> selectedRows,
                                       ref string text,
                                       ref bool warning)
        {
            var identities = selectedRows?.Select(x => x.Tag as IIdentity)
                .Where(x => x != null)
                .ToArray();

            if (identities?.Any() ?? false)
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

        private void ProcessContainers(IPropertiesContainersContextAwareAction contextAwareAction,
                                       IEnumerable<GridRow> selectedRows,
                                       ref string text,
                                       ref bool warning)
        {
            var identities = selectedRows?.Select(x => x.Tag as IPropertiesContainer)
                .Where(x => x != null)
                .ToArray();

            if (identities?.Any() ?? false)
            {
                if (contextAwareAction.Execute(identities))
                {
                    text = contextAwareAction.Label;
                    _properties.Item = null;
                    _properties.Item = _currentRow?.Tag;
                }
                else
                {
                    text = $"{contextAwareAction.Label} failed.";
                    warning = true;
                }
            }
        }
    }
}