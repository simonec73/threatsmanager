using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Actions;
using ThreatsManager.DevOps.Panels.MitigationsKanban;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Initializers
{
    /// <summary>
    /// This initializer connects automatically to DevOps.
    /// </summary>
    [Extension("544AA74A-4C50-4C13-A952-C70831434780", "Automatic Connector to DevOps", 50, ExecutionMode.Management)]
    public class AutoConnector : IPostLoadProcessor
    {
        public void Process([NotNull] IThreatModel model)
        {
            var schemaManager = new DevOpsConfigPropertySchemaManager(model);
            var connector = schemaManager.GetDevOpsConnector(out var url, out var project);
            if (connector != null && !string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(project))
            {
                var projects = connector.Connect(url);
                if (projects.Contains(project))
                {
                    if (connector.OpenProject(project))
                    {
                        DevOpsManager.Register(connector, model);
                        Connect.ChangeDisconnectButtonStatus(connector, true);

                        var configManager = new ExtensionConfigurationManager(model, this.GetExtensionId());
                        configManager.Apply();
                    }
                }
            }
        }
    }
}
