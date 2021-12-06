using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("151EBBC3-2888-44CB-9BD9-6DA6612367F9", "Associate Mitigation Context Aware Action", 41, ExecutionMode.Simplified)]
    public class AssociateMultipleMitigations : IIdentityContextAwareAction
    {
        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public Scope Scope => Scope.ThreatType | Scope.ThreatEvent | Scope.Weakness | Scope.Vulnerability;
        public string Label => "Associate multiple Mitigations";
        public string Group => "Associate";
        public Bitmap Icon => Icons.Resources.standard_mitigations;
        public Bitmap SmallIcon => Icons.Resources.standard_mitigations_small;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(IIdentity identity)
        {
            var result = false;
            var dialog = new AssociateMitigationsDialog();

            if (identity is IThreatType threatType)
            {
                dialog.Initialize(threatType);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            } else if (identity is IThreatEvent threatEvent)
            {
                dialog.Initialize(threatEvent);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            }
            else if (identity is IWeakness weakness)
            {
                dialog.Initialize(weakness);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            }
            else if (identity is IVulnerability vulnerability)
            {
                dialog.Initialize(vulnerability);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            }

            return result;
        }
    }
}
