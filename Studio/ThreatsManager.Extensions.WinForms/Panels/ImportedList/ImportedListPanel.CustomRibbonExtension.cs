using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.ImportedList
{
    public partial class ImportedListPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Imported List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Remove", "Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Remove", "Remove Selected",
                            Resources.undefined_big_delete,
                            Resources.undefined_delete, false),
                        new ActionDefinition(Id, "RemoveNotInUse", "Remove not in Use",
                            Resources.undefined_big_sponge,
                            Resources.undefined_sponge_small),
                    }),
                };

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
                switch (action.Name)
                {
                    case "Remove":
                        if (_currentRow != null)
                        {
                            var selected1 = _grid.GetSelectedCells()?.OfType<GridCell>()
                               .Select(x => x.GridRow)
                               .Where(x => (x.Tag is IIdentity identity) && IsSelected(identity, null, ImportedListFilter.NotApplied))
                               .Distinct()
                               .ToArray();

                            if (selected1?.Any() ?? false)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected1.Length} not in use object(s). Do you want to remove them all?",
                                    "Remove objects not in use", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                if (outcome == DialogResult.Yes)
                                {
                                    bool removed = true;
                                    using (var scope = UndoRedoManager.OpenScope("Remove objects"))
                                    {
                                        foreach (var row in selected1)
                                        {
                                            removed &= Remove(row);
                                        }

                                        scope?.Complete();
                                    }

                                    if (removed)
                                    {
                                        text = "Remove objects";
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "One or more objects cannot be removed.";
                                    }

                                    LoadModel();
                                }
                            }
                        }
                        break;
                    case "RemoveNotInUse":
                        var list = new List<GridRow>();
                        var selected2 = _grid.PrimaryGrid.Rows.OfType<GridRow>()
                           .Where(x => (x.Tag is IIdentity identity) && IsSelected(identity, null, ImportedListFilter.NotApplied))
                           .Distinct()
                           .ToArray();

                        if (selected2?.Any() ?? false)
                        {
                            if (MessageBox.Show(Form.ActiveForm, 
                                $"You are about to remove {selected2.Count()} objects that are not in use. Are you sure?",
                                "Remove objects not in use", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                using (var scope = UndoRedoManager.OpenScope("Remove Flows Not In Any Diagram"))
                                {
                                    bool removed = true;
                                    foreach (var row in selected2)
                                    {
                                        removed &= Remove(row);
                                    }

                                    scope?.Complete();

                                    if (removed)
                                    {
                                        text = "Remove objects not in use";
                                    }
                                    else
                                    {
                                        warning = true;
                                        text = "One or more objects not in use cannot be removed.";
                                    }
                                }

                                LoadModel();
                            }
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

        private bool Remove([NotNull] GridRow row)
        {
            bool result = false;

            if (row.Tag is IEntity entity)
            {
                result = _model.RemoveEntity(entity.Id);
            }
            else if (row.Tag is IGroup group)
            {
                result = _model.RemoveGroup(group.Id);
            }
            else if (row.Tag is IDataFlow flow)
            {
                result = _model.RemoveDataFlow(flow.Id);
            }
            else if (row.Tag is IDiagram diagram)
            {
                result = _model.RemoveDiagram(diagram.Id);
            }
            else if (row.Tag is IPropertySchema schema)
            {
                result = _model.RemoveSchema(schema.Id);
            }
            else if (row.Tag is IEntityTemplate entityTemplate)
            {
                result = _model.RemoveEntityTemplate(entityTemplate.Id);
            }
            else if (row.Tag is ITrustBoundaryTemplate trustBoundaryTemplate)
            {
                result = _model.RemoveTrustBoundaryTemplate(trustBoundaryTemplate.Id);
            }
            else if (row.Tag is IFlowTemplate flowTemplate)
            {
                result = _model.RemoveFlowTemplate(flowTemplate.Id);
            }
            else if (row.Tag is IThreatType threatType)
            {
                result = _model.RemoveThreatType(threatType.Id);
            }
            else if (row.Tag is IWeakness weakness)
            {
                result = _model.RemoveWeakness(weakness.Id);
            }
            else if (row.Tag is IMitigation mitigation)
            {
                result = _model.RemoveMitigation(mitigation.Id);
            }
            else if (row.Tag is IThreatActor actor)
            {
                result = _model.RemoveThreatActor(actor.Id);
            }

            if (result && row == _currentRow)
            {
                _properties.Item = null;
                _currentRow = null;
            }

            return result;
        }
    }
}