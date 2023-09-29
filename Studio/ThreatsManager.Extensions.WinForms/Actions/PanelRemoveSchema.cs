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
    [Extension("4C6BF542-1F16-4A81-BE86-647E11160AF4", "Remove Schema in Panels", 202, ExecutionMode.Expert)]
    public class PanelRemoveSchema : IPropertiesContainersContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.All;

        public string Label => "Remove Schema";

        public string Group => "Schemas";

        public Bitmap Icon => Properties.Resources.properties_delete;

        public Bitmap SmallIcon => Properties.Resources.properties_small_delete;

        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        }, true, Properties.Resources.properties);

        public IEnumerable<string> SupportedContexts => null;

        public IEnumerable<string> UnsupportedContexts => null;

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
                    var dialog = new RemoveSchemaMultipleDialog();
                    dialog.Initialize(containers);
                    if (dialog.ShowDialog(System.Windows.Forms.Form.ActiveForm) == System.Windows.Forms.DialogResult.OK)
                    {
                        var schema = dialog.SelectedSchema;
                        if (schema != null)
                        {
                            using (var scope = UndoRedoManager.OpenScope("Remove Schema from items"))
                            {
                                foreach (var container in containers)
                                {
                                    RemoveSchema(container, schema);
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

        private void RemoveSchema(IPropertiesContainer container, IPropertySchema schema)
        {
            var properties = container.Properties?
                .Where(x => (x.PropertyType?.SchemaId ?? Guid.Empty) == schema.Id)
                .ToArray();

            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    container.RemoveProperty(property.PropertyTypeId);
                }
            }
        }
    }
}
