using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Help;

namespace ThreatsManager.Extensions.Panels.Troubleshooting
{
#pragma warning disable CS0067
    public partial class TroubleshootingPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Troubleshooting";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                return new List<ICommandsBarDefinition>()
                    {
                        new CommandsBarDefinition("Export", "Export", 
                            new []
                            {
                                new ActionDefinition(Guid.NewGuid(), "OpenWeb", "Open in Browser", 
                                    Properties.Resources.window_environment_big, Properties.Resources.window_environment),
                                new ActionDefinition(Guid.NewGuid(), "CopyUrl", "Copy URL", 
                                    Properties.Resources.copy_big, Properties.Resources.copy)

                            })
                    };
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            switch (action.Name)
            {
                case "OpenWeb":
                    if (_tree.SelectedNode?.Tag is Page page)
                    {
                        ProcessStartInfo sInfo = new ProcessStartInfo(page.Url);
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                        Process.Start(sInfo);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    }
                    break;
                case "CopyUrl":
                    if (_tree.SelectedNode?.Tag is Page page2)
                    {
                        Clipboard.SetText(page2.Url);
                        ShowMessage?.Invoke("URL copied successfully.");
                    }
                    break;
            }
        }
    }
}