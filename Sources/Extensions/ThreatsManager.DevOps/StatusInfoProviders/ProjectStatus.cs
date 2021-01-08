using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.StatusInfoProviders
{
    [Extension("0396C195-9084-4A39-8337-AE0BBACCE8C4", "DevOps Connector Status Info Provider", 100, ExecutionMode.Business)]
    public class ProjectStatus : IStatusInfoProviderExtension
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

        public string CurrentStatus
        {
            get
            {
                return GetCurrentStatus(DevOpsManager.GetConnector(_model));
            }
        }

        public string Description => "Status Info Provider showing which DevOps Connector is active.";

        public override string ToString()
        {
            return "DevOps Connector Status";
        }

        public void Dispose()
        {
            DevOpsManager.ConnectorAdded -= OnConnectorAdded;
            DevOpsManager.ConnectorRemoved -= OnConnectorRemoved;
            _model = null;
        }

        private string GetCurrentStatus(IDevOpsConnector connector)
        {
            return $"DevOps Connector: {(connector?.ToString() ?? "<Disconnected>")}";
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
