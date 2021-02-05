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
    [Extension("6555EBB7-8D35-4C50-9B0F-61D22994991B", "Threat Type Edit Context Aware Action", 60, ExecutionMode.Simplified)]
    public class EditThreatType : IIdentityContextAwareAction, 
        IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Edit the Underlying Threat Type";
        public string Group => "Other";
        public Bitmap Icon => Resources.threat_type_big;
        public Bitmap SmallIcon => Resources.threat_type;
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
                using (var dialog = new ItemEditorDialog())
                {
                    dialog.SetExecutionMode(_executionMode);
                    dialog.Item = threatEvent.ThreatType;
                    dialog.ShowDialog(Form.ActiveForm);
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
