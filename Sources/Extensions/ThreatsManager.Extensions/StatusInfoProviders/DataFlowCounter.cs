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
    [Extension("1F1A0EC1-C0F1-46DF-957E-FE2F3FC7D84A", "Flow Counter Status Info Provider", 13, ExecutionMode.Simplified)]
    public class DataFlowCounter : IStatusInfoProviderExtension
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
            $"Flows: {_model?.DataFlows?.Count() ?? 0}";

        public string Description => "Counter of the Flows defined in the Threat Model.";

        private void Update(IIdentity obj)
        {
            if (obj is IDataFlow)
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Flow Counter";
        }
 
        public void Dispose()
        {
            _model.ChildCreated -= Update;
            _model.ChildRemoved -= Update;
            _model = null;
        }
    }
}
