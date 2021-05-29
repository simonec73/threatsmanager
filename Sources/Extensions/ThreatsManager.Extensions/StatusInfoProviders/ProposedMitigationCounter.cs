using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("815692E0-5582-48A2-9AF2-A89996991B71", "Proposed Mitigation Counter Status Info Provider", 37, ExecutionMode.Simplified)]
    public class ProposedMitigationCounter : IStatusInfoProviderExtension
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
            $"Proposed Mitigations: {(_model.GetThreatEventMitigations()?.Where(x => x.Status == MitigationStatus.Proposed).Count() ?? 0)}";

        public string Description => "Counter of the Threat Event Mitigations with status Proposed.";

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
            return "Proposed Mitigation Counter";
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
