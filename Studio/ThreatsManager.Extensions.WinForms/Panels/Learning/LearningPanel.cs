using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Extensions.Panels.Learning
{
    public partial class LearningPanel : UserControl, IStaticPanel<Form>, ICustomRibbonExtension, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public LearningPanel()
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
    }
}
