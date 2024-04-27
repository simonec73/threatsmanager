using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Panels.Configuration
{
    public partial class ConfigurationPanel : UserControl, IConfigurationPanel<Form>, IDesktopAlertAwareExtension
    {
        private QualityConfigurationManager _configuration;

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
            _configuration = QualityConfigurationManager.GetInstance(model);

            _enableCalculatedSeveritySupport.Checked = _configuration.EnableCalculatedSeverity;
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

        public string Label => "Quality";
        public Bitmap Icon => Properties.Resources.wax_seal_white;
        public Bitmap SelectedIcon => Properties.Resources.wax_seal_black;

        public IEnumerable<ConfigurationData> Configuration => _configuration?.Data;

        private void _enableCalculatedSeveritySupport_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.EnableCalculatedSeverity = _enableCalculatedSeveritySupport.Checked;
        }
    }
}
