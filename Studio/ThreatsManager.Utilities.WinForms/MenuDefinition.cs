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
        private static object _context;

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
        public event Action<IContextAwareAction, object> MenuClicked;

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

        /// <summary>
        /// Modifies the visibility of a ContextMenuStrip based on the context passed as argument.
        /// </summary>
        /// <param name="menu">Menu containing the items to be analyzed.</param>
        /// <param name="context">Context of the analysis.</param>
        public static void UpdateVisibility(ContextMenuStrip menu, object context)
        {
            _context = context;
            var items = menu?.Items.OfType<ToolStripMenuItem>().Where(x => x.Tag is IContextAwareAction).ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    if (item.Tag is IContextAwareAction action)
                    {
                        item.Visible = action.IsVisible(context);
                    }
                }

                bool previousWasSeparator = false;
                ToolStripSeparator lastSeparator = null;

                foreach (ToolStripItem item in menu.Items)
                {
                    if (item is ToolStripSeparator separator)
                    {
                        item.Visible = !previousWasSeparator;

                        previousWasSeparator = true;
                        lastSeparator = separator;
                    } else if (item.Tag is IContextAwareAction action)
                    {
                        if (action.IsVisible(context))
                            previousWasSeparator = false;
                    }
                }

                if (previousWasSeparator)
                    lastSeparator.Visible = false;
            }
        }

        /// <summary>
        /// Modifies the visibility of a ContextMenu based on the context passed as argument.
        /// </summary>
        /// <param name="menu">Menu containing the items to be analyzed.</param>
        /// <param name="context">Context of the analysis.</param>
        public static void UpdateVisibility(ContextMenu menu, object context)
        {
            _context = context;
            var items = menu?.MenuItems.OfType<MenuItem>().Where(x => x.Tag is IContextAwareAction).ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    if (item.Tag is IContextAwareAction action)
                    {
                        item.Visible = context != null && action.IsVisible(context);
                    }
                }

                var allItems = menu.MenuItems.OfType<MenuItem>().ToArray();
                if (allItems.Any())
                {
                    bool previousWasSeparator = false;
                    MenuItem lastSeparator = null;

                    foreach (var item in allItems)
                    {
                        if (item.Tag is IContextAwareAction action)
                        {
                            if (action.IsVisible(context))
                                previousWasSeparator = false;
                        }
                        else
                        {
                            item.Visible = !previousWasSeparator;
                            previousWasSeparator = true;
                            lastSeparator = item;
                        }
                    }

                    if (previousWasSeparator)
                        lastSeparator.Visible = false;
                }
            }
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
                MenuClicked?.Invoke(action, _context);
            }
        }
        #endregion
    }
}
