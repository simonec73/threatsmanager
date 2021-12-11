using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.DevOps.Actions
{
    [Extension("BDC79BF8-2737-4F28-B8F4-D95612C08482", "Unassign a Mitigation from any Iteration Context Aware Action", 41, ExecutionMode.Management)]
    public class UnassignFromIteration : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Mitigation;
        public string Label => "Unassign from Iteration";
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

            if (identity is IMitigation mitigation && mitigation.Model is IThreatModel model)
            {
                var schemaManager = new DevOpsPropertySchemaManager(model);
                var iteration = schemaManager.GetFirstSeenOn(mitigation)?.GetIteration(model);
                if (iteration != null)
                {
                    if (MessageBox.Show(
                        $"You are about to unassign mitigation '{mitigation.Name}' from iteration '{iteration.Name}'." +
                        $"\nDo you confirm?", "Unassign from Iteration", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        schemaManager.SetFirstSeenOn(mitigation, null);
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
