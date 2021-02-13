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
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("74921A99-9F27-4873-97AD-0D279A89B04F", "Associate Mitigation Context Aware Action", 40, ExecutionMode.Simplified)]
    public class AssociateMitigation : IIdentityContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatEvent | Scope.ThreatType;
        public string Label => "Associate a Mitigation";
        public string Group => "Associate";
        public Bitmap Icon => Resources.standard_mitigations_big;
        public Bitmap SmallIcon => Resources.standard_mitigations;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

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
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        result = true;
                        ShowMessage?.Invoke("Mitigation added successfully.");
                    }
                }
            }
            else if (identity is IThreatType threatType)
            {
                using (var dialog = new ThreatTypeMitigationSelectionDialog(threatType))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        result = true;
                        ShowMessage?.Invoke("Mitigation added successfully.");
                    }
                }

            }

            return result;
        }
    }
}
