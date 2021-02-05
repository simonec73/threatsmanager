using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("29A32F8C-2E72-4438-BED7-B7E7F56727B8", "Create Threat Event Mitigation Context Aware Action", 51, ExecutionMode.Simplified)]
    public class CreateThreatEventMitigation : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Create a Threat Event Mitigation...";
        public string Group => "Associate";
        public Bitmap Icon => Resources.scenario_big_delete;
        public Bitmap SmallIcon => Resources.scenario_delete;
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
                using (var dialog = new ThreatEventMitigationSelectionDialog(threatEvent))
                {
                    result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
                }
            }

            return result;
        }
    }
}
