using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("22C195E8-A95F-4C07-AD73-C7DCE0DECE6B", "Change Template Context Aware Action", 32, ExecutionMode.Simplified)]
    public class ChangeTemplate : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Entity | Scope.TrustBoundary | Scope.DataFlow;
        public string Label => "Change Template";
        public string Group => "ItemActions";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

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

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            using (var dialog = new ChangeTemplateDialog(identity))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    result = true;
            }

            return result;
        }
    }
}