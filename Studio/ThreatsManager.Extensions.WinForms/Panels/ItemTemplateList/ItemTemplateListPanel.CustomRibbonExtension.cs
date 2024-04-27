﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ItemTemplateList
{
    public partial class ItemTemplateListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Item Template List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddEntityTemplate", "Add Entity Template",
                            Properties.Resources.rubber_stamp_big_new,
                            Properties.Resources.rubber_stamp_new,
                            true, Shortcut.CtrlShiftP),
                        new ActionDefinition(Id, "AddFlowTemplate", "Add Flow Template",
                            Icons.Resources.flow_big_new,
                            Icons.Resources.flow_new),
                        new ActionDefinition(Id, "AddTrustBoundaryTemplate", "Add Trust Boundary Template",
                            Icons.Resources.trust_boundary_big_new,
                            Icons.Resources.trust_boundary_new),
                        new ActionDefinition(Id, "RemoveItemTemplates", "Remove Item Templates",
                            Properties.Resources.rubber_stamp_big_delete,
                            Properties.Resources.rubber_stamp_delete, false),
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
                var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                    .Select(x => x.GridRow)
                    .Distinct()
                    .ToArray();

                switch (action.Name)
                {
                    case "AddEntityTemplate":
                        using (var dialog = new CreateEntityTemplateDialog())
                        {
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Add Entity Template"))
                                {
                                    var entityTemplate = _model.AddEntityTemplate(dialog.EntityName,
                                        dialog.EntityDescription,
                                        dialog.BigImage,
                                        dialog.Image,
                                        dialog.SmallImage,
                                        dialog.EntityType);
                                    _model.AutoApplySchemas(entityTemplate);
                                    scope?.Complete();
                                }
                                text = "Add Entity Template";
                            }
                        }
                        break;
                    case "AddFlowTemplate":
                        using (var dialog = new GenericIdentityCreationDialog())
                        {
                            dialog.IdentityTypeName = "Flow Template";
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Add Flow Template"))
                                {
                                    var flowTemplate = _model.AddFlowTemplate(dialog.IdentityName,
                                        dialog.IdentityDescription);
                                    _model.AutoApplySchemas(flowTemplate);
                                    scope?.Complete();
                                }
                                text = "Add Flow Template";
                            }
                        }
                        break;
                    case "AddTrustBoundaryTemplate":
                        using (var dialog = new GenericIdentityCreationDialog())
                        {
                            dialog.IdentityTypeName = "Trust Boundary Template";
                            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Add Trust Boundary Template"))
                                {
                                    var trustBoundaryTemplate = _model.AddTrustBoundaryTemplate(dialog.IdentityName,
                                        dialog.IdentityDescription);
                                    _model.AutoApplySchemas(trustBoundaryTemplate);
                                    scope?.Complete();
                                }
                                text = "Add Trust Boundary Template";
                            }
                        }
                        break;
                    case "RemoveItemTemplates":
                        if (_currentRow != null)
                        {
                            using (var scope = UndoRedoManager.OpenScope("Remove Item Templates"))
                            {
                                if ((selected?.Length ?? 0) > 1)
                                {
                                    var outcome = MessageBox.Show(Form.ActiveForm,
                                        $"You have selected {selected.Length} Item Templates. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Item Templates,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                        "Remove Item Templates", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button3);
                                    switch (outcome)
                                    {
                                        case DialogResult.Yes:
                                            bool removed = true;
                                            foreach (var row in selected)
                                            {
                                                bool r = false;
                                                if (row.Tag is IEntityTemplate template)
                                                {
                                                    r = _model.RemoveEntityTemplate(template.Id);
                                                }
                                                else if (row.Tag is IFlowTemplate flowTemplate)
                                                {
                                                    r = _model.RemoveFlowTemplate(flowTemplate.Id);
                                                }
                                                else if (row.Tag is ITrustBoundaryTemplate trustBoundaryTemplate)
                                                {
                                                    r = _model.RemoveTrustBoundaryTemplate(trustBoundaryTemplate.Id);
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
                                                text = "Remove Item Templates";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "One or more Item Templates cannot be removed.";
                                            }

                                            break;
                                        case DialogResult.No:
                                            if (_currentRow?.Tag is IEntityTemplate template2)
                                            {
                                                if (_model.RemoveEntityTemplate(template2.Id))
                                                {
                                                    scope?.Complete();
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                    text = "Remove Entity Template";
                                                }
                                                else
                                                {
                                                    warning = true;
                                                    text = "The Entity Template cannot be removed.";
                                                }
                                            }
                                            else if (_currentRow?.Tag is IFlowTemplate template3)
                                            {
                                                if (_model.RemoveFlowTemplate(template3.Id))
                                                {
                                                    scope?.Complete();
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                    text = "Remove Flow Template";
                                                }
                                                else
                                                {
                                                    warning = true;
                                                    text = "The Flow Template cannot be removed.";
                                                }
                                            }
                                            else if (_currentRow?.Tag is ITrustBoundaryTemplate template4)
                                            {
                                                if (_model.RemoveTrustBoundaryTemplate(template4.Id))
                                                {
                                                    scope?.Complete();
                                                    _properties.Item = null;
                                                    _currentRow = null;
                                                    text = "Remove Trust Boundary Template";
                                                }
                                                else
                                                {
                                                    warning = true;
                                                    text = "The Trust Boundary Template cannot be removed.";
                                                }
                                            }

                                            break;
                                    }
                                }
                                else if (_currentRow?.Tag is IEntityTemplate template &&
                                         MessageBox.Show(Form.ActiveForm,
                                             $"You are about to remove Entity Template '{template.Name}'. Are you sure?",
                                             "Remove Entity Template", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                             MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (_model.RemoveEntityTemplate(template.Id))
                                    {
                                        scope?.Complete();
                                        text = "Remove Entity Template";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Entity Template cannot be removed.";
                                    }
                                }
                                else if (_currentRow?.Tag is IFlowTemplate flowTemplate &&
                                           MessageBox.Show(Form.ActiveForm,
                                               $"You are about to remove Flow Template '{flowTemplate.Name}'. Are you sure?",
                                               "Remove Flow Template", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                               MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (_model.RemoveFlowTemplate(flowTemplate.Id))
                                    {
                                        scope?.Complete();
                                        text = "Remove Flow Template";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Flow Template cannot be removed.";
                                    }
                                }
                                else if (_currentRow?.Tag is ITrustBoundaryTemplate trustBoundaryTemplate &&
                                           MessageBox.Show(Form.ActiveForm,
                                               $"You are about to remove Trust Boundary Template '{trustBoundaryTemplate.Name}'. Are you sure?",
                                               "Remove Trust Boundary Template", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                               MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (_model.RemoveTrustBoundaryTemplate(trustBoundaryTemplate.Id))
                                    {
                                        scope?.Complete();
                                        text = "Remove Trust Boundary Template";
                                        _properties.Item = null;
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "The Trust Boundary Template cannot be removed.";
                                    }
                                }
                            }
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
                        else if (action.Tag is IPropertiesContainersContextAwareAction containersContextAwareAction)
                        {
                            if ((selected?.Any() ?? false) &&
                                (containersContextAwareAction.Scope & SupportedScopes) != 0)
                            {
                                var containers = selected.Select(x => x.Tag as IPropertiesContainer)
                                    .Where(x => x != null)
                                    .ToArray();

                                if (containers.Any())
                                {
                                    if (containersContextAwareAction.Execute(containers))
                                    {
                                        text = containersContextAwareAction.Label;
                                        _properties.Item = null;
                                        _properties.Item = _currentRow?.Tag;
                                    }
                                    else
                                    {
                                        text = $"{containersContextAwareAction.Label} failed.";
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