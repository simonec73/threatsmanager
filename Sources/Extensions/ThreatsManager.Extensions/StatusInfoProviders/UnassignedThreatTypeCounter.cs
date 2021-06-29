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
    [Extension("7C960B99-F1DD-4D94-A030-48BFEB11B927", 
        "Threat Types with no associated Threat Event Counter Status Info Provider", 28, ExecutionMode.Simplified)]
    public class UnassignedThreatTypeCounter : IStatusInfoProviderExtension
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
            $"Unassigned Threat Types: {(_model.ThreatTypes?.Count() ?? 0) - _model.AssignedThreatTypes}";

        public string Description => "Counter of the Threat Types having no associated Threat Event.";

        private void Update(IThreatEventsContainer container, IThreatEvent mitigation)
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Unassigned Threat Type Counter";
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
