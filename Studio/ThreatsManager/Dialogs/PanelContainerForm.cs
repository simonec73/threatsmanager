using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Dialogs
{
    public partial class PanelContainerForm : Form
    {
        private ICustomRibbonExtension _extension;
        private readonly List<RibbonBar> _ribbonBars = new List<RibbonBar>();

        public PanelContainerForm()
        {
            InitializeComponent();
        }

        public static event Action<PanelContainerForm> InstanceCreated;
        public static event Action<PanelContainerForm> InstanceClosed;
        public static event Action<PanelContainerForm> InstanceTextChanged;

        public string FormId { get; set; }

        public IPanel Panel
        {
            get => Controls.OfType<IPanel>().FirstOrDefault();
            set
            {
                if (value is Control control)
                {
                    SuspendLayout();
                    control.SuspendLayout();
                    control.Visible = false;
                    control.Dock = DockStyle.Fill;
                    Controls.Add(control);
                    control.ResumeLayout();
                    ResumeLayout();
                }
            }
        }

        public void InitializeRibbon([NotNull] ICustomRibbonExtension extension, int mergeIndex)
        {
            _extension = extension;
            _ribbonMerge.MergeRibbonTabItemIndex = mergeIndex;
            extension.ChangeCustomActionStatus += PerformActionStatusChange;

            var bars = extension.CommandBars?.ToArray();

            if (bars?.Any() ?? false)
            {
                _ribbonMerge.RibbonTabText = extension.TabLabel;

                try
                {
                    _ribbonMerge.SuspendLayout();

                    int left = 0;

                    foreach (var bar in bars)
                    {
                        if (bar.Commands?.Any() ?? false)
                        {
                            var ribbonBar = bar.Label.CreateBar();
                            ribbonBar.Left = left;
                            foreach (var command in bar.Commands)
                            {
                                var button = ribbonBar.CreateButton(command, _superTooltip);
                                button.Click += ButtonOnClick;
                                ribbonBar.Items.Add(button);
                            }

                            _ribbonBars.Add(ribbonBar);
                            _ribbonMerge.Controls.Add(ribbonBar);
                            ribbonBar.RecalcLayout();
                            left += 100;
                        }
                    }
                }
                finally
                {
                    _ribbonMerge.ResumeLayout();
                }
            }
        }
        
        public void HideRibbon()
        {
            _ribbonMerge.AllowMerge = false;
        }

        private void PerformActionStatusChange([Required] string actionName, bool status)
        {
            foreach (var bar in _ribbonBars)
            {
                var buttons = bar.Items.OfType<ButtonItem>();
                foreach (var button in buttons)
                {
                    if (button.Tag is IActionDefinition action)
                    {
                        if (string.CompareOrdinal(actionName, action.Name) == 0)
                        {
                            button.Enabled = status;
                            break;
                        }
                    }
                }
            }
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (sender is ButtonItem buttonItem)
            {
                if (buttonItem.Tag is IActionDefinition action)
                {
                    _extension?.ExecuteCustomAction(action);
                }
            }
        }

        private void PanelContainerForm_TextChanged(object sender, EventArgs e)
        {
            InstanceTextChanged?.Invoke(this);
        }

        private void PanelContainerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            InstanceClosed?.Invoke(this);
        }

        private void PanelContainerForm_Load(object sender, EventArgs e)
        {
            InstanceCreated?.Invoke(this);
        }
    }
}
