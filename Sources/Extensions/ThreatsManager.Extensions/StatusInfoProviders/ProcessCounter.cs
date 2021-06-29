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
    [Extension("82DF6644-1955-464A-A244-C395856B527B", "Process Counter Status Info Provider", 11, ExecutionMode.Simplified)]
    public class ProcessCounter : IStatusInfoProviderExtension
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
            $"Processes: {_model?.Entities?.OfType<IProcess>().Count() ?? 0}";

        public string Description => "Counter of the Processes defined in the Threat Model.";

        private void Update(IIdentity obj)
        {
            if (obj is IProcess)
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Processes Counter";
        }
 
        public void Dispose()
        {
            _model.ChildCreated -= Update;
            _model.ChildRemoved -= Update;
            _model = null;
        }
    }
}
