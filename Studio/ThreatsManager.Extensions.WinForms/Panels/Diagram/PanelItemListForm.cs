﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class PanelItemListForm : Form
    {
        private static List<PanelItemListForm> _instances = new List<PanelItemListForm>();
        private static Graphics _graphics = (new Label()).CreateGraphics();
        private static List<string> _buckets;
        private static Dictionary<string, List<IContextAwareAction>> _actions;
        private readonly object _referenceObject;
        private PanelItem _currentPanelItem;

        private const string cEdit = "<a href=\"Edit\">Edit the item.</a>";
        private const string cRemove = "<a href=\"Remove\">Remove the item.</a>";

        public PanelItemListForm(object referenceObject)
        {
            InitializeComponent();
            _instances.Add(this);

            _referenceObject = referenceObject;
            PanelItemListFormTrigger.PanelStatusUpdated += PanelItemListFormTrigger_PanelStatusUpdated;
        }

        private void PanelItemListFormTrigger_PanelStatusUpdated(PanelsStatus status, Northwoods.Go.GoView view)
        {
            if (status == PanelsStatus.Normal)
                this.Close();
        }

        /// <summary>
        /// Event raised when a panel item is clicked.
        /// </summary>
        /// <remarks>The returned object is the Tag associated with the Panel Item.</remarks>
        public event Action<object> PanelItemClicked;

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
            else
            {
                _buckets = null;
                _actions = null;
            }
        }

        public void ShowPanelItems([NotNull] IEnumerable<PanelItem> panelItems)
        {
            var sorted = panelItems.OrderBy(x => x.Name).ToArray();

            foreach (var panelItem in sorted)
            {
                var text = Trim(panelItem.Name, _panel.Font, Width - 50);

                var item = new MetroTileItem()
                {
                    Text = text,
                    Tag = panelItem,
                    TileColor = ConvertColor(panelItem.BackColor),
                    TileSize = new Size(100, 20),
                    Image = panelItem.Icon
                };
                item.Click += ItemClicked;
                item.CheckedChanged += ItemContextMenu;
                _panel.Items.Add(item);
                if (panelItem.ActionOnClick == ClickAction.ShowTooltip)
                {
                    var footer = GetTooltipFooter(panelItem);
                    _superTooltip.SetSuperTooltip(item, new SuperTooltipInfo()
                    {
                        HeaderText = panelItem.Name,
                        HeaderVisible = true,
                        BodyText = panelItem.TooltipText,
                        FooterText = footer,
                        FooterVisible = !string.IsNullOrWhiteSpace(footer)
                    });
                }
            }
        }

        private eMetroTileColor ConvertColor(Color color)
        {
            eMetroTileColor result = eMetroTileColor.Default;

            if (color.IsKnownColor)
            {
                switch (color.ToKnownColor())
                {
                    case KnownColor.Green:
                        result = eMetroTileColor.Green;
                        break;
                    case KnownColor.Orange:
                        result = eMetroTileColor.Orange;
                        break;
                    case KnownColor.Magenta:
                        result = eMetroTileColor.Magenta;
                        break;
                    case KnownColor.Blue:
                        result = eMetroTileColor.Blue;
                        break;
                    case KnownColor.Teal:
                        result = eMetroTileColor.Teal;
                        break;
                    case KnownColor.Plum:
                        result = eMetroTileColor.Plum;
                        break;
                    case KnownColor.MediumVioletRed:
                    case KnownColor.PaleVioletRed:
                        result = eMetroTileColor.RedViolet;
                        break;
                    case KnownColor.Olive:
                        result = eMetroTileColor.Olive;
                        break;
                    case KnownColor.DarkOliveGreen:
                        result = eMetroTileColor.DarkOlive;
                        break;
                    case KnownColor.Maroon:
                        result = eMetroTileColor.Maroon;
                        break;
                    case KnownColor.Yellow:
                        result = eMetroTileColor.Yellow;
                        break;
                    case KnownColor.LightGoldenrodYellow:
                    case KnownColor.LightYellow:
                        result = eMetroTileColor.Yellowish;
                        break;
                    case KnownColor.LightBlue:
                    case KnownColor.LightSkyBlue:
                    case KnownColor.SkyBlue:
                        result = eMetroTileColor.Blueish;
                        break;
                    case KnownColor.DarkBlue:
                        result = eMetroTileColor.DarkBlue;
                        break;
                    case KnownColor.Gray:
                    case KnownColor.White:
                        result = eMetroTileColor.Gray;
                        break;
                    case KnownColor.DarkGreen:
                        result = eMetroTileColor.DarkGreen;
                        break;
                    case KnownColor.OrangeRed:
                        result = eMetroTileColor.RedOrange;
                        break;
                    case KnownColor.Azure:
                        result = eMetroTileColor.Azure;
                        break;
                }
            }
            else if (color == ThreatModelManager.StandardColor)
            {
                result = eMetroTileColor.Blueish;
            }
            else if (color == ThreatModelManager.ThreatsColor)
            {
                result = eMetroTileColor.RedOrange;
            }

            return result;
        }

        private string GetTooltipFooter(PanelItem panelItem)
        {
            string result = null;

            if (panelItem.TooltipAction != TooltipAction.None)
            {
                if (panelItem.TooltipAction == TooltipAction.Edit)
                {
                    result = cEdit;
                } 
                else if (panelItem.TooltipAction == TooltipAction.Remove)
                {
                    result = cRemove;
                }
                else
                {
                    result = $"{cEdit} - {cRemove}";
                }
            }

            return result;
        }

        private string Trim(string text, Font font, int maxSizeInPixels)
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
            PanelItemListFormTrigger.PanelStatusUpdated -= PanelItemListFormTrigger_PanelStatusUpdated;
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
            _currentPanelItem = null;

            if (sender is MetroTileItem item && item.Tag is PanelItem panelItem)
            {
                switch (panelItem.ActionOnClick)
                {
                    case ClickAction.ShowObject:
                        PanelItemClicked?.Invoke(panelItem.Tag);
                        break;
                    case ClickAction.ShowTooltip:
                        _currentPanelItem = panelItem;
                        _superTooltip.ShowTooltip(item);
                        break;
                    case ClickAction.CallMethod:
                        panelItem.ExecuteAction(_referenceObject);
                        break;
                }
            }
        }

        private void ThreatEventListForm_Deactivate(object sender, EventArgs e)
        {
            if (PanelItemListFormTrigger.CurrentStatus == PanelsStatus.Normal)
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
                menuItem.Owner.Tag is PanelItem item)
            {
                Close();
                action.Execute(item.Tag);
            }
        }

        private void _superTooltip_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            if (_currentPanelItem != null)
            {
                if (string.CompareOrdinal(e.HRef, "Edit") == 0)
                {
                    _currentPanelItem.EditAction(_referenceObject);
                }
                else if (string.CompareOrdinal(e.HRef, "Remove") == 0)
                {
                    if (MessageBox.Show(Application.OpenForms.OfType<IWin32Window>().FirstOrDefault(),
                        $"Do you confirm removal of item '{_currentPanelItem.Name}'?",
                        "Item removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        Close();
                        _currentPanelItem.RemoveAction(_referenceObject);
                    }
                }
            }
        }
    }
}
