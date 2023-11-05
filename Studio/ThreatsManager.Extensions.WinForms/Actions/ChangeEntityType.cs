//using System.Drawing;
//using System.Windows.Forms;
//using ThreatsManager.Extensions.Dialogs;
//using ThreatsManager.Interfaces;
//using ThreatsManager.Interfaces.Extensions.Actions;
//using ThreatsManager.Interfaces.ObjectModel;
//using ThreatsManager.Utilities;
//using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

//namespace ThreatsManager.Extensions.Actions
//{
//    [Extension("D9DD406D-56F1-400F-BC1C-8AA5DB7F114B", "Change Entity Type Context Aware Action", 40, ExecutionMode.Simplified)]
//    public class ChangeEntityType : IIdentityContextAwareAction
//    {
//        public Scope Scope => Scope.Entity;
//        public string Label => "Change Entity Type";
//        public string Group => "ItemActions";
//        public Bitmap Icon => null;
//        public Bitmap SmallIcon => null;
//        public Shortcut Shortcut => Shortcut.None;

//        public bool Execute(object item)
//        {
//            bool result = false;

//            if (item is IIdentity identity)
//                result = Execute(identity);

//            return result;
//        }

//        public bool IsVisible(object item)
//        {
//            return true;
//        }

//        public bool Execute(IIdentity identity)
//        {
//            bool result = false;

//            using (var scope = UndoRedoManager.OpenScope("Change Entity Type"))
//            {
//                using (var dialog = new ChangeTemplateDialog(identity))
//                {
//                    if (dialog.ShowDialog() == DialogResult.OK)
//                        result = true;
//                }

//                if (result) scope?.Complete();
//            }

//            return result;
//        }
//    }
//}