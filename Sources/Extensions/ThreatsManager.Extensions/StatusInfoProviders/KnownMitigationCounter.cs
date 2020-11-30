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
    [Extension("F6103ED2-6ECF-43D0-B47E-A08FB56E7A84", "Mitigation Counter Status Info Provider", 21, ExecutionMode.Simplified)]
    public class KnownMitigationCounter : IStatusInfoProviderExtension
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
            _model.ChildCreated += Update;
            _model.ChildRemoved += Update;
        }

        public string CurrentStatus =>
            $"Known Mitigations: {_model?.Mitigations?.Count() ?? 0}";

        public string Description => "Counter of the Known Mitigations defined in the Threat Model.";

        private void Update(IIdentity obj)
        {
            if (obj is IMitigation)
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Known Mitigation Counter";
        }
 
        public void Dispose()
        {
            _model.ChildCreated -= Update;
            _model.ChildRemoved -= Update;
            _model = null;
        }
    }
}
