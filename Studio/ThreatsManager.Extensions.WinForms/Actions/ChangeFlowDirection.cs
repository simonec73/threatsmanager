using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("82480A81-36AC-4045-9C73-95D60B8F905F", "Change Flow Direction Context Aware Action", 35, ExecutionMode.Simplified)]
    public class ChangeEntityType : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.DataFlow;
        public string Label => "Change Flow Direction";
        public string Group => "Flow";
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

            if (identity is IDataFlow flow)
            {
                using (var scope = UndoRedoManager.OpenScope("Reverse direction of the Flow"))
                {
                    flow.Flip();
                    scope?.Complete();
                }
            }

            return result;
        }
    }
}