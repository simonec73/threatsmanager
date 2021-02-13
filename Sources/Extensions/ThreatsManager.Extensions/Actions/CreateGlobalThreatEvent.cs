using System;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
#pragma warning disable CS0067
    [Extension("7FCCFBE8-A4DF-4004-A192-CD223AC6F187", "Create Global Threat Event Context Aware Action", 10, ExecutionMode.Simplified)]
    public class CreateGlobalThreatEvent : IIdentityContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatType;
        public string Label => "Create Global Threat Event";
        public string Group => "ThreatTypesActions";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IThreatType threatType)
            {
                threatType.Model?.AddThreatEvent(threatType);
                ShowMessage?.Invoke("Add Global Threat Event has been executed successfully.");
                result = true;
            }

            return result;
        }
    }
}