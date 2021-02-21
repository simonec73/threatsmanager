using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using Microsoft.Web.WebView2.Core;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Help;

namespace ThreatsManager.Extensions.Panels.Troubleshooting
{
    public partial class TroubleshootingPanel : UserControl, IStaticPanel<Form>, ICustomRibbonExtension, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public TroubleshootingPanel()
        {
            InitializeComponent();
            InitializeAsync();
        }
        
        private async void InitializeAsync()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string cacheFolder = Path.Combine(localAppData, "WindowsFormsWebView2");
            var environment = await CoreWebView2Environment.CreateAsync(null, cacheFolder);

            await _browser.EnsureCoreWebView2Async(environment);
        }

        #region Implementation of interface IStaticPanel.

        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetAction(string parameter)
        {
        }
        #endregion
     
        private void TroubleshootingPanel_Load(object sender, EventArgs e)
        {
            try
            {
                var pages = TroubleshootingManager.Instance.Pages?.ToArray();

                _tree.Nodes.Clear();

                if (pages?.Any() ?? false)
                {
                    var backStyle = new DevComponents.DotNetBar.ElementStyle()
                    {
                        Border = DevComponents.DotNetBar.eStyleBorderType.Solid,
                        BorderColor = ThreatModelManager.StandardColor,
                        BackColor = Color.White,
                        TextColor = Color.Black
                    };

                    foreach (var page in pages)
                    {
                        var node = new Node(page.Name)
                        {
                            Image = Properties.Resources.lifebelt,
                            Tooltip = page.Description,
                            StyleSelected = backStyle,
                            Tag = page
                        };
                        node.NodeClick += NodeClick;
                        _tree.Nodes.Add(node);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void NodeClick(object sender, EventArgs e)
        {
            if (sender is Node node && node.Tag is Page lesson)
            {
                _browser.CoreWebView2.Navigate(lesson.Url);
            }
        }
    }
}
