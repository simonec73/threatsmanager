using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager
{
    static class RibbonExtensions
    {
        public static RibbonBar CreateBar([Required] this string label)
        {
            var result = new RibbonBar
            {
                Text = label,
                AutoOverflowEnabled = true,
                ContainerControlProcessDialogKey = true,
                Dock = System.Windows.Forms.DockStyle.Left,
                LicenseKey = "PUT_YOUR_LICENSE_HERE",
                Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled,
                OverflowButtonImage = Properties.Resources.cabinet
            };
            result.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            result.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;

            return result;
        }

        public static ButtonItem CreateButton([NotNull] this RibbonBar bar, [NotNull] IActionDefinition actionDefinition, [NotNull] SuperTooltip tooltip)
        {
            ButtonItem result = null;

            string key = $"{actionDefinition.Id:N}_{actionDefinition.Name}";

            if (!bar.Items.Contains(key))
            {
                result = new ButtonItem
                {
                    ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText,
                    ImagePosition = DevComponents.DotNetBar.eImagePosition.Top,
                    SubItemsExpandWidth = 14,
                    Name = key,
                    Text = actionDefinition.Label,
                    Image = actionDefinition.Icon,
                    ImageSmall = actionDefinition.SmallIcon,
                    ImageFixedSize = new Size(32, 32),
                    Enabled = actionDefinition.Enabled,
                    Tag = actionDefinition
                };

                if (actionDefinition.Shortcut != Shortcut.None)
                {
                    result.Shortcuts.Add((eShortcut)actionDefinition.Shortcut);
                }

                if (actionDefinition.Shortcut != Shortcut.None || !string.IsNullOrWhiteSpace(actionDefinition.Tooltip))
                {
                    string header;
                    if (actionDefinition.Shortcut != Shortcut.None)
                        header = $"{actionDefinition.Label} ({GetShortcutString(actionDefinition.Shortcut)})";
                    else
                        header = $"{actionDefinition.Label}";
                    tooltip.SetSuperTooltip(result, new SuperTooltipInfo(header, null, actionDefinition.Tooltip, null, null, eTooltipColor.Gray));
                }
            }

            return result;
        }

        public static ButtonItem CreateButton([NotNull] this ButtonItem parent, [NotNull] IActionDefinition actionDefinition, [NotNull] SuperTooltip tooltip)
        {
            ButtonItem result = null;

            string key = $"{parent.Name}_{actionDefinition.Name}";

            if (!parent.SubItems.Contains(key))
            {
                result = new ButtonItem
                {
                    ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText,
                    ImagePosition = DevComponents.DotNetBar.eImagePosition.Left,
                    Name = key,
                    Text = actionDefinition.Label,
                    Image = actionDefinition.Icon,
                    ImageSmall = actionDefinition.SmallIcon,
                    ImageFixedSize = new Size(24, 24),
                    Enabled = actionDefinition.Enabled,
                    Tag = actionDefinition
                };
 
                if (actionDefinition.Shortcut != Shortcut.None)
                {
                    result.Shortcuts.Add((eShortcut)actionDefinition.Shortcut);
                }

                if (actionDefinition.Shortcut != Shortcut.None || !string.IsNullOrWhiteSpace(actionDefinition.Tooltip))
                {
                    string header;
                    if (actionDefinition.Shortcut != Shortcut.None)
                        header = $"{actionDefinition.Label} ({GetShortcutString(actionDefinition.Shortcut)})";
                    else
                        header = $"{actionDefinition.Label}";
                    tooltip.SetSuperTooltip(result, new SuperTooltipInfo(header, null, actionDefinition.Tooltip, null, null, eTooltipColor.Gray));
                }
            }

            return result;
        }

        private static string GetShortcutString(Shortcut shortcut)
        {
            StringBuilder result = new StringBuilder();

            try {
                var regex = Regex.Match(shortcut.ToString(), @"(Ctrl)?(Shift)?(Alt)?(\w{1,3})", RegexOptions.IgnoreCase);
                for (int i = 1; i < regex.Groups.Count; i++)
                {
                    if (regex.Groups[i].Success)
                    {
                        result.Append(regex.Groups[i].Value);

                        if (i < regex.Groups.Count - 1)
                            result.Append("+");
                    }
                }
            } catch (ArgumentException) {
                // Syntax error in the regular expression
            }

            return result.ToString();
        }
    }
}
