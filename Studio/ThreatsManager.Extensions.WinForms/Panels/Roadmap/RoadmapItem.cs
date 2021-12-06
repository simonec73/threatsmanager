using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public class RoadmapItem : GoBoxNode
    {
        private const float DefaultWidth = 320f;
        private const float DefaultMargin = 50f;
        private IMitigation _mitigation;
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

        public RoadmapItem([NotNull] IMitigation mitigation, Effectiveness effectiveness)
        {
            _mitigation = mitigation;

            PortBorderMargin = SizeF.Empty;
            
            Label.Text = mitigation.Name;
            ToolTipText = mitigation.Name;

            switch (effectiveness)
            {
                case Effectiveness.Unknown:
                    Label.TextColor = Color.White;
                    Header.BrushColor = ThreatModelManager.StandardColor;
                    break;
                case Effectiveness.Minor:
                    Label.TextColor = Color.Black;
                    Header.BrushColor = Color.White;
                    Header.BorderPenColor = Color.DarkGreen;
                    break;
                case Effectiveness.Average:
                    Label.TextColor = Color.Black;
                    Header.BrushColor = Color.LimeGreen;
                    Header.BorderPenColor = Color.LimeGreen;
                    break;
                case Effectiveness.Major:
                    Label.TextColor = Color.White;
                    Header.BrushColor = Color.DarkGreen;
                    Header.BorderPenColor = Color.DarkGreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectiveness), effectiveness, null);
            }

            if (effectiveness != Effectiveness.Unknown)
            {
                AddItem("Effectiveness", effectiveness.ToString());
            }
            var schemaManager = new EffortPropertySchemaManager(mitigation.Model);
            if (schemaManager.IsEffortSupported)
            {
                AddItem("Estimated Effort", CalculateMaxEffort(schemaManager).ToString());
            }

            var properties = ExtensionUtils.GetExtensions<IRoadmapPropertyProvider>()?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    var propertyValue = property?.GetValue(mitigation);
                    if (propertyValue != null)
                        AddItem(property.Name, propertyValue);
                }
            }
        }

        public IMitigation Mitigation => _mitigation;

        public void Refresh()
        {
            if (_mitigation != null)
            {
                var schemaManager = new EffortPropertySchemaManager(_mitigation.Model);
                if (schemaManager.IsEffortSupported)
                {
                    SetInfo("Estimated Effort", CalculateMaxEffort(schemaManager).ToString());
                }

                var properties = ExtensionUtils.GetExtensions<IRoadmapPropertyProvider>()?.ToArray();
                if (properties?.Any() ?? false)
                {
                    foreach (var property in properties)
                    {
                        var propertyValue = property.GetValue(_mitigation);
                        if (propertyValue != null)
                            SetInfo(property.Name, propertyValue);
                    }
                }
            }
        }

        private Effort CalculateMaxEffort([NotNull] EffortPropertySchemaManager schemaManager)
        {
            Effort result = Effort.Undefined;

            var model = _mitigation.Model;
            var propertyType = schemaManager.GetPropertyType();
            if (model != null && propertyType != null)
            {
                CalculateMaxEffort(model, propertyType);

                var entities = model.Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        var effort = CalculateMaxEffort(entity, propertyType);
                        if (effort > result)
                            result = effort;
                    }
                }

                var flows = model.DataFlows?.ToArray();
                if (flows?.Any() ?? false)
                {
                    foreach (var flow in flows)
                    {
                        var effort = CalculateMaxEffort(flow, propertyType);
                        if (effort > result)
                            result = effort;
                    }
                }
            }

            return result;
        }

        private Effort CalculateMaxEffort([NotNull] IThreatEventsContainer container, 
            [NotNull] IPropertyType propertyType)
        {
            Effort result = Effort.Undefined;

            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    var effort = threatEvent.Mitigations?
                        .FirstOrDefault(x => x.MitigationId == _mitigation.Id)?
                        .GetProperty(propertyType)?.StringValue?.GetEnumValue<Effort>();

                    if (effort.HasValue && effort > result)
                        result = effort.Value;
                }
            }

            return result;
        }

        #region Context menu.
        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            Scope scope = Scope.Mitigation;

            var effective = actions
                .Where(x => (x.Scope & scope) != 0 && (x is IIdentityContextAwareAction))
                .ToArray();
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

        public override GoContextMenu GetContextMenu(GoView view)
        {
            if (view is GoOverview) return null;
            var cm = new GoContextMenu(view);

            if (_buckets?.Any() ?? false)
            {
                bool first = true;
                foreach (var bucket in _buckets)
                {
                    if (first)
                        first = false;
                    else
                        cm.MenuItems.Add(new MenuItem("-"));
                    AddMenu(cm, _actions[bucket]);
                }
            }

            return cm;
        }

        private void AddMenu([NotNull] GoContextMenu menu, [NotNull] List<IContextAwareAction> actions)
        {
            foreach (var action in actions)
            {
                menu.MenuItems.Add(new MenuItem(action.Label, DoAction)
                {
                    Tag = action
                });
            }
        }

        private void DoAction(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem &&
                menuItem.Tag is IIdentityContextAwareAction action)
            {
                if (action.Execute(_mitigation))
                    Refresh();
            }
        }
        #endregion

        private void AddItem([Required] string key, string value = null)
        {
            var list = Items;
            var recordItem = new RoadmapSubItem();
            recordItem.Label.Width = (DefaultWidth / 2f) - DefaultMargin;
            recordItem.Label.WrappingWidth = (DefaultWidth / 2f) - DefaultMargin;
            recordItem.Label.Text = key;
            recordItem.Value.WrappingWidth = DefaultWidth / 2f - DefaultMargin;
            if (value != null)
                recordItem.Value.Text = value;
            list.Add(recordItem);
        }

        public void SetInfo([Required] string key, string value)
        {
            var item = Items.OfType<RoadmapSubItem>()
                .FirstOrDefault(x => string.CompareOrdinal(x.Label.Text, key) == 0);
            if (item != null)
                item.Value.Text = value;
            else
                AddItem(key, value);
        }

        private GoListGroup Header => (GoListGroup)((GoGroup)this.Body)[0];

        private GoListGroup Items => (GoListGroup)((GoGroup)this.Body)[1];

        protected override GoObject CreateBody() 
        {
            var container = new GoListGroup
            {
                Selectable = false,
                BrushColor = Color.White,
                Spacing = 2,
                TopLeftMargin = new SizeF(0, 0),
                BottomRightMargin = new SizeF(0, 2),
                Width = DefaultWidth
            };

            GoListGroup header = new GoListGroup
            {
                Orientation = Orientation.Horizontal,
                Selectable = false,
                AutoRescales = false,
                Width = DefaultWidth,
                Spacing = 2
            };

            GoText headerText = new GoText
            {
                DragsNode = true,
                Selectable = false,
                Editable = false,
                Wrapping = false,
                AutoRescales = false,
                AutoResizes = false,
                StringTrimming = StringTrimming.EllipsisCharacter,
                FontSize = 9,
                Width = DefaultWidth - DefaultMargin,
                TextColor = Color.White,
                Height = 16 * Dpi.Factor.Height
            };
            header.Add(headerText);

            GoImage img = new GoImage
            {
                Selectable = false,
                AutoRescales = false,
                AutoResizes = false,
                Size = new SizeF(16 * Dpi.Factor.Width, 16 * Dpi.Factor.Height),
                Visible = false
            };
            header.Add(img);

            container.Add(header);

            GoListGroup items = new GoListGroup();
            items.Selectable = false;
            container.Add(items);

            return container;
        }

        public void SetWidth(int width)
        {
            Header.Width = width;
            Header[0].Width = width - DefaultMargin;
            Items.Width = width;
            var items = Items.OfType<RoadmapSubItem>().ToArray();
            foreach (var item in items)
            {
                item.Label.Width = (width / 2f) - DefaultMargin;
                item.Label.WrappingWidth = (width / 2f) - DefaultMargin;
                item.Value.WrappingWidth = width / 2f - DefaultMargin;
            }
        }
    }
}
