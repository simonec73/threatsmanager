using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Configuration
{
    public partial class ConfigurationPanel : UserControl, IConfigurationPanel<Form>, IDesktopAlertAwareExtension
    {
        private ExtensionConfigurationManager _configuration;

        public ConfigurationPanel()
        {
            InitializeComponent();
        }

        private readonly Guid _id = Guid.NewGuid();

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public IIdentity ReferenceObject => null;

        public void Initialize([NotNull] IThreatModel model)
        {
            _configuration = new ExtensionConfigurationManager(model);

            _enableEffortSupport.Checked = _configuration.EnableEffort;
            _normalizationReference.Value = _configuration.NormalizationReference;
        }

        public void Apply()
        {
            try
            {
                _configuration.Apply();
            }
            catch (Exception ex)
            {
                ShowWarning?.Invoke(ex.Message);
            }
        }

        public string Label => "Main Extensions";
        public Bitmap Icon => Properties.Resources.library_white;
        public Bitmap SelectedIcon => Properties.Resources.library_black;

        public IEnumerable<ConfigurationData> Configuration => _configuration?.Data;

        private void _enableEffortSupport_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.EnableEffort = _enableEffortSupport.Checked;
        }

        private void _normalizationReference_ValueChanged(object sender, EventArgs e)
        {
            _configuration.NormalizationReference = _normalizationReference.Value;
        }
    }
}
