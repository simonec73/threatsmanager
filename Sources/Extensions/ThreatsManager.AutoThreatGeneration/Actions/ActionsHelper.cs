using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    public static class ActionsHelper
    {
        private const string OldSchemaName = "Automatic Threat Event Generation Extension Configuration";
        private const string OldPropertyName = "AutoGenRule";

        public static bool GenerateThreatEvents(this IThreatModel model)
        {
            var result = false;

            var threatTypesWithRules = GetThreatTypesWithRules(model)?.ToArray();
            if (threatTypesWithRules?.Any() ?? false)
            {
                ApplyThreatTypes(model, threatTypesWithRules);

                var entities = model.Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        ApplyThreatTypes(entity, threatTypesWithRules);
                    }
                }
                var dataFlows = model.DataFlows?.ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        ApplyThreatTypes(dataFlow, threatTypesWithRules);
                    }
                }

                result = true;
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IEntity entity)
        {
            bool result = false;

            if (entity.Model is IThreatModel model)
            {
                var threatTypesWithRules = GetThreatTypesWithRules(model)?.ToArray();
                if (threatTypesWithRules?.Any() ?? false)
                {
                    ApplyThreatTypes(entity, threatTypesWithRules);
                    result = true;
                }
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IDataFlow flow)
        {
            bool result = false;

            if (flow.Model is IThreatModel model)
            {
                var threatTypesWithRules = GetThreatTypesWithRules(model)?.ToArray();
                if (threatTypesWithRules?.Any() ?? false)
                {
                    ApplyThreatTypes(flow, threatTypesWithRules);
                    result = true;
                }
            }

            return result;
        }

        private static bool ApplyThreatTypes([NotNull] IIdentity identity, 
            [NotNull] IEnumerable<KeyValuePair<IThreatType, SelectionRule>> threatTypesWithRules)
        {
            bool result = false;

            foreach (var threatTypeWithRule in threatTypesWithRules)
            {
                result |= ApplyThreatType(identity, threatTypeWithRule.Key, threatTypeWithRule.Value);
            }

            return result;
        }

        private static bool ApplyThreatType([NotNull] IIdentity identity,
            [NotNull] IThreatType threatType, [NotNull] SelectionRule rule)
        {
            bool result = false;

            if (rule.Evaluate(identity) && identity is IThreatEventsContainer container)
            {
                var threatEvent = container.AddThreatEvent(threatType);
                if (threatEvent == null)
                {
                    threatEvent = container.ThreatEvents.FirstOrDefault(x => x.ThreatTypeId == threatType.Id);
                }
                else
                {
                    result = true;
                }

                if (threatEvent != null)
                {
                    result |= threatEvent.ApplyMitigations();
                }
            }

            return result;
        }

        public static bool ApplyMitigations(this IThreatEvent threatEvent)
        {
            bool result = false;

            if (threatEvent.ThreatType is IThreatType threatType && threatEvent.Model is IThreatModel model &&
                threatEvent.Parent is IIdentity identity)
            {
                var mitigations = threatType.Mitigations?.ToArray();
                if (mitigations?.Any() ?? false)
                {
                    ISeverity maximumSeverity = null;
                    var generated = false;

                    foreach (var mitigation in mitigations)
                    {
                        var rule = GetRule(mitigation);
                        if (rule?.Evaluate(identity) ?? false)
                        {
                            var strength = mitigation.Strength;
                            if (rule.StrengthId.HasValue &&
                                model.GetStrength(rule.StrengthId.Value) is IStrength strengthOverride)
                                strength = strengthOverride;

                            if (rule.Status.HasValue)
                                generated = (threatEvent.AddMitigation(mitigation.Mitigation, strength,
                                                 rule.Status.Value) !=
                                             null);
                            else
                                generated = (threatEvent.AddMitigation(mitigation.Mitigation, strength) !=
                                             null);
                            result |= generated;

                            if (generated && rule.SeverityId.HasValue &&
                                model.GetSeverity(rule.SeverityId.Value) is ISeverity severity &&
                                (maximumSeverity == null || maximumSeverity.Id > severity.Id))
                            {
                                maximumSeverity = severity;
                            }
                        }
                    }

                    if (maximumSeverity != null && maximumSeverity.Id < threatEvent.SeverityId)
                    {
                        threatEvent.Severity = maximumSeverity;
                    }
                }
            }

            return result;
        }

        public static SelectionRule GetRule([NotNull] IThreatType threatType)
        {
            return threatType.GetRule(threatType.Model);
        }

        public static MitigationSelectionRule GetRule([NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            return threatTypeMitigation.GetRule(threatTypeMitigation.Model) as MitigationSelectionRule;
        }

        public static SelectionRule GetRule([NotNull] IWeakness weakness)
        {
            return weakness.GetRule(weakness.Model);
        }

        public static MitigationSelectionRule GetRule([NotNull] IWeaknessMitigation weaknessMitigation)
        {
            return weaknessMitigation.GetRule(weaknessMitigation.Model) as MitigationSelectionRule;
        }

        public static SelectionRule GetRule(this IPropertiesContainer container, IThreatModel model = null)
        {
            SelectionRule result = null;

            if (model == null && container is IThreatModelChild child)
                model = child.Model;

            if (model != null)
            {
                var schemaManager = new AutoGenRulesPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                if (container.HasProperty(propertyType))
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject)
                    {
                        result = jsonSerializableObject.Value as SelectionRule;
                    }
                }
                else
                {
                    var oldSchema = model.GetSchema(OldSchemaName, Properties.Resources.DefaultNamespace);
                    var oldPropertyType = oldSchema?.GetPropertyType(OldPropertyName);
                    if (oldPropertyType != null)
                    {
                        if (container.GetProperty(oldPropertyType) is IPropertyJsonSerializableObject
                            jsonSerializableObject)
                        {
                            result = jsonSerializableObject.Value as SelectionRule;
                            container.AddProperty(propertyType, jsonSerializableObject.StringValue);
                            container.RemoveProperty(oldPropertyType);
                        }
                    }
                }
            }

            return result;
        }

        public static void SetRule(this IPropertiesContainer container, SelectionRule rule, IThreatModel model = null)
        {
            if (model == null && container is IThreatModelChild child)
                model = child.Model;

            if (model != null)
            {
                var schemaManager = new AutoGenRulesPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                var property = container.GetProperty(propertyType) ?? container.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    jsonSerializableObject.Value = rule;
                }
            }
        }

        private static IEnumerable<KeyValuePair<IThreatType, SelectionRule>> GetThreatTypesWithRules(
            [NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<IThreatType, SelectionRule>> result = null;

            var threatTypes = model.ThreatTypes?.ToArray();
            if (threatTypes?.Any() ?? false)
            {
                List<KeyValuePair<IThreatType, SelectionRule>> list = null;

                foreach (var threatType in threatTypes)
                {
                    var rule = GetRule(threatType);
                    if (rule != null)
                    {
                        if (list == null)
                            list = new List<KeyValuePair<IThreatType, SelectionRule>>();
                        list.Add(new KeyValuePair<IThreatType, SelectionRule>(threatType, rule));
                    }
                }

                result = list?.AsReadOnly();
            }
            return result;
        }
    }
}
