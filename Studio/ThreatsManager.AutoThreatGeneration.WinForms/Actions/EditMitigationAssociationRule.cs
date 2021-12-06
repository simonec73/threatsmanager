using System;
using System.Drawing;
using ThreatsManager.AutoThreatGeneration.Dialogs;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
#pragma warning disable CS0067
    [Extension("5CE7EB67-6D80-4A43-AEBC-32E413C7A8A1", "Edit Automatic Mitigation Association Rule", 52, ExecutionMode.Expert)]
    public class EditMitigationAssociationRule : IThreatTypeMitigationContextAwareAction, IDesktopAlertAwareExtension
    {
        public Interfaces.Scope Scope => Interfaces.Scope.ThreatTypeMitigation;
        public string Label => "Edit Automatic Mitigation Association Rule...";
        public string Group => "AutoGen";
        public Bitmap Icon => Resources.standard_mitigations_big_gearwheel;
        public Bitmap SmallIcon => Resources.standard_mitigations_gearwheel;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IThreatTypeMitigation mitigation)
            {
                result = Execute(mitigation);
            }

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            bool result = false;
            
            using (var dialog = new MitigationRuleEditDialog())
            {
                if (dialog.Initialize(mitigation))
                    result = mitigation.SetRule(dialog);
                else
                {
                    ShowWarning?.Invoke("Threat Event Generation Rule for the related Threat Type has not been set.");
                }
            }

            return result;
        }
    }
}