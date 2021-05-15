using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreatsManager.Extensions.Dialogs
{
    public enum DiagramIssue
    {
        Ok,
        TooSmall,
        TooLarge
    }

    public partial class FixDiagram : Form
    {
        public FixDiagram()
        {
            InitializeComponent();
        }

        public DiagramIssue Issue { get; private set; }

        private void _tooSmall_CheckedChanged(object sender, EventArgs e)
        {
            Issue = DiagramIssue.TooSmall;
            _ok.Enabled = true;
        }

        private void _tooLarge_CheckedChanged(object sender, EventArgs e)
        {
            Issue = DiagramIssue.TooLarge;
            _ok.Enabled = true;
        }
    }
}
