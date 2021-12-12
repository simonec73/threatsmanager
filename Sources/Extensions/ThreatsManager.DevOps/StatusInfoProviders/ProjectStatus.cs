using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.StatusInfoProviders
{
    [Extension("0396C195-9084-4A39-8337-AE0BBACCE8C4", "DevOps Connector Status Info Provider", 100, ExecutionMode.Management)]
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
            var connector = DevOpsManager.GetConnector(model);
            if (connector != null)
                connector.ProjectOpened += OnProjectOpened;
            DevOpsManager.ConnectorAdded += OnConnectorAdded;
            DevOpsManager.ConnectorRemoved += OnConnectorRemoved;
        }

        public string CurrentStatus => GetCurrentStatus(DevOpsManager.GetConnector(_model));

        public string Description => "Status Info Provider showing the DevOps Project name.";

        public override string ToString()
        {
            return "DevOps Project";
        }

        public void Dispose()
        {
            DevOpsManager.ConnectorAdded -= OnConnectorAdded;
            DevOpsManager.ConnectorRemoved -= OnConnectorRemoved;
            var connector = DevOpsManager.GetConnector(_model);
            if (connector != null)
                connector.ProjectOpened -= OnProjectOpened;
            _model = null;
        }

        private string GetCurrentStatus(IDevOpsConnector connector)
        {
            return GetCurrentStatus(connector?.Project);
        }

        private string GetCurrentStatus(string project = null)
        {
            return $"DevOps Project: {(project ?? "<No Project Selected>")}";
        }

        private void OnProjectOpened(IDevOpsConnector connector, string projectName)
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), GetCurrentStatus(projectName));
        }

        private void OnConnectorAdded(IThreatModel model, IDevOpsConnector connector)
        {
            if (model == _model)
            {
                UpdateInfo?.Invoke(this.GetExtensionId(), GetCurrentStatus(connector));
                connector.ProjectOpened += OnProjectOpened;
            }
        }

        private void OnConnectorRemoved(IThreatModel model, IDevOpsConnector connector)
        {
            if (model == _model)
                UpdateInfo?.Invoke(this.GetExtensionId(), GetCurrentStatus());
            connector.ProjectOpened -= OnProjectOpened;
        }
    }
}
