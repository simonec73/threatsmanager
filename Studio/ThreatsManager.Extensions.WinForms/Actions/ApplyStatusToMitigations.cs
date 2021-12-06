using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.DevOps.Actions
{
    [Extension("46B17EC5-D0ED-47DE-85A7-B29C369727F5", "Apply same Status to related Mitigations Context Aware Action", 
        30, ExecutionMode.Simplified)]
    public class ApplyStatusToMitigations : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Mitigation;
        public string Label => "Apply Status to all Instances";
        public string Group => "Mitigations";
        public Bitmap Icon => Icons.Resources.mitigations;
        public Bitmap SmallIcon => Icons.Resources.mitigations_small;
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
                var dialog = new SelectStatusDialog(mitigation);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
                if (result && mitigation.Model is IThreatModel model)
                {
                    var mitigations = model.GetThreatEventMitigations(mitigation)?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var m in mitigations)
                        {
                            m.Status = dialog.Status;
                        }
                    }
                }
            }

            return result;
        }
    }
}
