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

namespace ThreatsManager.Extensions.Panels.ProcessList
{
    public partial class ProcessListPanel
    {
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
                    }), 
                    new CommandsBarDefinition("Find", "Find", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "FindProcess", "Find Process in Diagrams",
                            Resources.process_big_view,
                            Resources.process_view, false),
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
                    case "AddProcess":
                        text = "Add Process";
                        _model.AddEntity<IProcess>();
                        break;
                    case "RemoveProcess":
                        var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
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
                        break;
                    case "FindProcess":
                        bool found = false;
                        if (_currentRow != null && _currentRow.Tag is IProcess process2)
                        {
                            var diagrams = _model.Diagrams?.ToArray();
                            if (diagrams?.Any() ?? false)
                            {
                                foreach (var diagram in diagrams)
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