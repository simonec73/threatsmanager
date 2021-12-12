using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.DevOps.Actions
{
    [Extension("83FA5293-988D-4FD4-A962-D9726EB6F25A", "Assign a Mitigation to an Iteration Context Aware Action", 40, ExecutionMode.Management)]
    public class AssignToIteration : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Mitigation;
        public string Label => "Assign to Iteration";
        public string Group => "Iterations";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
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

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IMitigation mitigation)
            {
                var dialog = new DevOpsIterationAssignmentDialog(mitigation);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            }

            return result;
        }
    }
}
