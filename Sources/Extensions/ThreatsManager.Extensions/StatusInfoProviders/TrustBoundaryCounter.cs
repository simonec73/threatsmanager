using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("731C499F-5FE8-412F-A1A9-A81921A8D15C", "Trust Boundary Counter Status Info Provider", 14, ExecutionMode.Simplified)]
    public class TrustBoundaryCounter : IStatusInfoProviderExtension
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
            $"Trust Boundaries: {_model?.Groups?.OfType<ITrustBoundary>().Count() ?? 0}";

        public string Description => "Counter of the Trust Boundaries defined in the Threat Model.";

        private void Update(IIdentity obj)
        {
            if (obj is ITrustBoundary)
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Trust Boundary Counter";
        }
 
        public void Dispose()
        {
            _model.ChildCreated -= Update;
            _model.ChildRemoved -= Update;
            _model = null;
        }
    }
}
