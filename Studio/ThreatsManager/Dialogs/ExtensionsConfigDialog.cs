using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Layout;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Controls;
using ThreatsManager.Engine;

namespace ThreatsManager.Dialogs
{
    public partial class ExtensionsConfigDialog : Form
    {
        private int _total;

        public ExtensionsConfigDialog()
        {
            InitializeComponent();
        }

        [Background]
        private void ExtensionsConfigDialog_Load(object sender, EventArgs e)
        {
            var configuration = Manager.Instance.Configuration;
            LoadExtensions(configuration.Extensions);
        }

        [Dispatched]
        private void LoadExtensions([NotNull] IEnumerable<string> extensions)
        {
            SuspendLayout();

            try
            {
                if (extensions != null)
                {
                    var groups = new Dictionary<string, List<ExtensionConfig>>();

                    foreach (var id in extensions)
                    {
                        ExtensionConfig item = new ExtensionConfig();
                        item.ConfigureExtension(id);

                        if (!groups.TryGetValue(item.AssemblyTitle, out var list))
                        {
                            list = new List<ExtensionConfig>();
                            groups.Add(item.AssemblyTitle, list);
                        }
                        list.Add(item);
                    }

                    var sorted = groups.Select(x => x.Key).OrderBy(x => x).ToArray();
                    if (sorted.Any())
                    {
                        foreach (var item in sorted)
                        {
                            if (groups.TryGetValue(item, out var group))
                            {
                                if (group?.Any() ?? false)
                                {
                                    var layout = AddSide(item, group.Count);
                                    var sortedGroup = group.OrderBy(x => x.ExtensionType)
                                        .ThenBy(x => x.ExtensionName).ToArray();
                                    foreach (var curr in sortedGroup)
                                        AddItem(layout, curr);
                                }
                            }
                        }

                        if (_side.Items[0] is SideNavItem sideNavItem)
                            sideNavItem.Checked = true;
                    }

                    _total = extensions.Count();
                    this.Text = $"{this.Text} ({extensions.Count()})";
                }
            }
            finally
            {
                ResumeLayout();
                Refresh();
            }
        }

        private LayoutControl AddSide([Required] string assembly, int count)
        {
            var layout = new LayoutControl
            {
                Dock = DockStyle.Fill,
                ForeColor = System.Drawing.Color.Black,
                Location = new System.Drawing.Point(0, 0),
                Margin = new System.Windows.Forms.Padding(6, 6, 6, 6),
                Name = $"{assembly}Layout"
            };

            var combo = new ComboBox()
                {
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
            if (count <= 50)
            {
                combo.Items.Add("<All>");
                combo.SelectedIndex = 0;
            }
            combo.SelectedIndexChanged += ChangedExtensionType;
            layout.Controls.Add(combo);
            layout.RootGroup.Items.Add(new LayoutControlItem()
            {
                Control = combo,
                Padding = new System.Windows.Forms.Padding(8, 8, 8, 8),
                TextVisible = true,
                Text = "Extension Type",
                Height = combo.Height,
                HeightType = eLayoutSizeType.Absolute,
                Width = 85,
                WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent
            });

            var label = new Label()
            {
                Text = $"Count: {count}",
                TextAlign = ContentAlignment.MiddleRight,
                Width = 10
            };
            layout.Controls.Add(label);
            layout.RootGroup.Items.Add(new LayoutControlItem()
            {
                Control = label, 
                Padding = new System.Windows.Forms.Padding(8, 8, 8, 8),
                TextVisible = false,
                Height = 46,
                HeightType = eLayoutSizeType.Absolute,
                Width = 15,
                WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent
            });

            var panel = new SideNavPanel {Dock = DockStyle.Fill, Name = $"{assembly}Panel", Location = new Point(10,10)};
            panel.Controls.Add(layout);

            _side.Items.Add(new SideNavItem()
            {
                Name = assembly,
                Text = $"{assembly.Replace("ThreatsManager.", "")} ({count})",
                Panel = panel
            });
            _side.Controls.Add(panel);

            return layout;
        }

        private void ChangedExtensionType(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo?.SelectedItem is string extensionType && combo.Parent is LayoutControl layout)
            {
                layout.SuspendLayout();

                var count = 0;

                var layoutControlItems = layout.RootGroup.Items.OfType<LayoutControlItem>().ToArray();
                if (string.CompareOrdinal(extensionType, "<All>") == 0)
                {
                    foreach (var item in layoutControlItems)
                    {
                        if (item.Control is ExtensionConfig extensionConfig)
                        {
                            item.Visible = true;
                            count++;
                        }
                    }
                }
                else
                {
                    foreach (var item in layoutControlItems)
                    {
                        if (item.Control is ExtensionConfig extensionConfig)
                        {
                            item.Visible = string.CompareOrdinal(extensionType, extensionConfig.ExtensionType) == 0;
                            if (item.Visible)
                                count++;
                        }
                    }
                }

                if (layout.RootGroup.Items[1] is LayoutControlItem layoutControlItem &&
                    layoutControlItem.Control is Label label)
                    label.Text = $"Count: {count}";

                layout.ResumeLayout();
            }
        }

        private void AddItem([NotNull] LayoutControl layout, [NotNull] ExtensionConfig item)
        {
            var visible = true;

            var combo = layout.Controls.OfType<ComboBox>().FirstOrDefault();
            if (combo != null)
            {
                if (!combo.Items.Contains(item.ExtensionType))
                {
                    combo.Items.Add(item.ExtensionType);
                    if (combo.Items.Count == 1)
                    {
                        combo.SelectedIndex = 0;
                    }
                }

                var extensionType = combo.SelectedItem as string;
                if (!string.IsNullOrWhiteSpace(extensionType) &&
                    (string.CompareOrdinal("<All>", extensionType) != 0 &&
                        string.CompareOrdinal(item.ExtensionType, extensionType) != 0))
                {
                    visible = false;
                }
            }

            layout.Controls.Add(item);
            layout.RootGroup.Items.Add(new LayoutControlItem()
            {
                Control = item,
                Padding = new System.Windows.Forms.Padding(8, 8, 8, 8),
                TextVisible = false,
                Height = item.Height,
                HeightType = eLayoutSizeType.Absolute,
                Width = 100,
                WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent,
                Visible = visible
            });

            if (layout.RootGroup.Items[1] is LayoutControlItem layoutControlItem &&
                layoutControlItem.Control is Label label)
            {
                var count = layout.RootGroup.Items.OfType<LayoutControlItem>()
                    .Count(x => x.Control is ExtensionConfig && x.Visible);
                label.Text = $"Count: {count}";
            }
        }
    }
}
