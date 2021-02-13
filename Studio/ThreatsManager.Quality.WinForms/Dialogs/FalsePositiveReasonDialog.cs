using System.Windows.Forms;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Quality.Dialogs
{
    public partial class FalsePositiveReasonDialog : Form
    {
        public FalsePositiveReasonDialog()
        {
            InitializeComponent();
        }

        public FalsePositiveReasonDialog([Required] string analyzerName, [NotNull] object finding) : this()
        {
            _analyzerName.Text = analyzerName;
            _finding.Text = finding.ToString();
        }

        public string Reason => _reason.Text;

        private void _reason_TextChanged(object sender, System.EventArgs e)
        {
            _ok.Enabled = !string.IsNullOrWhiteSpace(_reason.Text);
        }
    }
}
