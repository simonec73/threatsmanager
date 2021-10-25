using System;
using System.Drawing;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("C2029A88-0E8E-49F1-A9DF-3FEFAF5582CB", 
        "Apply Mitigation Association Rule to Threat Event Context Aware Action", 53, ExecutionMode.Simplified)]
    public class ApplyMitigationAssociationRules : IIdentityContextAwareAction, IDesktopAlertAwareExtension, IAsker
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Apply Mitigation Association Rules";
        public string Group => "ItemActions";
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
            if (identity is IThreatEvent threatEvent)
            {
                if (threatEvent.HasTop())
                {
                    Ask?.Invoke(this, threatEvent, "Association of Mitigations",
                        $"Do you want to associate all Mitigations to {identity.Name}?\nPress Yes to confirm.\nPress No to associate only the Top Mitigations.\nPress Cancel to avoid associating anything.",
                        false, RequestOptions.YesNoCancel);
                }
                else
                {
                    Ask?.Invoke(this, threatEvent, "Association of Mitigations",
                        $"Do you want to associate Mitigations to {identity.Name}?",
                        false, RequestOptions.OkCancel);
                }

            }

            return true;
        }

        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;
        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes || answer == AnswerType.Ok || answer == AnswerType.No)
            {
                if (context is IThreatEvent threatEvent)
                {
                    if (threatEvent.ApplyMitigations(answer == AnswerType.No))
                        ShowMessage?.Invoke(Resources.SuccessAssociation);
                    else
                    {
                        ShowWarning?.Invoke(Resources.WarningNoAssociations);
                    }
                }
            }
        }
    }
}