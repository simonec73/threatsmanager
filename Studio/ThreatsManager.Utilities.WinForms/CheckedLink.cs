using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreatsManager.Utilities.WinForms
{
    public partial class CheckedLink : UserControl
    {
        public CheckedLink()
        {
            InitializeComponent();
        }

        public override string Text { get => _link.Text; set => _link.Text = value; }

        public string Link
        {
            get
            {
                return _link.Tag.ToString();
            }

            set
            {
                _link.Tag = value;

                _tooltip.SetToolTip(_checkBox, value);
                _tooltip.SetToolTip(_link, value);
            }
        }

        public bool Checked
        {
            get
            {
                return _checkBox.Checked;
            }

            set
            {
                _checkBox.Checked = value;
            }
        }

        private void _link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_link.Tag.ToString()))
                System.Diagnostics.Process.Start(_link.Tag.ToString());
        }
    }
}
