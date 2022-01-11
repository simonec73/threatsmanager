using System;
using System.Linq;
using ThreatsManager.DevOps.Actions;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.DevOps.Initializers
{
    /// <summary>
    /// This initializer connects automatically to DevOps.
    /// </summary>
    [Extension("544AA74A-4C50-4C13-A952-C70831434780", "Automatic Connector to DevOps", 50, ExecutionMode.Management)]
    public class AutoConnector : IPostLoadProcessor, IDesktopAlertAwareExtension
    {
        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public async void Process(IThreatModel model)
        {
            Connect.ChangeDisconnectButtonStatus(null, false);

            var schemaManager = new DevOpsConfigPropertySchemaManager(model);
            var connector = schemaManager.GetDevOpsConnector(out var url, out var project);
            if (connector != null && !string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(project))
            {
                try
                {
                    var tokenManager = new SecretsManager();
                    var token = tokenManager.GetSecret(url);
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        connector.Connect(url, token);
                        var projects = (await connector.GetProjectsAsync())?.ToArray();
                        if (projects?.Contains(project) ?? false)
                        {
                            if (connector.OpenProject(project))
                            {
                                DevOpsManager.Register(connector, model);
                                Connect.ChangeDisconnectButtonStatus(connector, true);

                                var configManager = new ExtensionConfigurationManager(model, this.GetExtensionId());
                                configManager.Apply();

                                await DevOpsManager.UpdateAsync(model);
                            }
                        }
                        else
                        {
                            connector.Disconnect();
                            ShowWarning?.Invoke(
                                "DevOps system cannot be automatically connected due to an internal error.");
                        }
                    }
                    else
                    {
                        ShowWarning?.Invoke(
                            "DevOps system cannot be automatically connected because no Personal Access Token has been found.");
                    }
                }
                catch (Exception exc)
                {
                    ShowWarning?.Invoke($@"DevOps system cannot be automatically connected due to the following error: {exc.Message}. Everything else should be unaffected.");
                }
            }
        }
    }
}
