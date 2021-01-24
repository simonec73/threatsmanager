using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Utilities.WinForms
{
    /// <summary>
    /// Menu definition class, used to configure the context menu based on the Scope.
    /// </summary>
    public class MenuDefinition
    {
        protected Dictionary<string, List<IContextAwareAction>> _actions;
        protected List<string> _buckets;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="actions">Defined actions.</param>
        /// <param name="scope">Scope defining the Menu.</param>
        public MenuDefinition([NotNull] IEnumerable<IContextAwareAction> actions, Scope scope)
        {
            var effective = actions.Where(x => ((x.Scope & scope) != 0) && IsApplicable(x, scope)).ToArray();
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
            }
        }

        /// <summary>
        /// Event raised when the Menu action is clicked.
        /// </summary>
        public event Action<Point, IContextAwareAction> MenuClicked;

        /// <summary>
        /// Creates the Menu associated to the MenuDefinition.
        /// </summary>
        /// <returns></returns>
        public virtual ContextMenuStrip CreateMenu()
        {
            ContextMenuStrip result = new ContextMenuStrip();

            if (_buckets?.Any() ?? false)
            {
                var first = true;
                foreach (var bucket in _buckets)
                {
                    if (first)
                        first = false;
                    else
                        result.Items.Add(new ToolStripSeparator());
                    AddMenu(result, _actions[bucket]);
                }
            }

            return result;
        }

        #region Private or protected members.
        private bool IsApplicable(IContextAwareAction action, Scope scope)
        {
            bool result = false;

            if ((scope & (Scope.Entity | Scope.DataFlow | Scope.Group | Scope.Diagram | Scope.ItemTemplate | 
                          Scope.Threats | Scope.Mitigation | Scope.ThreatModel | Scope.ThreatActor | Scope.PropertySchema)) != 0)
                result = action is IIdentityContextAwareAction;
            if (!result && (scope & Scope.Link) != 0)
                result = action is ILinkContextAwareAction;
            if (!result && (scope & Scope.EntityShape) != 0)
                result = action is IShapeContextAwareAction || action is IShapesContextAwareAction;
            if (!result && (scope & Scope.GroupShape) != 0)
                result = action is IShapeContextAwareAction || action is IShapesContextAwareAction;
            if (!result && (scope & Scope.ThreatEventMitigation) != 0)
                result = action is IThreatEventMitigationContextAwareAction;
            if (!result && (scope & Scope.ThreatTypeMitigation) != 0)
                result = action is IThreatTypeMitigationContextAwareAction;

            return result;
        }

        protected void AddMenu([NotNull] ContextMenuStrip menu, [NotNull] List<IContextAwareAction> actions)
        {
            foreach (var action in actions)
            {
                menu.Items.Add(new ToolStripMenuItem(action.Label, action.SmallIcon, DoAction)
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
                MenuClicked?.Invoke(point, action);
            }
        }
        #endregion
    }
}
