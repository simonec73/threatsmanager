using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("2A01104E-E774-4AB9-8700-27F015CCADDE", "Threat Event Counter Status Info Provider", 25, ExecutionMode.Simplified)]
    public class ThreatEventCounter : IStatusInfoProviderExtension
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
        }

        public string CurrentStatus =>
            $"Threat Events: {_model.TotalThreatEvents}";
        
        public string Description => "Counter of the Threat Events defined in the Threat Model.";

        private void Update([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Threat Event Counter";
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
