using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("D02C5FE4-19ED-4FA0-B978-D197DF519198", "Apply Auto Gen Rules in Panels", 200, ExecutionMode.Simplified)]
    public class PanelApplyAutoGenRules : IIdentitiesContextAwareAction, ICommandsBarContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ExternalInteractor | Scope.Process | Scope.DataStore | Scope.DataFlow;

        public string Label => "Apply Auto Gen Rules";

        public string Group => "Auto Generation";

        public Bitmap Icon => Properties.Resources.industrial_robot;

        public Bitmap SmallIcon => Properties.Resources.industrial_robot_small;

        public Shortcut Shortcut => Shortcut.None;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        }, false);

        public IEnumerable<string> SupportedContexts => null;
        public IEnumerable<string> UnsupportedContexts => null;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

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

            var model = identities?.OfType<IThreatModelChild>().FirstOrDefault(x => x != null)?.Model;

            if (model != null)
            {
                var containers = identities.OfType<IThreatEventsContainer>().ToArray();

                if (containers.Any())
                {
                    var generated = false;

                    try
                    {

                        if (model.HasTop())
                        {
                            var outcome = System.Windows.Forms.MessageBox.Show(System.Windows.Forms.Form.ActiveForm,
                                $"Do you want to generate all Threat Events and Mitigations for the selected {identities.Count()} items?\nPress Yes to confirm.\nPress No to generate only the Top Threats and Mitigations.\nPress Cancel to avoid generating anything.",
                                Properties.Resources.ApplyAutoGenRules_Caption,
                                System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            switch (outcome)
                            {
                                case System.Windows.Forms.DialogResult.Yes:
                                    generated = GenerateThreats(model, containers, false);
                                    break;
                                case System.Windows.Forms.DialogResult.No:
                                    generated = GenerateThreats(model, containers, true);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            if (System.Windows.Forms.MessageBox.Show(System.Windows.Forms.Form.ActiveForm,
                                $"Do you want to generate all Threat Events and Mitigations for the selected {identities.Count()} items?",
                                Properties.Resources.ApplyAutoGenRules_Caption,
                                System.Windows.Forms.MessageBoxButtons.OKCancel,
                                System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                            {
                                generated = GenerateThreats(model, containers, false);
                            }
                        }
                    }
                    catch
                    {
                        ShowWarning?.Invoke("Automatic Threat Event generation failed.\nNo changes have been applied.");
                    }

                    result = generated;
                }                
            }

            return result;
        }

        private bool GenerateThreats([NotNull] IThreatModel model, 
            [NotNull] IEnumerable<IThreatEventsContainer> containers, bool topOnly)
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Apply Auto Gen Rules"))
            {
                var schemaManager = new AutoGenRulesPropertySchemaManager(model);

                foreach (var container in containers) 
                {
                    result |= container.GenerateThreatEvents(topOnly, schemaManager);
                }

                scope?.Complete();
            }

            return result;
        }
    }
}
