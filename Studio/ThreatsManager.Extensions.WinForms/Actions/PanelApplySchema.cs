using ExCSS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("B6093EEB-32C6-4131-BD7B-D2694A71EECA", "Apply Schema in Panels", 200, ExecutionMode.Expert)]
    public class PanelApplySchema : IPropertiesContainersContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.All;

        public string Label => "Apply Schema";

        public string Group => "Schemas";

        public Bitmap Icon => Properties.Resources.properties;

        public Bitmap SmallIcon => Properties.Resources.properties_small;

        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        }, true, Icon);

        public IEnumerable<string> SupportedContexts => null;

        public IEnumerable<string> UnsupportedContexts => new[] { "Roadmap" };

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(object item)
        {
            return Execute(item as IEnumerable<IIdentity>);
        }

        public bool Execute(IEnumerable<IPropertiesContainer> containers)
        {
            bool result = true;

            if (containers?.Any() ?? false)
            {
                try
                {
                    var dialog = new ApplySchemaMultipleDialog();
                    dialog.Initialize(containers);
                    if (dialog.ShowDialog(System.Windows.Forms.Form.ActiveForm) == System.Windows.Forms.DialogResult.OK)
                    {
                        var schema = dialog.SelectedSchema;
                        if (schema != null)
                        {
                            using (var scope = UndoRedoManager.OpenScope("Apply Schema to items"))
                            {
                                foreach (var container in containers)
                                {
                                    container.Apply(schema);
                                }

                                scope?.Complete();
                            }
                        }
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
