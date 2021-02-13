using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;

namespace ThreatsManager.DevOps.Panels
{
    public class KanbanHyperlink : GoListGroup 
    {
        class HyperlinkControl : LinkLabel, IGoControlObject 
        {
            private GoControl _goControl = null;
            private string _hyperlink;

            public GoControl GoControl 
            {
                get => _goControl;

                set 
                {
                    GoControl old = _goControl;
                    if (old != value) 
                    {
                        _goControl = value;
                        if (value?.Parent is KanbanHyperlink hyperlink) 
                        {
                            Text = hyperlink.HyperlinkLabel;
                            _hyperlink = hyperlink.Hyperlink;
                            LinkClicked += OpenLink;
                        }
                    }
                }
            }

            private void OpenLink(object sender, LinkLabelLinkClickedEventArgs e)
            {
                try
                {
                    if (Regex.IsMatch(_hyperlink,
                        @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]",
                        RegexOptions.IgnoreCase))
                    {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                        this.LinkVisited = true;
                        Process.Start(_hyperlink);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    }
                }
                catch
                {
                    // Ignore the error because the link is simply not trusted.
                }
            }

            public GoView GoView { get; set; }
        }

        public KanbanHyperlink() 
        {
            Selectable = false;
            Deletable = false;
            Orientation =  System.Windows.Forms.Orientation.Horizontal;
            TopLeftMargin = new SizeF(0, 0);
            BottomRightMargin = new SizeF(0, 0);

            GoImage img = new GoImage();
            img.Selectable = false;
            img.AutoRescales = false;
            img.AutoResizes = false;
            img.Size = new SizeF(16 * Dpi.Factor.Width, 16 * Dpi.Factor.Height);
            Add(img);
            GoText t = new GoText();
            t.FontSize = 9;
            t.AutoResizes = false;
            t.Selectable = false;
            t.Editable = false;
            t.Multiline = true;
            t.Wrapping = true;
            t.WrappingWidth = 150;
            Add(t);

            GoControl host = new GoControl();
            host.Selectable = false;
            host.ControlType = typeof(HyperlinkControl);
            host.Height = 16 * Dpi.Factor.Height;
            Add(host);
        }

        public string HyperlinkLabel { get; set; }
        public string Hyperlink { get; set; }

        // parts
        public GoImage Image => (GoImage)this[0];

        public GoText Label => (GoText)this[1];

        public GoControl Value => (GoControl)this[2];
    }
}