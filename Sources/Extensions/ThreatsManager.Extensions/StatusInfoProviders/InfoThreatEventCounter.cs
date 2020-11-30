using System;
using System.ComponentModel;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("5D7BB0EC-CEA0-4A37-86E9-06B690FFC333", 
        "Info Severity Threat Event Counter Status Info Provider", 44, ExecutionMode.Simplified)]
    public class InfoThreatEventCounter : IStatusInfoProviderExtension
    {
        private IThreatModel _model;

        public event Action<string, string> UpdateInfo;

        public void Initialize([NotNull] IThreatModel model)
        {
            if (_model != null)
            {
                Dispose();
            }

            _model = model;
            _model.ThreatEventAdded += Update;
            _model.ThreatEventAddedToEntity += Update;
            _model.ThreatEventAddedToDataFlow += Update;
            _model.ThreatEventRemoved += Update;
            _model.ThreatEventRemovedFromEntity += Update;
            _model.ThreatEventRemovedFromDataFlow += Update;

            var modelTe = _model.ThreatEvents?.ToArray();
            if (modelTe?.Any() ?? false)
            {
                foreach (var te1 in modelTe)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    ((INotifyPropertyChanged)te1).PropertyChanged += OnPropertyChanged;
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEvent)
            {
                if (e.PropertyName == "Severity") UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
            }
        }

        public string CurrentStatus =>
            $"Info: {_model.CountThreatEvents((int)DefaultSeverity.Info)}";

        public string Description => "Counter of the Threat Events which have been categorized as Info Severity.";

        private void Update([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)threatEvent).PropertyChanged += OnPropertyChanged;
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Info Severity Threat Event Counter";
        }
 
        public void Dispose()
        {
            _model.ThreatEventAdded -= Update;
            _model.ThreatEventAddedToEntity -= Update;
            _model.ThreatEventAddedToDataFlow -= Update;
            _model.ThreatEventRemoved -= Update;
            _model.ThreatEventRemovedFromEntity -= Update;
            _model.ThreatEventRemovedFromDataFlow -= Update;
            _model = null;
        }
    }
}
