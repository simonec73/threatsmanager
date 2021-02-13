using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ThreatsManager.Dialogs
{
    public partial class NewVersionDialog : Form
    { 
        #if MICROSOFT_EDITION
        private const string UrlDownload = "https://aka.ms/tmplatform";
        #else
        private const string UrlDownload = "https://threatsmanager.com/downloads/";
        #endif

        public NewVersionDialog()
        {
            InitializeComponent();
        }

        public void Initialize(Version version, string highlights)
        {
            _title.Text = $"Version {version.ToString()} is available!";

            string appendix = null;
            if (!string.IsNullOrWhiteSpace(highlights))
                appendix = $"<br/><br/>Some highlights of the new version:<br/>{highlights}";
            _description.Text =
                $"Please go to <a href=\"{UrlDownload}\">{UrlDownload}</a> to download it.{appendix}";
        }

        private void _description_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.HRef, 
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", 
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(e.HRef);
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
