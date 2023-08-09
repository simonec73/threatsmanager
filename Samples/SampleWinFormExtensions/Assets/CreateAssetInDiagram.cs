using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.SampleWinFormExtensions.Assets;
using System.Drawing;
using ThreatsManager.Interfaces.Extensions.Actions;
using System.Linq;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("F3182F64-9E8E-46C9-96B3-00F68BBD47A0", "Create Asset In Diagram Context Aware Action", 500, ExecutionMode.Simplified)]
    public class CreateAssetInDiagram : IIdentityContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.ThreatModel;
        public string Label => "Create Asset";
        public string Group => "Add";
        public Bitmap Icon => Icons.Resources.undefined_big;
        public Bitmap SmallIcon => Icons.Resources.undefined;
        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, true, Shortcut)
            {
                Tag = this
            }
        });

        public string VisibilityContext => "Diagram";

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IThreatModel model)
                result = Execute(model);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IThreatModel model)
            {
                var dialog = new CreateAssetDialog(model);
                dialog.Show(System.Windows.Forms.Form.ActiveForm);
            }

            return result;
        }
    }
}
