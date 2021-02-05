using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("2F10F338-426F-445E-B865-65A25FC880F1", "Create Threat Event Scenario Context Aware Action", 50, ExecutionMode.Pioneer)]
    public class CreateThreatEventScenario : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Create a Threat Event Scenario...";
        public string Group => "Associate";
        public Bitmap Icon => Resources.scenario_big_new;
        public Bitmap SmallIcon => Resources.scenario_new;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            bool result = false;

            if (item is IIdentity identity)
            {
                result = Execute(identity);
            }

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IIdentity identity)
        {
            bool result = false;

            if (identity is IThreatEvent threatEvent)
            {
                using (var dialog = new ThreatEventScenarioCreationDialog(threatEvent))
                {
                    result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
                }
            }

            return result;
        }
    }
}
