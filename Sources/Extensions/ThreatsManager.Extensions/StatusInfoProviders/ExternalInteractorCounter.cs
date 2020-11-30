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
    [Extension("DADF4373-6F4A-4943-ABC3-CA7B31CC1998", "External Interactor Counter Status Info Provider", 10, ExecutionMode.Simplified)]
    public class ExternalInteractorCounter : IStatusInfoProviderExtension
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
            $"External Interactors: {_model?.Entities?.OfType<IExternalInteractor>().Count() ?? 0}";

        public string Description => "Counter of the External Interactors defined in the Threat Model.";

        private void Update(IIdentity obj)
        {
            if (obj is IExternalInteractor)
                UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "External Interactors Counter";
        }
  
        public void Dispose()
        {
            _model.ChildCreated -= Update;
            _model.ChildRemoved -= Update;
            _model = null;
        }
    }
}
