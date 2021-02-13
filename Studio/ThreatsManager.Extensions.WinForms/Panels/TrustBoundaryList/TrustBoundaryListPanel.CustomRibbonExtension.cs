using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.TrustBoundaryList
{
    public partial class TrustBoundaryListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Trust Boundary List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddTrustBoundary", "Add Trust Boundary",
                            Resources.trust_boundary_big_new,
                            Resources.trust_boundary_new,
                            true),
                        new ActionDefinition(Id, "RemoveTrustBoundary", "Remove Trust Boundary",
                            Resources.trust_boundary_big_delete,
                            Resources.trust_boundary_delete, false),
                    }),
                    new CommandsBarDefinition("Find", "Find", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "FindTrustBoundary", "Find Trust Boundary in Diagrams",
                            Resources.trust_boundary_big_view,
                            Resources.trust_boundary_view, false),
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
                    case "AddTrustBoundary":
                        text = "Add Trust Boundary";
                        _model.AddGroup<ITrustBoundary>();
                        break;
                    case "RemoveTrustBoundary":
                        var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Trust Boundaries. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Trust Boundaries,\nNo to remove only the last one you selected, '{_currentRow.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Trust Boundaries", MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IGroup group)
                                            {
                                                r = _model.RemoveGroup(group.Id);
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
                                            text = "Remove Trust Boundaries";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Trust Boundaries cannot be removed.";
                                        }

                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IGroup group2)
                                        {
                                            if (_model.RemoveGroup(group2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Trust Boundary";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Trust Boundary cannot be removed.";
                                            }
                                        }

                                        break;
                                }
                            }
                            else if (_currentRow != null && _currentRow.Tag is ITrustBoundary tb &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Trust Boundary '{tb.Name}'. Are you sure?",
                                         "Remove Trust Boundary", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveGroup(tb.Id))
                                {
                                    _properties.Item = null;
                                    text = "Remove Trust Boundary";
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Trust Boundary cannot be removed.";
                                }
                            }
                        }
                        break;
                    case "FindTrustBoundary":
                        bool found = false;
                        if (_currentRow != null && _currentRow.Tag is ITrustBoundary trustBoundary2)
                        {
                            var diagrams = _model.Diagrams?.ToArray();
                            if (diagrams?.Any() ?? false)
                            {
                                foreach (var diagram in diagrams)
                                {
                                    var shape = diagram.GetGroupShape(trustBoundary2.Id);
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
                            text = "The Trust Boundary has not been found in any Diagram.";
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