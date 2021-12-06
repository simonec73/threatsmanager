using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class SelectStrengthDialog : Form
    {
        private IStrength _strength;

        public SelectStrengthDialog()
        {
            InitializeComponent();
        }

        public SelectStrengthDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigationName.Text = mitigation.Name;

            if (mitigation.Model is IThreatModel model)
            {
                var strengths = model.Strengths?.Where(x => x.Visible).ToArray();
                if (strengths?.Any() ?? false)
                    _strengths.Items.AddRange(strengths);
            }
        }

        public IStrength Strength => _strength;

        private void _strengths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_strengths.SelectedItem is IStrength strength)
            {
                _strength = strength;
                _ok.Enabled = true;
            }
            else
            {
                _ok.Enabled = false;
            }
        }
    }
}
