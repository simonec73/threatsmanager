using System;
using System.Drawing;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("FC9151DF-D604-4B88-A529-6418495FA5C8", "Apply Auto Gen Rules to Item Context Aware Action", 35, ExecutionMode.Simplified)]
    public class ApplyAutoGenRulesToItem : IIdentityContextAwareAction, IDesktopAlertAwareExtension, IAsker
    {
        public Scope Scope => Scope.Entity | Scope.DataFlow;
        public string Label => "Apply Auto Gen Rules";
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
            if (identity is IThreatModelChild child && child.Model != null)
            {
                if (child.Model.HasTop())
                {
                    Ask?.Invoke(this, identity, "Generation of Threat Events and Mitigations",
                        $"Do you want to generate all Threat Events and Mitigations for {identity.Name}?\nPress Yes to confirm.\nPress No to generate only the Top Threats and Mitigations.\nPress Cancel to avoid generating anything.",
                        false, RequestOptions.YesNoCancel);
                }
                else
                {
                    Ask?.Invoke(this, identity, "Generation of Threat Events and Mitigations",
                        $"Do you want to generate Threat Events and Mitigations for {identity.Name}?",
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
                if (context is IEntity entity)
                {
                    if (entity.GenerateThreatEvents(answer == AnswerType.No))
                        ShowMessage?.Invoke(Resources.SuccessGeneration);
                    else
                    {
                        ShowWarning?.Invoke(Resources.WarningNothingGenerated);
                    }

                }
                else if (context is IDataFlow flow)
                {
                    if (flow.GenerateThreatEvents(answer == AnswerType.No))
                        ShowMessage?.Invoke(Resources.SuccessGeneration);
                    else
                    {
                        ShowWarning?.Invoke(Resources.WarningNothingGenerated);
                    }
                }
            }
        }
    }
}