using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("9E2A2E02-4BA4-47ED-8D89-5D6A469321CD", "Change Entity Icon Context Aware Action", 30, ExecutionMode.Simplified)]
    public class ChangeEntityIcon : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Change Entity Icon";
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
            if (identity is IEntity entity)
            {
                using (var dialog = new SelectImagesDialog(entity))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var bigImage = dialog.BigImage;
                        if (bigImage != null)
                        {
                            entity.BigImage = bigImage;
                        }

                        var image = dialog.Image;
                        if (image != null)
                            entity.Image = image;
                        var smallImage = dialog.SmallImage;
                        if (smallImage != null)
                            entity.SmallImage = smallImage;

                    }
                }
            }

            return true;
        }
    }
}