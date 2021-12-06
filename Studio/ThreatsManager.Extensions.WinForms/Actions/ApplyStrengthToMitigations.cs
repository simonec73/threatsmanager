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
    [Extension("FE74F5BC-31AC-4996-B3CB-6A6CF7152B07", "Apply same Strength to related Mitigations Context Aware Action", 
        32, ExecutionMode.Simplified)]
    public class ApplyStrengthToMitigations : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Mitigation;
        public string Label => "Apply Strength to all Instances";
        public string Group => "Mitigations";
        public Bitmap Icon => Icons.Resources.strength;
        public Bitmap SmallIcon => Icons.Resources.strength_small;
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
                var dialog = new SelectStrengthDialog(mitigation);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
                if (result && mitigation.Model is IThreatModel model)
                {
                    var mitigations = model.GetThreatEventMitigations(mitigation)?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var m in mitigations)
                        {
                            m.Strength = dialog.Strength;
                        }
                    }
                }
            }

            return result;
        }
    }
}
