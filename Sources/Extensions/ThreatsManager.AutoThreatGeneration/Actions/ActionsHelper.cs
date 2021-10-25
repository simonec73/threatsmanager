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

        private class ThreatTypeInfo
        {
            public ThreatTypeInfo([NotNull] IThreatType threatType, [NotNull] SelectionRule rule, bool topOnly)
            {
                ThreatType = threatType;
                Rule = rule;
                TopOnly = topOnly;
            }

            public IThreatType ThreatType { get; private set; }
            public SelectionRule Rule { get; private set; }
            public bool TopOnly { get; private set; }
        }

        #region Public functions.
        public static bool GenerateThreatEvents(this IThreatModel model, bool topOnly, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            var result = false;

            if (schemaManager == null)
                schemaManager = new AutoGenRulesPropertySchemaManager(model);
            var threatTypesWithRules = GetThreatTypesWithRules(model, topOnly, schemaManager)?.ToArray();
            if (threatTypesWithRules?.Any() ?? false)
            {
                ApplyThreatTypes(model, threatTypesWithRules, schemaManager);

                var entities = model.Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        ApplyThreatTypes(entity, threatTypesWithRules, schemaManager);
                    }
                }
                var dataFlows = model.DataFlows?.ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        ApplyThreatTypes(dataFlow, threatTypesWithRules, schemaManager);
                    }
                }

                result = true;
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IEntity entity, bool topOnly, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            bool result = false;

            if (entity.Model is IThreatModel model)
            {
                if (schemaManager == null)
                    schemaManager = new AutoGenRulesPropertySchemaManager(model);
                var threatTypesWithRules = GetThreatTypesWithRules(model, topOnly, schemaManager)?.ToArray();
                if (threatTypesWithRules?.Any() ?? false)
                {
                    ApplyThreatTypes(entity, threatTypesWithRules, schemaManager);
                    result = true;
                }
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IDataFlow flow, bool topOnly, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            bool result = false;

            if (flow.Model is IThreatModel model)
            {
                if (schemaManager == null)
                    schemaManager = new AutoGenRulesPropertySchemaManager(model);
                var threatTypesWithRules = GetThreatTypesWithRules(model, topOnly, schemaManager)?.ToArray();
                if (threatTypesWithRules?.Any() ?? false)
                {
                    ApplyThreatTypes(flow, threatTypesWithRules, schemaManager);
                    result = true;
                }
            }

            return result;
        }

        public static bool ApplyMitigations(this IThreatEvent threatEvent, bool topOnly, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            bool result = false;

            if (threatEvent.ThreatType is IThreatType threatType && 
                threatEvent.Model is IThreatModel model &&
                threatEvent.Parent is IIdentity identity)
            {
                if (schemaManager == null)
                    schemaManager = new AutoGenRulesPropertySchemaManager(model);

                var mitigations = threatType.Mitigations?
                    .Where(x => !topOnly || schemaManager.IsTop(x))
                    .ToArray();
                if (mitigations?.Any() ?? false)
                {
                    ISeverity maximumSeverity = null;

                    foreach (var mitigation in mitigations)
                    {
                        var rule = GetRule(mitigation);
                        if (rule?.Evaluate(identity) ?? false)
                        {
                            var strength = mitigation.Strength;

                            if (rule is MitigationSelectionRule mitigationRule)
                            {
                                if (mitigationRule.StrengthId.HasValue &&
                                    model.GetStrength(mitigationRule.StrengthId.Value) is IStrength strengthOverride)
                                    strength = strengthOverride;

                                var status = mitigationRule.Status ?? MitigationStatus.Undefined;
                                var generated = (threatEvent.AddMitigation(mitigation.Mitigation, strength, status) != null);
                                result |= generated;

                                if (generated && mitigationRule.SeverityId.HasValue &&
                                    model.GetSeverity(mitigationRule.SeverityId.Value) is ISeverity severity &&
                                    (maximumSeverity == null || maximumSeverity.Id > severity.Id))
                                {
                                    maximumSeverity = severity;
                                }
                            }
                            else
                            {
                                result |= (threatEvent.AddMitigation(mitigation.Mitigation, strength, MitigationStatus.Undefined) != null);
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

        public static SelectionRule GetRule([NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            return threatTypeMitigation.GetRule(threatTypeMitigation.Model);
        }

        public static SelectionRule GetRule([NotNull] IWeakness weakness)
        {
            return weakness.GetRule(weakness.Model);
        }

        public static SelectionRule GetRule([NotNull] IWeaknessMitigation weaknessMitigation)
        {
            return weaknessMitigation.GetRule(weaknessMitigation.Model);
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

        public static bool HasTop(this IThreatModel model, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            bool result = false;

            if (schemaManager == null)
                schemaManager = new AutoGenRulesPropertySchemaManager(model);

            var threatTypes = model.ThreatTypes?.Where(x => schemaManager.IsTop(x)).ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    if (threatType.Mitigations?.Any(x => schemaManager.IsTop(x)) ?? false)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public static bool HasTop(this IThreatEvent threatEvent, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            return threatEvent?.ThreatType?.HasTop(schemaManager) ?? false;
        }

        public static bool HasTop(this IThreatType threatType, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            bool result = false;

            if (schemaManager == null)
                schemaManager = new AutoGenRulesPropertySchemaManager(threatType.Model);

            if (schemaManager.IsTop(threatType))
            {
                if (threatType.Mitigations?.Any(x => schemaManager.IsTop(x)) ?? false)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool HasTop(this IThreatTypeMitigation mitigation, AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            if (schemaManager == null)
                schemaManager = new AutoGenRulesPropertySchemaManager(mitigation.Model);

            return schemaManager.IsTop(mitigation);
        }

        public static void SetTop(this IPropertiesContainer container, bool top,
            AutoGenRulesPropertySchemaManager schemaManager = null)
        {
            if (container is IThreatModelChild child && (container is IThreatType || container is IThreatTypeMitigation))
            {
                if (schemaManager == null)
                    schemaManager = new AutoGenRulesPropertySchemaManager(child.Model);

                schemaManager.SetTop(container, top);
            }
        }
        #endregion

        #region Private auxiliary functions.
        private static bool ApplyThreatTypes([NotNull] IIdentity identity, 
            [NotNull] IEnumerable<ThreatTypeInfo> threatTypesWithRules, [NotNull] AutoGenRulesPropertySchemaManager schemaManager)
        {
            bool result = false;

            foreach (var threatTypeWithRule in threatTypesWithRules)
            {
                result |= ApplyThreatType(identity, threatTypeWithRule, schemaManager);
            }

            return result;
        }

        private static bool ApplyThreatType([NotNull] IIdentity identity, [NotNull] ThreatTypeInfo tti, 
            [NotNull] AutoGenRulesPropertySchemaManager schemaManager)
        {
            bool result = false;

            if (tti.Rule.Evaluate(identity) && identity is IThreatEventsContainer container)
            {
                var threatEvent = container.AddThreatEvent(tti.ThreatType);
                if (threatEvent == null)
                {
                    threatEvent = container.ThreatEvents.FirstOrDefault(x => x.ThreatTypeId == tti.ThreatType.Id);
                }
                else
                {
                    result = true;
                }

                if (threatEvent != null)
                {
                    result |= threatEvent.ApplyMitigations(tti.TopOnly, schemaManager);
                }
            }

            return result;
        }

        private static IEnumerable<ThreatTypeInfo> GetThreatTypesWithRules(
            [NotNull] IThreatModel model, bool topOnly, [NotNull] AutoGenRulesPropertySchemaManager schemaManager)
        {
            IEnumerable<ThreatTypeInfo> result = null;

            var threatTypes = model.ThreatTypes?
                .Where(x => !topOnly || schemaManager.IsTop(x))
                .ToArray();
            if (threatTypes?.Any() ?? false)
            {
                List<ThreatTypeInfo> list = null;

                foreach (var threatType in threatTypes)
                {
                    var rule = GetRule(threatType);
                    if (rule != null)
                    {
                        if (list == null)
                            list = new List<ThreatTypeInfo>();
                        list.Add(new ThreatTypeInfo(threatType, rule, topOnly));
                    }
                }

                result = list?.AsReadOnly();
            }
            return result;
        }
        #endregion
    }
}
