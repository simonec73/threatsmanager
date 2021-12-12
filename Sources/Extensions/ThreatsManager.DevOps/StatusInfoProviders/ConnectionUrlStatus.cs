using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.StatusInfoProviders
{
    [Extension("731CE9F1-6973-4619-A7A7-44654CB0EE78", "DevOps Connection Url Status Info Provider", 100, ExecutionMode.Management)]
    public class ConnectionUrlStatus : IStatusInfoProviderExtension
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
            DevOpsManager.ConnectorAdded += OnConnectorAdded;
            DevOpsManager.ConnectorRemoved += OnConnectorRemoved;
        }

        public string CurrentStatus => GetCurrentStatus(DevOpsManager.GetConnector(_model));

        public string Description => "Status Info Provider showing the Url of the DevOps service.";

        public override string ToString()
        {
            return "DevOps Url";
        }

        public void Dispose()
        {
            DevOpsManager.ConnectorAdded -= OnConnectorAdded;
            DevOpsManager.ConnectorRemoved -= OnConnectorRemoved;
            _model = null;
        }

        private string GetCurrentStatus(IDevOpsConnector connector)
        {
            return $"DevOps Url: {(connector?.Url ?? "<Disconnected>")}";
        }

        private void OnConnectorAdded(IThreatModel model, IDevOpsConnector connector)
        {
            if (model == _model)
                UpdateInfo?.Invoke(this.GetExtensionId(), GetCurrentStatus(connector));
        }

        private void OnConnectorRemoved(IThreatModel model, IDevOpsConnector connector)
        {
            if (model == _model)
                UpdateInfo?.Invoke(this.GetExtensionId(), GetCurrentStatus(null));
        }
    }
}
