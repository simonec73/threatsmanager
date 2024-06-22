using System.Windows.Forms;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    public partial class UrlEditorDialog : Form
    {
        public UrlEditorDialog()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => _label.Text;
            set
            {
                _label.Text = value;
                Text = "Edit the URL";
            }
        }

        public string Url
        {
            get => _url.Text;
            set
            {
                _url.Text = value;
                Text = "Edit the URL";
            }
        }
    }
}
