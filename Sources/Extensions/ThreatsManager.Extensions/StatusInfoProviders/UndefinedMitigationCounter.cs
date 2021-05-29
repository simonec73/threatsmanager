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
    [Extension("4F0DD59A-F93A-4FB7-B184-345D8DA9ABAB", "Undefined Mitigation Counter Status Info Provider", 37, ExecutionMode.Simplified)]
    public class UndefinedMitigationCounter : IStatusInfoProviderExtension
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
            $"Undefined Mitigations: {(_model.GetThreatEventMitigations()?.Where(x => x.Status == MitigationStatus.Undefined).Count() ?? 0)}";

        public string Description => "Counter of the Threat Event Mitigations with status Undefined.";

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
            return "Undefined Mitigation Counter";
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
