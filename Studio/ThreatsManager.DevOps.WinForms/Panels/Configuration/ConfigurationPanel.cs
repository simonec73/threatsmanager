using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Initializers;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.Configuration
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
            _configuration = new ExtensionConfigurationManager(model, (new AutoConnector()).GetExtensionId());

            _scheduledRefresh.Checked = _configuration.ScheduledRefreshes;
            _interval.Value = _configuration.RefreshInterval;
            switch (_configuration.NotificationStrategy)
            {
                case NotificationType.SuccessOnly:
                    _raiseSomeEvents.Checked = true;
                    break;
                case NotificationType.Full:
                    _raiseAllEvents.Checked = true;
                    break;
                default:
                    _raiseNoEvents.Checked = true;
                    break;
            }
        }

        public void Apply()
        {
            _configuration.Apply();
        }

        public string Label => "DevOps";
        public Bitmap Icon => Properties.Resources.devops_white;
        public Bitmap SelectedIcon => Properties.Resources.devops_black;

        public IEnumerable<ConfigurationData> Configuration => _configuration?.Data;

        private void _scheduledRefresh_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.ScheduledRefreshes = _interval.Enabled = 
                _raiseAllEvents.Enabled = _raiseSomeEvents.Enabled = _raiseNoEvents.Enabled = 
                    _scheduledRefresh.Checked;
        }

        private void _interval_ValueChanged(object sender, EventArgs e)
        {
            _configuration.RefreshInterval = _interval.Value;
        }

        private void _raiseNoEvents_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.NotificationStrategy = NotificationType.None;
        }

        private void _raiseSomeEvents_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.NotificationStrategy = NotificationType.SuccessOnly;
        }

        private void _raiseAllEvents_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.NotificationStrategy = NotificationType.Full;
        }
    }
}
