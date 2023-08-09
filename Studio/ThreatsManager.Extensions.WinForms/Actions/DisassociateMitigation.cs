using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("7B43D139-2843-4E81-8394-33C9EBF43C79", "Disassociate Mitigation Context Aware Action", 45, ExecutionMode.Simplified)]
    public class DisassociateMitigation : IThreatEventMitigationContextAwareAction,
        IThreatTypeMitigationContextAwareAction, IWeaknessMitigationContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatEventMitigation | Scope.ThreatTypeMitigation | Scope.WeaknessMitigation;
        public string Label => "Disassociate the Mitigation";
        public string Group => "Associate";
        public Bitmap Icon => Resources.standard_mitigations_big_delete;
        public Bitmap SmallIcon => Resources.standard_mitigations_delete;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute([NotNull] object item)
        {
            bool result = false;

            if (item is IThreatTypeMitigation threatTypeMitigation)
            {
                result = Execute(threatTypeMitigation);
            } else if (item is IThreatEventMitigation threatEventMitigation)
            {
                result = Execute(threatEventMitigation);
            } else if (item is IWeaknessMitigation weaknessMitigation)
            {
                result = Execute(weaknessMitigation);
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

            if (MessageBox.Show(Form.ActiveForm,
                $"You are about to disassociate Mitigation '{mitigation.Mitigation.Name}'. Are you sure?",
                "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                using (var scope = UndoRedoManager.OpenScope("Disassociate Mitigation"))
                {
                    if (mitigation.ThreatType.RemoveMitigation(mitigation.MitigationId))
                    {
                        result = true;
                        scope?.Complete();
                        ShowMessage?.Invoke("Mitigation disassociated successfully.");
                    }
                    else
                    {
                        ShowWarning?.Invoke("The Mitigation association cannot be removed.");
                    }
                }
            }

            return result;
        }

        public bool Execute(IThreatEventMitigation mitigation)
        {
            bool result = false;

            if (MessageBox.Show(Form.ActiveForm,
                $"You are about to remove mitigation '{mitigation.Mitigation.Name}' from the current Threat Event. Are you sure?",
                "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                using (var scope = UndoRedoManager.OpenScope("Disassociate Mitigation"))
                {
                    if (mitigation.ThreatEvent.RemoveMitigation(mitigation.MitigationId))
                    {
                        result = true;
                        scope?.Complete();
                        ShowMessage?.Invoke("Mitigation disassociated successfully.");
                    }
                    else
                    {
                        ShowWarning?.Invoke("The Mitigation association cannot be removed.");
                    }
                }
            }

            return result;
        }

        public bool Execute(IWeaknessMitigation mitigation)
        {
            bool result = false;

            if (MessageBox.Show(Form.ActiveForm,
                $"You are about to remove mitigation '{mitigation.Mitigation.Name}' from the current Weakness. Are you sure?",
                "Remove Mitigation association", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                using (var scope = UndoRedoManager.OpenScope("Disassociate Mitigation"))
                {
                    if (mitigation.Weakness.RemoveMitigation(mitigation.MitigationId))
                    {
                        result = true;
                        scope?.Complete();
                        ShowMessage?.Invoke("Mitigation disassociated successfully.");
                    }
                    else
                    {
                        ShowWarning?.Invoke("The Mitigation association cannot be removed.");
                    }
                }
            }

            return result;
        }
    }
}
