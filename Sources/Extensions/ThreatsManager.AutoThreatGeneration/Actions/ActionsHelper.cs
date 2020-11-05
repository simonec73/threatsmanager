using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.AutoThreatGeneration.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    public static class ActionsHelper
    {
        public static bool GenerateThreatEvents(this IThreatModel model)
        {
            bool result = false;

            var schemaManager = new AutoThreatGenPropertySchemaManager(model);
            var propertyType = schemaManager.GetPropertyType();
            if (propertyType is IJsonSerializableObjectPropertyType jsonPropertyType)
            {
                var threatTypes = model.ThreatTypes?.Where(x => x.HasProperty(jsonPropertyType)).ToArray();
                if (threatTypes?.Any() ?? false)
                {
                    ApplyThreatTypes(model, threatTypes, jsonPropertyType);

                    var entities = model.Entities?.ToArray();
                    if (entities?.Any() ?? false)
                    {
                        foreach (var entity in entities)
                        {
                            ApplyThreatTypes(entity, threatTypes, jsonPropertyType);
                        }
                    }
                    var dataFlows = model.DataFlows?.ToArray();
                    if (dataFlows?.Any() ?? false)
                    {
                        foreach (var dataFlow in dataFlows)
                        {
                            ApplyThreatTypes(dataFlow, threatTypes, jsonPropertyType);
                        }
                    }

                    result = true;
                }
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IEntity entity)
        {
            bool result = false;

            if (entity.Model is IThreatModel model)
            {
                var schemaManager = new AutoThreatGenPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                if (propertyType is IJsonSerializableObjectPropertyType jsonPropertyType)
                {
                    var threatTypes = model.ThreatTypes?.Where(x => x.HasProperty(jsonPropertyType)).ToArray();
                    if (threatTypes?.Any() ?? false)
                    {
                        ApplyThreatTypes(entity, threatTypes, jsonPropertyType);

                        result = true;
                    }
                }
            }

            return result;
        }
        
        public static bool GenerateThreatEvents(this IDataFlow flow)
        {
            bool result = false;

            if (flow.Model is IThreatModel model)
            {
                var schemaManager = new AutoThreatGenPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                if (propertyType is IJsonSerializableObjectPropertyType jsonPropertyType)
                {
                    var threatTypes = model.ThreatTypes?.Where(x => x.HasProperty(jsonPropertyType)).ToArray();
                    if (threatTypes?.Any() ?? false)
                    {
                        ApplyThreatTypes(flow, threatTypes, jsonPropertyType);

                        result = true;
                    }
                }
            }

            return result;
        }

        private static bool ApplyThreatTypes([NotNull] IIdentity identity, 
            [NotNull] IEnumerable<IThreatType> threatTypes,
            [NotNull] IJsonSerializableObjectPropertyType propertyType)
        {
            bool result = false;

            foreach (var threatType in threatTypes)
            {
                result |= ApplyThreatType(identity, threatType, propertyType);
            }

            return result;
        }

        private static bool ApplyThreatType([NotNull] IIdentity identity,
            [NotNull] IThreatType threatType, [NotNull] IJsonSerializableObjectPropertyType propertyType)
        {
            bool result = false;

            var property = threatType.GetProperty(propertyType);
            if (property is IPropertyJsonSerializableObject jsonProperty &&
                jsonProperty.Value is SelectionRule rule && rule.Evaluate(identity) &&
                identity is IThreatEventsContainer container)
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
                    result |= threatEvent.ApplyMitigations(propertyType);
                }
            }

            return result;
        }

        public static bool ApplyMitigations(this IThreatEvent threatEvent, 
            IJsonSerializableObjectPropertyType propertyType = null)
        {
            bool result = false;

            if (threatEvent.ThreatType is IThreatType threatType && threatEvent.Model is IThreatModel model &&
                threatEvent.Parent is IIdentity identity)
            {
                if (propertyType == null)
                {
                    var schemaManager = new AutoThreatGenPropertySchemaManager(model);
                    propertyType = schemaManager.GetPropertyType() as IJsonSerializableObjectPropertyType;
                }

                if (propertyType != null)
                {
                    var mitigations = threatType.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        ISeverity maximumSeverity = null;
                        var generated = false;

                        foreach (var mitigation in mitigations)
                        {
                            var mProperty = mitigation.GetProperty(propertyType);
                            if (mProperty is IPropertyJsonSerializableObject jsonMProperty &&
                                jsonMProperty.Value is MitigationSelectionRule mRule && mRule.Evaluate(identity))
                            {
                                var strength = mitigation.Strength;
                                if (mRule.StrengthId.HasValue &&
                                    model.GetStrength(mRule.StrengthId.Value) is IStrength strengthOverride)
                                    strength = strengthOverride;

                                if (mRule.Status.HasValue)
                                    generated = (threatEvent.AddMitigation(mitigation.Mitigation, strength,
                                                     mRule.Status.Value) !=
                                                 null);
                                else
                                    generated = (threatEvent.AddMitigation(mitigation.Mitigation, strength) !=
                                                 null);
                                result |= generated;

                                if (generated && mRule.SeverityId.HasValue &&
                                    model.GetSeverity(mRule.SeverityId.Value) is ISeverity severity &&
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
            }

            return result;
        }
    }
}
