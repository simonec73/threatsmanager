using System.Security;
using System.Windows.Forms;

namespace ThreatsManager.Dialogs
{
    public partial class PasswordDialog : Form
    {
        public PasswordDialog()
        {
            InitializeComponent();
        }

        public bool VerificationRequired
        {
            get { return true; }
            set
            {

            }
        }

        public SecureString Password => _password.SecureString;
    }
}
