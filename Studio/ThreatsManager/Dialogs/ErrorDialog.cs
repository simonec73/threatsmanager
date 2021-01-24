using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ThreatsManager.Dialogs
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => _title.Text;
            set => _title.Text = value;
        }

        public string Description
        {
            get => _description.Text;
            set => _description.Text = value;
        }

        private void _description_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.LinkText, 
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", 
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(e.LinkText);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                }
            }
            catch
            {
                // Ignore the error because the link is simply not trusted.
            }
        }
    }
}
