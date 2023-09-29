using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Forms = System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Actions
{
    [Extension("4AB416DC-6AC4-424C-9982-0FD5BDEDD6D2", "Disable Annotations in Panels", 1005, ExecutionMode.Management)]
    public class PanelDisableAnnotations : IIdentitiesContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.All;

        public string Label => "Disable Annotations";

        public string Group => "Annotations";

        public Bitmap Icon => Properties.Resources.note_text_delete;

        public Bitmap SmallIcon => Properties.Resources.note_text_small_delete;

        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        }, true, Properties.Resources.note_text);

        public IEnumerable<string> SupportedContexts => null;

        public IEnumerable<string> UnsupportedContexts => null;

        Interfaces.Extensions.Shortcut IContextAwareAction.Shortcut => throw new NotImplementedException();

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(object item)
        {
            return Execute(item as IEnumerable<IIdentity>);
        }

        public bool Execute(IEnumerable<IIdentity> identities)
        {
            bool result = false;

            var containers = identities?.OfType<IPropertiesContainer>().ToArray();
            var model = containers?.OfType<IThreatModelChild>().FirstOrDefault()?.Model;

            if (model != null)
            {
                if (Forms.MessageBox.Show(Forms.Form.ActiveForm,
                    "Do you confirm Annotations removal from all the selected items?",
                    "Annotations removal", Forms.MessageBoxButtons.YesNo, Forms.MessageBoxIcon.Warning) == Forms.DialogResult.Yes)
                {
                    result = true;
                    var schemaManager = new AnnotationsPropertySchemaManager(model);

                    using (var scope = UndoRedoManager.OpenScope("Disable Annotations on multiple objects"))
                    {
                        try
                        {
                            foreach (var container in containers)
                            {
                                if (schemaManager.IsInitialized)
                                {
                                    schemaManager.DisableAnnotations(container);
                                }
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
            }

            return result;
        }
    }
}
