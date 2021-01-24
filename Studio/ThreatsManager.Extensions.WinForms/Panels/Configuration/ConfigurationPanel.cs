using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Configuration
{
    public partial class ConfigurationPanel : UserControl, IConfigurationPanel<Form>
    {
        private ExtensionConfigurationManager _configuration;

        public ConfigurationPanel()
        {
            InitializeComponent();
        }

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void Initialize([NotNull] IThreatModel model)
        {
            _configuration = new ExtensionConfigurationManager(model, (new ConfigurationPanelFactory()).GetExtensionId());

            _enableEffortSupport.Checked = _configuration.EnableEffort;
            _normalizationReference.Value = _configuration.NormalizationReference;
        }

        public void Apply()
        {
            _configuration.Apply();
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
