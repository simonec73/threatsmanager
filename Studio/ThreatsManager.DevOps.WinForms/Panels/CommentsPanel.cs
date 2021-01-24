using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Panels
{
    public partial class CommentsPanel : UserControl
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public CommentsPanel()
        {
            InitializeComponent();

            Clear();

            _browser.Navigating += OnNavigating;
        }

        private void OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url?.Scheme.StartsWith("http") ?? false)
            {
                e.Cancel = true;

                try
                {
                    if (Regex.IsMatch(e.Url.AbsoluteUri,
                        @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]",
                        RegexOptions.IgnoreCase))
                    {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                        Process.Start(e.Url.AbsoluteUri);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    }
                }
                catch
                {
                    // Ignore the error because the link is simply not trusted.
                }
            }
        }

        public void AddComment([Required] string author, [Required] string comment, DateTime timestamp, bool me)
        {
            _builder.AppendLine($"<div class=\"container{(me ? " me" : "")}\">");
            _builder.AppendLine(comment);
            _builder.AppendLine($"<span class=\"footer\">{timestamp.ToShortDateString()} {author}");
            _builder.AppendLine("</div>");
        }

        public void Clear()
        {
            _builder.Clear();
            _builder.AppendLine("<!DOCTYPE html><html><head><style>");
            _builder.AppendLine(Properties.Resources.CommentsStyleSheet);
            _builder.AppendLine("</style></head><body>");
        }

        public void RefreshNodes()
        {
            _browser.DocumentText = $"{_builder.ToString()}\n</body></html>";
        }
    }
}
