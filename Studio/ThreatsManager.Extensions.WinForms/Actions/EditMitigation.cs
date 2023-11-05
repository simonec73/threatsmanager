using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("E9183827-B866-4BCA-AF14-FA0AE0C640B8", "Mitigation Edit Context Aware Action", 30, ExecutionMode.Simplified)]
    public class EditMitigation : IThreatTypeMitigationContextAwareAction, 
        IThreatEventMitigationContextAwareAction, IWeaknessMitigationContextAwareAction, IExecutionModeSupport
    {
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        public Scope Scope => Scope.ThreatEventMitigation | Scope.ThreatTypeMitigation | Scope.WeaknessMitigation;
        public string Label => "Edit the Underlying Mitigation";
        public string Group => "Edit";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            bool result = false;

            if (item is IThreatTypeMitigation ttm)
            {
                result = Execute(ttm);
            } else if (item is IThreatEventMitigation tem)
            {
                result = Execute(tem);
            }

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IThreatEventMitigation teMitigation)
        {
            bool result = false;

            if (teMitigation != null)
            {
                using (var dialog = new ItemEditorDialog())
                {
                    using (var scope = UndoRedoManager.OpenScope("Edit Mitigation"))
                    {
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Item = teMitigation.Mitigation;
                        dialog.ShowDialog(Form.ActiveForm);
                        scope?.Complete();
                    }
                }
                result = true;
            }

            return result;
        }

        public bool Execute(IThreatTypeMitigation ttMitigation)
        {
            bool result = false;

            if (ttMitigation != null)
            {
                using (var dialog = new ItemEditorDialog())
                {
                    using (var scope = UndoRedoManager.OpenScope("Edit Mitigation"))
                    {
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Item = ttMitigation.Mitigation;
                        dialog.ShowDialog(Form.ActiveForm);
                        scope?.Complete();
                    }
                }                
                result = true;
            }

            return result;
        }

        public bool Execute(IWeaknessMitigation wMitigation)
        {
            bool result = false;

            if (wMitigation != null)
            {
                using (var dialog = new ItemEditorDialog())
                {
                    using (var scope = UndoRedoManager.OpenScope("Edit Mitigation"))
                    {
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Item = wMitigation.Mitigation;
                        dialog.ShowDialog(Form.ActiveForm);
                        scope?.Complete();
                    }
                }
                result = true;
            }

            return result;
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }
    }
}