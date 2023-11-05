using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class SelectSeverityForItemsDialog : Form
    {
        private ISeverity _severity;

        public SelectSeverityForItemsDialog()
        {
            InitializeComponent();

        }

        public SelectSeverityForItemsDialog(IEnumerable<IThreatModelChild> items, string itemsType = null) : this()
        {
            if (!string.IsNullOrEmpty(itemsType))
            {
                Text = $"Select the Severity to be applied to the selected {itemsType}";
                _countLayoutControlItem.Text = $"# of selected {itemsType}";
            }
            _count.Text = (items?.Count() ?? 0).ToString();

            if (items?.Any() ?? false)
            {
                if (items.FirstOrDefault(x => x != null)?.Model is IThreatModel model)
                {
                    var severities = model.Severities?.Where(x => x.Visible).ToArray();
                    if (severities?.Any() ?? false)
                        _severities.Items.AddRange(severities);
                }
            }
        }

        public ISeverity Severity => _severity;

        private void _severities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_severities.SelectedItem is ISeverity severity)
            {
                _severity = severity;
                _ok.Enabled = true;
            }
            else
            {
                _ok.Enabled = false;
            }
        }
    }
}
