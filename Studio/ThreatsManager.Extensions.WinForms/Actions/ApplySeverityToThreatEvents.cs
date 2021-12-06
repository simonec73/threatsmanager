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
    [Extension("1652517A-FA1D-4FDD-8082-BD5E7A5EA7D9", "Apply same Severity to related Threat Events Context Aware Action", 
        40, ExecutionMode.Simplified)]
    public class ApplySeverityToThreatEvents : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.ThreatType;
        public string Label => "Apply Severity to all Instances";
        public string Group => "ThreatEvent";
        public Bitmap Icon => Icons.Resources.severity;
        public Bitmap SmallIcon => Icons.Resources.severity_small;
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

            if (identity is IThreatType threatType)
            {
                var dialog = new SelectSeverityDialog(threatType);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
                if (result && threatType.Model is IThreatModel model)
                {
                    var threatEvents = model.GetThreatEvents(threatType)?.ToArray();
                    if (threatEvents?.Any() ?? false)
                    {
                        foreach (var threatEvent in threatEvents)
                        {
                            threatEvent.Severity = dialog.Severity;
                        }
                    }
                }
            }

            return result;
        }
    }
}
