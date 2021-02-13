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

namespace ThreatsManager.Extensions.Panels.DataFlowList
{
    public partial class DataFlowListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Flow List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    //new CommandsBarDefinition("Add", "Add", new IActionDefinition[]
                    //{
                    //    new ActionDefinition(Id, "AddDataFlow", "Add Flow",
                    //        Resources.flow_big_new,
                    //        Resources.flow_new,
                    //        true, Shortcut.CtrlShiftS),
                    //}),
                    new CommandsBarDefinition("Remove", "Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "RemoveDataFlow", "Remove Flow",
                            Resources.flow_big_delete,
                            Resources.flow_delete, false),
                    }),
                    new CommandsBarDefinition("Find", "Find", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "FindDataFlow", "Find Flow in Diagrams",
                            Resources.flow_big_view,
                            Resources.flow_view, false),
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
                    case "AddDataFlow":
                        //text = "Add Data Flow";
                        //_model.AddEntity<IDataStore>();
                        break;
                    case "RemoveDataFlow":
                        var selected = _grid.GetSelectedCells()?.OfType<GridCell>()
                            .Select(x => x.GridRow)
                            .Distinct()
                            .ToArray();

                        if (_currentRow != null)
                        {
                            if ((selected?.Length ?? 0) > 1)
                            {
                                var outcome = MessageBox.Show(Form.ActiveForm,
                                    $"You have selected {selected.Length} Flows. Do you want to remove them all?\nPlease click 'Yes' to remove all selected Flows,\nNo to remove only the last one you selected, '{_currentRow?.Tag?.ToString()}'.\nPress Cancel to abort.",
                                    "Remove Flows", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button3);
                                switch (outcome)
                                {
                                    case DialogResult.Yes:
                                        bool removed = true;
                                        foreach (var row in selected)
                                        {
                                            bool r = false;
                                            if (row.Tag is IDataFlow flow)
                                            {
                                                r = _model.RemoveDataFlow(flow.Id);
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
                                            text = "Remove Flows";
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "One or more Flows cannot be removed.";
                                        }
                                        break;
                                    case DialogResult.No:
                                        if (_currentRow != null && _currentRow.Tag is IDataFlow flow2)
                                        {
                                            if (_model.RemoveDataFlow(flow2.Id))
                                            {
                                                _properties.Item = null;
                                                _currentRow = null;
                                                text = "Remove Flow";
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "The Flow cannot be removed.";
                                            }
                                        }
                                        break;
                                }
                            }
                            else if (_currentRow != null && _currentRow.Tag is IDataFlow dataFlow &&
                                     MessageBox.Show(Form.ActiveForm,
                                         $"You are about to remove Flow '{dataFlow.Name}'. Are you sure?",
                                         "Remove Flow", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                if (_model.RemoveDataFlow(dataFlow.Id))
                                {
                                    _properties.Item = null;
                                    text = "Remove Flow";
                                }
                                else
                                {
                                    warning = true;
                                    text = "The Flow cannot be removed.";
                                }
                            }
                        }
                        break;
                    case "FindDataFlow":
                        bool found = false;
                        if (_currentRow != null && _currentRow.Tag is IDataFlow dataFlow2)
                        {
                            var diagrams = _model.Diagrams?.ToArray();
                            if (diagrams?.Any() ?? false)
                            {
                                foreach (var diagram in diagrams)
                                {
                                    var flow = diagram.GetLink(dataFlow2.Id);
                                    if (flow != null)
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
                            text = "The Flow has not been found in any Diagram.";
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