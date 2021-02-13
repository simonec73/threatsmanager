using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ThreatEventListForm : Form
    {
        private static List<ThreatEventListForm> _instances = new List<ThreatEventListForm>();
        private static Graphics _graphics = (new System.Windows.Forms.Label()).CreateGraphics();
        private static List<string> _buckets;
        private static Dictionary<string, List<IContextAwareAction>> _actions;

        public ThreatEventListForm()
        {
            InitializeComponent();
            _instances.Add(this);
        }

        
        public event Action<IThreatEvent> ThreatEventClicked;

        public static void SetActions(IEnumerable<IContextAwareAction> actions)
        {
            var effective = actions?.ToArray();
            if (effective?.Any() ?? false)
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

        public void ShowThreatEvents([NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            var sorted = threatEvents.OrderBy(x => x.Name).ToArray();

            foreach (var threatEvent in sorted)
            {
                var text = Trim(threatEvent.Name, _panel.Font, Width - 50);

                var item = new MetroTileItem()
                {
                    Text = text,
                    Tag = threatEvent,
                    TileColor = eMetroTileColor.RedOrange,
                    TileSize = new Size(100, 20),
                    Image = GetMitigationImage(threatEvent)
                };
                item.Click += ItemClicked;
                item.CheckedChanged += ItemContextMenu;
                _panel.Items.Add(item);
            }
        }

        private string Trim(string text, System.Drawing.Font font, int maxSizeInPixels)
        {
            var trimmedText = text;
            var currentSize = Convert.ToInt32(_graphics.MeasureString(trimmedText, font).Width);
            var ratio = Convert.ToDouble(maxSizeInPixels) / currentSize;
            var oldRatio = 0.0;
            while (ratio < 1.0)
            {
                trimmedText = String.Concat(
                    trimmedText.Substring(0, Convert.ToInt32(trimmedText.Length * ratio) - 3), 
                    "...");
                currentSize = Convert.ToInt32(_graphics.MeasureString(trimmedText, font).Width);
                oldRatio = ratio;
                ratio = Convert.ToDouble(maxSizeInPixels) / currentSize;
                if (oldRatio == ratio)
                {
                    trimmedText = trimmedText.Substring(0, trimmedText.Length - 4);
                    currentSize = Convert.ToInt32(_graphics.MeasureString(trimmedText, font).Width);
                    ratio = Convert.ToDouble(maxSizeInPixels) / currentSize;
                }
            }
            return trimmedText;
        }

        private static Image GetMitigationImage([NotNull] IThreatEvent threatEvent)
        {
            Image result = null;

            switch (threatEvent.GetMitigationLevel())
            {
                case MitigationLevel.Partial:
                    result = Resources.threat_circle_orange_small;
                    break;
                case MitigationLevel.Complete:
                    result = Resources.threat_circle_green_small;
                    break;
                default:
                    result = Resources.threat_circle_small;
                    break;
            }

            return result;
        }

        public static void CloseAll()
        {
            var instances = _instances.ToArray();
            foreach (var instance in instances)
            {
                instance.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _instances.Remove(this);
            base.OnClosed(e);
        }

        private void ItemContextMenu(object sender, EventArgs e)
        {
            if (sender is MetroTileItem item)
            {
                item.Checked = false;
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Tag = item.Tag
                };

                if (_buckets?.Any() ?? false)
                {
                    var first = true;
                    foreach (var bucket in _buckets)
                    {
                        if (first)
                            first = false;
                        else
                            ContextMenuStrip.Items.Add("-");
                        foreach (var action in _actions[bucket])
                        {
                            ContextMenuStrip.Items.Add(new ToolStripMenuItem(action.Label, action.SmallIcon, DoAction)
                            {
                                Tag = action
                            });
                        }
                    }
                }
            }
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            if (sender is MetroTileItem item && item.Tag is IThreatEvent threatEvent)
            {
                ThreatEventClicked?.Invoke(threatEvent);
            }
        }

        private void ThreatEventListForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _panel_Leave(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.Close();
        }
 
        private void DoAction(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Tag is IContextAwareAction action &&
                menuItem.Owner.Tag is IThreatEvent threatEvent)
            {
                this.Close();
                action.Execute(threatEvent);
            }
        }
    }
}
