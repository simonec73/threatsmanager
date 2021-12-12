using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class SelectSeverityDialog : Form
    {
        private ISeverity _severity;

        public SelectSeverityDialog()
        {
            InitializeComponent();

        }

        public SelectSeverityDialog([NotNull] IThreatType threatType) : this()
        {
            _threatTypeName.Text = threatType.Name;

            if (threatType.Model is IThreatModel model)
            {
                var severities = model.Severities?.Where(x => x.Visible).ToArray();
                if (severities?.Any() ?? false)
                    _severities.Items.AddRange(severities);
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
