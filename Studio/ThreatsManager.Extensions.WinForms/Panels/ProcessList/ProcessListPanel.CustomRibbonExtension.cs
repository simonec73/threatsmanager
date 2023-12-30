using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ProcessList
{
    public partial class ProcessListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Process List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddProcess", "Add Process",
                            Resources.process_big_new,
                            Resources.process_new,
                            true, Shortcut.CtrlShiftP),
                        new ActionDefinition(Id, "RemoveProcess", "Remove Process",
                            Resources.process_big_delete,
                            Resources.process_delete, false),
                        new ActionDefinition(Id, "RemoveNotInDiagrams", "Remove not in Diagrams",
                            Properties.Resources.process_big_sponge,
                            Properties.Resources.process_sponge),
                    }, false), 
                    new CommandsBarDefinition("Find", "Find", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "FindProcess", "Find Process in Diagrams",
                            Resources.process_big_view,
                            Resources.process_view, false),
                    }, false)
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

                        result.Add(new CommandsBarDefinition(definitions[0].Name, definitions[0].Label, actions, 
                            definitions[0].Collapsible, definitions[0].CollapsedImage));
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
                var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                    .Select(x => x.GridRow)
                    .Distinct()
                    .ToArray();

                switch (action.Name)
                {
                    case "AddProcess":
                        text = "Add Process";
                        using (var scope = UndoRedoManager.OpenScope("Add Process"))
                        {
                            _model.AddEntity<IProcess>();
                            scope?.Complete();
                        }
                        break;
                    case "RemoveProcess":
                        if (_currentRow != null)
                        {
                            using (var scope = UndoRedoManager.OpenScope("Remove Processes"))
                            {
                                if ((selected?.Length ?? 0) > 1)
                                {
                                    var outcome = MessageBox.Show(Form.ActiveForm,
                                        $"You have selected {selected.Length} Processes. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Processes,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                        "Remove Processes", MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button3);
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selected)
                                            {
                                                bool r = false;
                                                if (row.Tag is IEntity entity)
                                                {
                                                    r = _model.RemoveEntity(entity.Id);
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
                                                text = "Remove Processes";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Processes cannot be removed.";
                                            }

                                            break;
                                        case DialogResult.No:
                                            if (_currentRow != null && _currentRow.Tag is IEntity entity2)
                                            {
                                                if (_model.RemoveEntity(entity2.Id))
                                                {
                                                    scope?.Complete();
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                    text = "Remove Process";
                                                }
                                                else
                                                {
                                                    warning = true;
                                                    text = "The Process cannot be removed.";
                                                }
                                            }

                                            break;
                                    }
                                }
                                else if (_currentRow != null && _currentRow.Tag is IProcess process &&
                                         MessageBox.Show(Form.ActiveForm,
                                             $"You are about to remove Process '{process.Name}'. Are you sure?",
                                             "Remove Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                             MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (_model.RemoveEntity(process.Id))
                                    {
                                        scope?.Complete();
                                        text = "Remove Process";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Process cannot be removed.";
                                    }
                                }
                            }
                        }
                        break;
                    case "RemoveNotInDiagrams":
                        var diagrams = _model.Diagrams?.ToArray();
                        var processes = _model.Entities?
                            .OfType<IProcess>()
                            .Where(x => !(diagrams?.Any(y => y.Entities?.Any(z => x.Id == z.AssociatedId) ?? false) ?? false))
                            .ToArray();
                        if (processes?.Any() ?? false)
                        {
                            if (MessageBox.Show(Form.ActiveForm,
                                $"You are about to remove {processes.Count()} Processes that are not associated with any Diagram. Are you sure?",
                                "Remove Processes not in any Diagram", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                var currentProcess = _currentRow?.Tag as IProcess;
                                if (currentProcess != null && processes.Any(x => x.Id == currentProcess.Id))
                                {
                                    _properties.Item = null;
                                    _currentRow = null;

                                }

                                using (var scope = UndoRedoManager.OpenScope("Remove Processes Not In Any Diagram"))
                                {
                                    var removed = true;
                                    foreach (var process in processes)
                                    {
                                        removed &= _model.RemoveEntity(process.Id);
                                    }

                                    if (removed)
                                    {
                                        text = "Remove Processes Not In Any Diagram ";
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "One or more Processes cannot be removed.";
                                    }

                                    scope?.Complete();
                                }
                            }
                        }
                        break;
                    case "FindProcess":
                        bool found = false;
                        if (_currentRow != null && _currentRow.Tag is IProcess process2)
                        {
                            var diagrams2 = _model.Diagrams?.ToArray();
                            if (diagrams2?.Any() ?? false)
                            {
                                foreach (var diagram in diagrams2)
                                {
                                    var shape = diagram.GetEntityShape(process2.Id);
                                    if (shape != null)
                                    {
                                        found = true;
                                        var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
                                        if (factory != null)
                                            OpenPanel?.Invoke(factory, diagram);
                                        break;
                                    }
                                }
                            }
                        }
  
                        if (!found)
                        {
                            warning = true;
                            text = "The Process has not been found in any Diagram.";
                        }
                      break;  
                    case "Refresh":
                        LoadModel();
                        break;
                    default:
                        if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                        {
                            if ((selected?.Any() ?? false) &&
                                (identitiesContextAwareAction.Scope & SupportedScopes) != 0)
                            {
                                var identities = selected.Select(x => x.Tag as IIdentity)
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
                        else if (action.Tag is IPropertiesContainersContextAwareAction pcContextAwareAction)
                        {
                            if ((selected?.Any() ?? false) &&
                                (pcContextAwareAction.Scope & SupportedScopes) != 0)
                            {
                                var containers = selected.Select(x => x.Tag as IPropertiesContainer)
                                    .Where(x => x != null)
                                    .ToArray();

                                if (containers.Any())
                                {
                                    if (pcContextAwareAction.Execute(containers))
                                    {
                                        text = pcContextAwareAction.Label;
                                        _properties.Item = null;
                                        _properties.Item = _currentRow?.Tag;
                                    }
                                    else
                                    {
                                        text = $"{pcContextAwareAction.Label} failed.";
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