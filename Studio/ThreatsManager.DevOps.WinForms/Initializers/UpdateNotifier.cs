using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Initializers
{
    [Extension("D527CFC8-E594-415A-A993-3FC1175C6487", "DevOps Update Notifier", 100, ExecutionMode.Management)]
    public class UpdateNotifier : IExtensionInitializer, IDesktopAlertAwareExtension
    {
        public void Initialize()
        {
            DevOpsManager.RefreshDone += DevOpsManagerOnRefreshDone;
        }

        private void DevOpsManagerOnRefreshDone([NotNull] IThreatModel model, int count)
        {
            var configManager = new ExtensionConfigurationManager(model, (new AutoConnector()).GetExtensionId());
            bool show;
            switch (configManager.NotificationStrategy)
            {
                case NotificationType.SuccessOnly:
                    show = count > 0;
                    break;
                case NotificationType.Full:
                    show = true;
                    break;
                default:
                    show = false;
                    break;
            }

            if (show)
                ShowMessage?.Invoke($"DevOps Updater has detected {count} update(s).");
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
    }
}
