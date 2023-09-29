using ExCSS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("664654F9-0DEB-41B3-9711-0C9BDF53CEF8", "Change Template in Panels", 100, ExecutionMode.Expert)]
    public class PanelChangeTemplate : IIdentitiesContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.Entity | Scope.DataFlow | Scope.TrustBoundary;

        public string Label => "Change Template";

        public string Group => "Edit";

        public Bitmap Icon => Properties.Resources.rubber_stamp;

        public Bitmap SmallIcon => Properties.Resources.rubber_stamp_small;

        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        }, true, Icon);

        public IEnumerable<string> SupportedContexts => new[] { "ExternalInteractorList", "ProcessList", "DataStoreList", "DataFlowList", "TrustBoundaryList" };

        public IEnumerable<string> UnsupportedContexts => null;

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

            if (identities?.Any() ?? false)
            {
                try
                {
                    ChangeTemplateMultipleDialog dialog = null;

                    var externalInteractors = identities.OfType<ExternalInteractor>().ToList();
                    if (externalInteractors.Any())
                    {
                        dialog = new ChangeTemplateMultipleDialog(externalInteractors);
                    }
                    else
                    {
                        var processes = identities.OfType<Process>().ToList();
                        if (processes.Any())
                        {
                            dialog = new ChangeTemplateMultipleDialog(processes);
                        }
                        else
                        {
                            var dataStores = identities.OfType<DataStore>().ToList();
                            if (dataStores.Any())
                            {
                                dialog = new ChangeTemplateMultipleDialog(dataStores);
                            }
                            else
                            {
                                var flows = identities.OfType<DataFlow>().ToList();
                                if (flows.Any())
                                {
                                    dialog = new ChangeTemplateMultipleDialog(flows);
                                }
                                else
                                {
                                    var trustBoundaries = identities.OfType<TrustBoundary>().ToList();
                                    if (trustBoundaries.Any())
                                    {
                                        dialog = new ChangeTemplateMultipleDialog(trustBoundaries);
                                    }
                                }
                            }
                        }
                    }

                    if (dialog != null)
                    {
                        using (var scope = UndoRedoManager.OpenScope("Change Template on multiple items"))
                        {
                            if (dialog.ShowDialog(System.Windows.Forms.Form.ActiveForm) == System.Windows.Forms.DialogResult.OK)
                                result = true;

                            if (result) scope?.Complete();
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
