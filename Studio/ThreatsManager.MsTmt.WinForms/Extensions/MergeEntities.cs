using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
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
    [Extension("C40C1DBE-7856-434C-9023-4EF8686E08BC", "Merge Entities Context Aware Action", 20, ExecutionMode.Simplified)]
    public class MergeEntities : IShapesContextAwareAction, 
        IDataFlowAddingRequiredAction, IDataFlowRemovingRequiredAction,
        ICommandsBarContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Merge Entities";
        public string Group => "Merge";
        public Bitmap Icon => Properties.Resources.logic_or_big;
        public Bitmap SmallIcon => Properties.Resources.logic_or;
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
            if (entities.Any())
            {
                using (var dialog = new EntityMergeDialog())
                {
                    dialog.Initialize(entities);
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var target = dialog.Target;
                        var sources = dialog.Sources?.ToArray();
                        var strategy = dialog.Strategy;

                        if (target != null && target.Model is IThreatModel model &&
                            (sources?.Any() ?? false) && Validate(target, sources, model, strategy))
                        {
                            var targetShape = shapes.FirstOrDefault(x => x.AssociatedId == target.Id);

                            var diagram =
                                model.Diagrams?.FirstOrDefault(x => x.GetEntityShape(target.Id) == targetShape);

                            // Merge is about moving the Data Flows, nothing else.
                            HandleIncoming(target, sources, model, diagram, strategy);
                            HandleOutgoing(target, sources, model, diagram, strategy);

                            foreach (var source in sources)
                            {
                                model.RemoveEntity(source.Id);
                            }

                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private bool Validate([NotNull] IEntity target, [NotNull] IEnumerable<IEntity> sources, 
            [NotNull] IThreatModel model, ReplacementStrategy strategy)
        {
            bool result = false;

            var cycles = model.DataFlows.Where(x => (sources.Any(y => y.Id == x.TargetId) || x.TargetId == target.Id) &&
                                      (sources.Any(y => y.Id == x.SourceId) || x.SourceId == target.Id));

            if (cycles.Any())
            {
                ShowWarning?.Invoke($"Unable to merge, because {cycles.Count()} Flows would create cycles, including '{cycles.First().Name}'");
            }
            else if (strategy == ReplacementStrategy.Stop)
            {
                var incoming = model.DataFlows.Where(x => sources.Any(y => y.Id == x.TargetId));
                var inExisting = model.DataFlows.Where(x => x.TargetId == target.Id && incoming.Any(y => y.SourceId == x.SourceId));
                if (inExisting.Any())
                {
                    ShowWarning?.Invoke($"Unable to merge, because {inExisting.Count()} incoming Flows already exist, including '{inExisting.First().Name}'");
                }
                else
                {
                    var outgoing = model.DataFlows.Where(x => sources.Any(y => y.Id == x.SourceId));
                    var outExisting = model.DataFlows.Where(x => x.SourceId == target.Id && outgoing.Any(y => y.TargetId == x.TargetId));
                    if (outExisting.Any())
                    {
                        ShowWarning?.Invoke($"Unable to merge, because {outExisting.Count()} outgoing Flows already exist, including '{outExisting.First().Name}'");
                    }
                    else
                    {
                        result = true;
                    }                    
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        private void HandleIncoming([NotNull] IEntity target, [NotNull] IEnumerable<IEntity> sources, 
            [NotNull] IThreatModel model, IDiagram diagram, ReplacementStrategy strategy)
        {
            var incoming = model.DataFlows.Where(x => sources.Any(y => y.Id == x.TargetId)).ToArray();

            if (incoming.Any())
            {
                foreach (var flow in incoming)
                {
                    var existing = model.GetDataFlow(flow.SourceId, target.Id);
                    HandleIncomingDataFlow(target, model, diagram, strategy, flow, existing);
                }
            }
        }

        private void HandleOutgoing([NotNull] IEntity target, [NotNull] IEnumerable<IEntity> sources, 
            [NotNull] IThreatModel model, IDiagram diagram, ReplacementStrategy strategy)
        {
            var outgoing = model.DataFlows.Where(x => sources.Any(y => y.Id == x.SourceId)).ToArray();

            if (outgoing.Any())
            {
                foreach (var flow in outgoing)
                {
                    var existing = model.GetDataFlow(target.Id, flow.TargetId);
                    HandleOutgoingDataFlow(target, model, diagram, strategy, flow, existing);
                }
            }
        }

        private void HandleIncomingDataFlow(IEntity target, IThreatModel model, IDiagram diagram, ReplacementStrategy strategy, IDataFlow flow, IDataFlow existing)
        {
            if (existing != null)
            {
                switch (strategy)
                {
                    case ReplacementStrategy.Stop:
                        // This should not happen!
                        throw new InvalidOperationException("Replacement Strategy is Stop.");
                    case ReplacementStrategy.Replace:
                        model.RemoveDataFlow(existing.Id);
                        MoveIncomingDataFlow(target, model, diagram, flow);
                        break;
                    case ReplacementStrategy.Skip:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
                }
            }
            else
            {
                MoveIncomingDataFlow(target, model, diagram, flow);
            }
        }
        
        private void HandleOutgoingDataFlow(IEntity target, IThreatModel model, IDiagram diagram, ReplacementStrategy strategy, IDataFlow flow, IDataFlow existing)
        {
            if (existing != null)
            {
                switch (strategy)
                {
                    case ReplacementStrategy.Stop:
                        // This should not happen!
                        throw new InvalidOperationException("Replacement Strategy is Stop.");
                    case ReplacementStrategy.Replace:
                        model.RemoveDataFlow(existing.Id);
                        MoveOutgoingDataFlow(target, model, diagram, flow);
                        break;
                    case ReplacementStrategy.Skip:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
                }
            }
            else
            {
                MoveOutgoingDataFlow(target, model, diagram, flow);
            }
        }

        private void MoveIncomingDataFlow(IEntity target, IThreatModel model, IDiagram diagram, IDataFlow flow)
        {
            var newFlow = model.AddDataFlow(flow.Name, flow.SourceId, target.Id);
            CopyFlowDetails(flow, newFlow);
            if (diagram != null)
                DataFlowAddingRequired?.Invoke(diagram, newFlow);
            model.RemoveDataFlow(flow.Id);
        }

        private void MoveOutgoingDataFlow(IEntity target, IThreatModel model, IDiagram diagram, IDataFlow flow)
        {
            var newFlow = model.AddDataFlow(flow.Name, target.Id, flow.TargetId);
            CopyFlowDetails(flow, newFlow);
            if (diagram != null)
                DataFlowAddingRequired?.Invoke(diagram, newFlow);
            model.RemoveDataFlow(flow.Id);
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