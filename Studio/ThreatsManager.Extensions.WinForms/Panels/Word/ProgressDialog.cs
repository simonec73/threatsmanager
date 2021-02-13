using System.Windows.Forms;

namespace ThreatsManager.Extensions.Panels.Word
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public int Value
        {
            get => _progress.Value;
            set
            {
                _progress.Value = value;
                _progress.Refresh();
                Application.DoEvents();
            }
        }
    }
}
