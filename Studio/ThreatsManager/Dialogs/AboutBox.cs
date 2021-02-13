using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ThreatsManager.Engine.ObjectModel;

namespace ThreatsManager.Dialogs
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyTitle;
            labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            var engineVersion = Assembly.GetAssembly(typeof(ThreatModel))?.GetName().Version.ToString(3);
            if (string.IsNullOrWhiteSpace(engineVersion))
                _moreInformation.Text = _moreInformation.Text.Replace("v{ENGINE_VERSION}", string.Empty);
            else
                _moreInformation.Text = _moreInformation.Text.Replace("{ENGINE_VERSION}", engineVersion);

#if MICROSOFT_EDITION
            _thirdParty.Text = $"{Properties.Resources.MicrosoftOnly}\n\n{Properties.Resources.ThirdParties}";
            _microsoftEdition.Visible = true;
#else
            _thirdParty.Text = Properties.Resources.ThirdParties;
#endif
        }

#region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
#endregion

        private void _moreInformation_MarkupLinkClick(object sender, DevComponents.DotNetBar.MarkupLinkClickEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/simonec73/threatsmanager");
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
            Process.Start(sInfo);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
        }

        private void _thirdParty_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.LinkText);
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
            Process.Start(sInfo);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
        }

        private void _links_MarkupLinkClick(object sender, DevComponents.DotNetBar.MarkupLinkClickEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.HRef);
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
            Process.Start(sInfo);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
        }
    }
}
