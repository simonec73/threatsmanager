using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.MsTmt.Dialogs;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.MsTmt.Extensions
{
#pragma warning disable CS0067
    [Extension("C2564246-BE5D-4C8E-A19C-E0D2CEA1BF20", "Merge Flows Context Aware Action", 21, ExecutionMode.Simplified)]
    public class MergeFlows : IShapesContextAwareAction, 
        IDataFlowAddingRequiredAction, IDataFlowRemovingRequiredAction,
        ICommandsBarContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity | Scope.DataFlow;
        public string Label => "Merge Flows";
        public string Group => "Merge";
        public Bitmap Icon => Properties.Resources.arrows_merge_big;
        public Bitmap SmallIcon => Properties.Resources.arrows_merge;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<IDiagram, IDataFlow> DataFlowAddingRequired;
        public event Action<ILink> DataFlowRemovingRequired;
        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute([NotNull] object item)
        {
            return false;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IEnumerable<IShape> shapes, [NotNull] IEnumerable<ILink> links)
        {
            bool result = false;

            var entities = shapes.Select(x => x.Identity).OfType<IEntity>().ToArray();
            var flows = links.Select(x => x.DataFlow).ToArray();
            if (entities.Any() || flows.Any())
            {
                using (var dialog = new FlowMergeDialog())
                {
                    if (dialog.Initialize(entities, flows))
                    {
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var merges = dialog.Merges?.ToArray();
                            if (merges?.Any() ?? false)
                            {
                                foreach (var merge in merges)
                                {
                                    foreach (var sourceFlow in merge.Slaves)
                                    {
                                        CopyFlowDetails(sourceFlow, merge.Master);
                                        merge.Master.Model?.RemoveDataFlow(sourceFlow.Id);
                                    }

                                    merge.Master.Name = merge.Name;
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowWarning?.Invoke("The selection does not include any Flows which could be merged.");
                    }
                }
            }

            return result;
        }

        private void CopyFlowDetails([NotNull] IDataFlow source, [NotNull] IDataFlow target)
        {
            target.FlowType = source.FlowType;
            target.Description = source.Description;

            ApplyProperties(source, target);

            var threatEvents = source.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    ApplyThreatEvent(target, threatEvent);
                }
            }
        }

        private void ApplyThreatEvent([NotNull] IDataFlow target, [NotNull] IThreatEvent threatEvent)
        {
            var newEvent = target.AddThreatEvent(threatEvent.ThreatType);
            if (newEvent != null)
            {
                newEvent.Name = threatEvent.Name;
                newEvent.Description = threatEvent.Description;

                ApplyProperties(threatEvent, newEvent);

                var scenarios = threatEvent.Scenarios?.ToArray();
                if (scenarios?.Any() ?? false)
                {
                    foreach (var scenario in scenarios)
                    {
                        var newScenario = newEvent.AddScenario(scenario.Actor, scenario.Severity, scenario.Name);
                        newScenario.Description = scenario.Description;
                        ApplyProperties(scenario, newScenario);
                    }
                }

                var mitigations = threatEvent.Mitigations?.ToArray();
                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        var newMitigation = newEvent.AddMitigation(mitigation.Mitigation, mitigation.Strength, mitigation.Status);
                        newMitigation.Directives = mitigation.Directives;
                        ApplyProperties(mitigation, newMitigation);
                    }
                }
            }
        }

        private void ApplyProperties([NotNull] IPropertiesContainer source, [NotNull] IPropertiesContainer target)
        {
            var properties = source.Properties?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if (target.Properties?.All(x => x.PropertyTypeId != property.PropertyTypeId) ?? true)
                        target.AddProperty(property.PropertyType, property.StringValue);
                }
            }
        }

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        });
    }
}