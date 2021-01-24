using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps
{
    class ExtensionConfigurationManager
    {
        private readonly IThreatModel _model;
        private readonly string _extensionId;
        private readonly ExtensionConfiguration _configuration;
        private bool _dirty;
        
        public ExtensionConfigurationManager([NotNull] IThreatModel model, [Required] string extensionId)
        {
            _model = model;
            _extensionId = extensionId;
            _configuration = model.GetExtensionConfiguration(extensionId);
        }

        public void Apply()
        {
            if (_dirty)
                _model.SetExtensionConfiguration(_extensionId, _configuration);
            if (ScheduledRefreshes)
                DevOpsManager.StartAutomaticUpdater(_model, RefreshInterval, NotificationStrategy);
            else
                DevOpsManager.StopAutomaticUpdater();

            if (EnableBacklogReview)
            {

            }
        }

        public IEnumerable<ConfigurationData> Data => _configuration?.Data;

        public bool EnableBacklogReview
        {
            get => _configuration?.LocalGet<bool>("backlogReview") ?? false;

            set
            {
                _configuration?.LocalSet<bool>("backlogReview", value);
                _dirty = true;
            }
        }

        public bool ScheduledRefreshes
        {
            get => _configuration?.GlobalGet<bool>("scheduledRefreshes") ?? false;

            set
            {
                _configuration?.GlobalSet<bool>("scheduledRefreshes", value);
                _dirty = true;
            }
        }

        public int RefreshInterval
        {
            get => _configuration?.GlobalGet<int>("refreshInterval", 10) ?? 0;

            set
            {
                _configuration?.GlobalSet<int>("refreshInterval", value);
                _dirty = true;
            }
        }

        public NotificationType NotificationStrategy
        {
            get => Enum.TryParse<NotificationType>(_configuration?.GlobalGet<string>("notification", 
                NotificationType.None.ToString()), out var result) ? result : NotificationType.None;

            set
            {
                _configuration?.GlobalSet<string>("notification", value.ToString());
                _dirty = true;
            }
        }
    }
}
