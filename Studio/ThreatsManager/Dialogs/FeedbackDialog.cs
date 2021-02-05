using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ThreatsManager.Dialogs
{
    public partial class FeedbackDialog : Form
    {
        public FeedbackDialog()
        {
            InitializeComponent();

            if (Dpi.Factor.Width >= 1.5)
            {
                _smile.Image = Properties.Resources.emoticon_smile_big;
                _frown.Image = Properties.Resources.emoticon_frown_big;
            }
        }

        public bool Smile => _smile.Checked;
        public string Comments => _comments.Text;
        public string Email => _email.Text;
        public bool Authorization => _publishAuthorization.Checked;

        public string Message =>
            $"{(Smile ? "Smile" : "Frown")}<br/>Email: {Email}<br/>Publishing {(Authorization ? "is" : "is NOT")} authorized<br/>{Comments}";
    }
}
