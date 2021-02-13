using System.Windows.Forms;

namespace ThreatsManager.MsTmt.Dialogs
{
    public partial class ImportResultDialog : Form
    {
        public ImportResultDialog()
        {
            InitializeComponent();
        }

        public object Properties
        {
            get => _properties.SelectedObject;
            set => _properties.SelectedObject = value;
        }
    }
}
