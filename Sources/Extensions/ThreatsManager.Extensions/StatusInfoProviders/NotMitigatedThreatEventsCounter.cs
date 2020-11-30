using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("3E723A65-69EB-489B-9538-D6F664CAC892", 
        "Not Mitigated Threat Events Counter Status Info Provider", 65, ExecutionMode.Simplified)]
    public class NotMitigatedThreatEventsCounter : IStatusInfoProviderExtension
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
            _model.ThreatEventAdded += ThreatEventAdded;
            _model.ThreatEventAddedToEntity += ThreatEventAdded;
            _model.ThreatEventAddedToDataFlow += ThreatEventAdded;
            _model.ThreatEventRemoved += ThreatEventRemoved;
            _model.ThreatEventRemovedFromEntity += ThreatEventRemoved;
            _model.ThreatEventRemovedFromDataFlow += ThreatEventRemoved;
        }

        public string CurrentStatus =>
            $"Not mitigated Threat Events: {_model.NotMitigatedThreatEvents}";
        
        public string Description => "Threat Events that have not been mitigated.";

        private void ThreatEventAdded([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            threatEvent.ThreatEventMitigationAdded += Update;
            threatEvent.ThreatEventMitigationRemoved += Update;
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        private void ThreatEventRemoved([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            threatEvent.ThreatEventMitigationAdded -= Update;
            threatEvent.ThreatEventMitigationRemoved -= Update;
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        private void Update(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Not mitigated Threat Events";
        }
 
        public void Dispose()
        {
            _model.ThreatEventAdded -= ThreatEventAdded;
            _model.ThreatEventAddedToEntity -= ThreatEventAdded;
            _model.ThreatEventAddedToDataFlow -= ThreatEventAdded;
            _model.ThreatEventRemoved -= ThreatEventRemoved;
            _model.ThreatEventRemovedFromEntity -= ThreatEventRemoved;
            _model.ThreatEventRemovedFromDataFlow -= ThreatEventRemoved;
            _model = null;
        }
    }
}
