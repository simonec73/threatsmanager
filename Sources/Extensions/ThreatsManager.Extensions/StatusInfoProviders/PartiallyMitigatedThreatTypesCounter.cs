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
    [Extension("A4ADAC0C-46BB-4306-925C-C09CA4976E83", 
        "Partially mitigated Threat Types Counter Status Info Provider", 30, ExecutionMode.Simplified)]
    public class PartiallyMitigatedThreatTypesCounter : IStatusInfoProviderExtension
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
            _model.ChildCreated += ChildCreated;
            _model.ChildRemoved += ChildRemoved;

            var threats = _model.ThreatTypes?.ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    threat.ThreatTypeMitigationAdded += Update;
                    threat.ThreatTypeMitigationRemoved += Update;
                }
            }
        }

        public string CurrentStatus =>
            $"Partially-mitigated Threat Types: {_model.PartiallyMitigatedThreatTypes}";

        public string Description => "Counter of the Threat Types having partial mitigations.";

        private void ChildCreated(IIdentity identity)
        {
            if (identity is IThreatType threat)
            {
                threat.ThreatTypeMitigationAdded += Update;
                threat.ThreatTypeMitigationRemoved += Update;
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
            }
        }

        private void ChildRemoved(IIdentity identity)
        {
            if (identity is IThreatType threat)
            {
                threat.ThreatTypeMitigationAdded -= Update;
                threat.ThreatTypeMitigationRemoved -= Update;
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
            }
        }

        private void Update(IThreatTypeMitigationsContainer arg1, IThreatTypeMitigation arg2)
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Partially-mitigated Threat Type Counter";
        }

        public void Dispose()
        {
            _model.ChildCreated -= ChildCreated;
            _model.ChildRemoved -= ChildRemoved;

            var threats = _model.ThreatTypes?.ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    threat.ThreatTypeMitigationAdded -= Update;
                    threat.ThreatTypeMitigationRemoved -= Update;
                }
            }

            _model = null;
        }
    }
}
