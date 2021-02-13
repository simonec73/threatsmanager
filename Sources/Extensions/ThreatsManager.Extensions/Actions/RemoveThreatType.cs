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
    [Extension("8CB90E20-19C5-43C1-A79A-C10794FE014E", "Remove Threat Type Context Aware Action", 50, ExecutionMode.Simplified)]
    public class RemoveThreatType : IIdentityContextAwareAction, IDesktopAlertAwareExtension, IAsker
    {
        public Scope Scope => Scope.ThreatType;
        public string Label => "Remove Threat Type";
        public string Group => "ObjectRemoval";
        public Bitmap Icon => Icons.Resources.threat_type_big_delete;
        public Bitmap SmallIcon => Icons.Resources.threat_type_delete;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

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
            if (identity is IThreatType threatType)
            {
                Ask?.Invoke(this, threatType, "Remove Threat Type",
                    $"You are about to remove Threat Type '{threatType.Name}'. Are you sure?",
                    false, RequestOptions.YesNo);
            }

            return true;
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IThreatType threatType &&
                threatType.Model is IThreatModel model)
            {
                var result = model.RemoveThreatType(threatType.Id);
                if (result)
                    ShowMessage?.Invoke("Remove Threat Type has been executed successfully.");
                else
                    ShowWarning?.Invoke("Remove Threat Type has failed.");
            }
        }
    }
}