using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class SelectStatusDialog : Form
    {
        private MitigationStatus _mitigationStatus;

        public SelectStatusDialog()
        {
            InitializeComponent();

            _status.Items.AddRange(EnumExtensions.GetEnumLabels<MitigationStatus>().ToArray());
        }

        public SelectStatusDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigationName.Text = mitigation.Name;
        }

        public MitigationStatus Status => _mitigationStatus;

        private void _status_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_status.SelectedItem != null)
            {
                _mitigationStatus = ((string) _status.SelectedItem).GetEnumValue<MitigationStatus>();
                _ok.Enabled = true;
            }
            else
            {
                _ok.Enabled = false;
            }
        }
    }
}
