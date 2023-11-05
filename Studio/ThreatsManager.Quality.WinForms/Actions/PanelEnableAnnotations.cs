using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Actions
{
    [Extension("96DB7BCB-93B0-4C42-B6CD-135E8EC712DA", "Enable Annotations in Panels", 1000, ExecutionMode.Management)]
    public class PanelEnableAnnotations : IPropertiesContainersContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.All;

        public string Label => "Enable Annotations";

        public string Group => "Annotations";

        public Bitmap Icon => Properties.Resources.note_text;

        public Bitmap SmallIcon => Properties.Resources.note_text_small;

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
            bool result = false;

            var model = containers?.OfType<IThreatModelChild>().FirstOrDefault()?.Model;

            if (model != null)
            {
                result = true;
                var schemaManager = new AnnotationsPropertySchemaManager(model);

                using (var scope = UndoRedoManager.OpenScope("Enable Annotations on multiple objects"))
                {
                    try
                    {
                        foreach (var container in containers)
                        {
                            schemaManager.EnableAnnotations(container);
                        }
                    }
                    catch
                    {
                        result = false;
                    }

                    if (result)
                        scope?.Complete();
                }
            }

            return result;
        }
    }
}
