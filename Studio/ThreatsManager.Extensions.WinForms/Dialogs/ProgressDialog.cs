using System.Windows.Forms;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => _label.Text;
            set
            {
                _label.Text = value;
            }
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
