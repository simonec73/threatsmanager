using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ThreatsManager.Dialogs
{
    public partial class FeedbackDialog : Form
    {
        public FeedbackDialog()
        {
            InitializeComponent();
        }

        public bool Smile => _smile.Checked;
        public string Comments => _comments.Text;
        public string Email => _email.Text;
        public bool Authorization => _publishAuthorization.Checked;

        public string Message =>
            $"{(Smile ? "Smile" : "Frown")}<br/>Email: {Email}<br/>Publishing {(Authorization ? "is" : "is NOT")} authorized<br/>{Comments}";
    }
}
