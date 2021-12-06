using System.Drawing;
using ThreatsManager.AutoThreatGeneration.Dialogs;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("EBF2DAFE-9C01-4F72-B5AA-E8727D2D1287", "Edit Automatic Threat Generation Rule", 50, ExecutionMode.Expert)]
    public class EditAutoGenRule : IIdentityContextAwareAction
    {
        public Interfaces.Scope Scope => Scope.ThreatType;
        public string Label => "Edit Automatic Threat Generation Rule...";
        public string Group => "AutoGen";
        public Bitmap Icon => Resources.threat_type_big_gearwheel;
        public Bitmap SmallIcon => Resources.threat_type_gearwheel;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IThreatType threatType)
            {
                result = Execute(threatType);
            }

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
                using (var dialog = new RuleEditDialog())
                {
                    dialog.Initialize(threatType);

                    result = threatType.SetRule(dialog);
                }
            }

            return result;
        }
    }
}