using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    public partial class PropertySchemaListPanel
    {
        private Dictionary<string, List<IContextAwareAction>> _actions;
        private List<string> _buckets;

        public Scope SupportedScopes => Scope.Process;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var effective = actions.Where(x => (x is IIdentityContextAwareAction) && 
                                               ((x.Scope & Scope.Process) != 0)).ToArray();
            if (effective.Any())
            {
                _buckets = new List<string>();
                _actions = new Dictionary<string, List<IContextAwareAction>>();

                foreach (var action in effective)
                {
                    var group = action.Group ?? string.Empty;
                    if (!_actions.TryGetValue(group, out var actionsInGroup))
                    {
                        actionsInGroup = new List<IContextAwareAction>();
                        _buckets.Add(group);
                        _actions.Add(group, actionsInGroup);
                    }
                    actionsInGroup.Add(action);
                }

                if (_buckets?.Any() ?? false)
                {
                    var first = true;
                    foreach (var bucket in _buckets)
                    {
                        if (first)
                            first = false;
                        else
                            _contextMenu.Items.Add(new ToolStripSeparator());
                        AddMenu(_actions[bucket]);
                    }
                }
            }
        }

        private void AddMenu([NotNull] List<IContextAwareAction> actions)
        {
            foreach (var action in actions)
            {
                _contextMenu.Items.Add(new ToolStripMenuItem(action.Label, null, DoAction)
                {
                    Tag = action
                });
            }
        }
 
        private void DoAction(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Tag is IContextAwareAction action)
            {
                var point = menuItem.GetCurrentParent().Location;
                var selectedRow = GetRow(_grid.PointToClient(point));
                if (selectedRow != null && selectedRow.Tag is IProcess process)
                    action.Execute(process);
            }
        }
    }
}