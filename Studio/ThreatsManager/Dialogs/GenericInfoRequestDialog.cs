using System;
using System.Windows.Forms;

namespace ThreatsManager.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class GenericInfoRequestDialog : Form
    {
        public GenericInfoRequestDialog()
        {
            InitializeComponent();
        }

        public string RequiredInfo
        {
            set
            {
                Text = value;
                _request.Text = $"Please insert the {value}";
                _layoutField.Text = value;
            }
        }

        public string Info => _info.Text;

        private void _info_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = !string.IsNullOrWhiteSpace(_info.Text);
        }
    }
}
