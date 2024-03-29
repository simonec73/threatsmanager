﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Initializers;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ThreatActorList
{
    public partial class ThreatActorListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Threat Actor List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddActor", "Add Threat Actor",
                            Resources.actor_big_new,
                            Resources.actor_new,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "RemoveActor", "Remove Threat Actor",
                            Resources.actor_big_delete,
                            Resources.actor_delete, false),
                    }),
                    new CommandsBarDefinition("Restore", "Restore", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Restore", "Restore Standard Threat Actors",
                            Properties.Resources.undo_big,
                            Properties.Resources.undo),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh List",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
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
                var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                    .Select(x => x.GridRow)
                    .Distinct()
                    .ToArray();

                switch (action.Name)
                {
                    case "AddActor":
                        using (var dialog = new GenericIdentityCreationDialog())
                        {
                            dialog.IdentityTypeName = "Threat Actor";
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Add Threat Actor";
                                _model.AddThreatActor(dialog.IdentityName, dialog.IdentityDescription);
                            }
                        }
                        break;
                    case "RemoveActor":
                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Threat Actors. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Threat Actors,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Threat Actors", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IThreatActor actor)
                                            {
                                                r = _model.RemoveThreatActor(actor.Id);
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
                                            text = "Remove Threat Actors";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Threat Actors cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IThreatActor actor2)
                                        {
                                            if (_model.RemoveThreatActor(actor2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Threat Actor";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Threat Actor cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow?.Tag is IThreatActor actor &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Threat Actor '{actor.Name}'. Are you sure?",
                                         "Remove Threat Actor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveThreatActor(actor.Id))
                                {
                                    text = "Remove Threat Actor";
                                    _properties.Item = null;
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Threat Actor cannot be removed.";
                                }
                            }
                        }

                        break;
                    case "Restore":
                        var initializer = new ActorsInitializer();
                        initializer.Initialize(_model);
                        text = "Threat Actors restore";
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