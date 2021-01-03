using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.AutoGenRules.PropertySchemasUpdaters
{
    [Extension("B8EE3575-CB38-4E76-8CB8-8E37AF85C507", "Auto Gen Rules Property Schema Updater", 20, ExecutionMode.Simplified)]
    public class AutoGenRulesPropertySchemasUpdater : IPropertySchemasUpdater
    {
        public bool HasPropertySchema(IThreatModel model, string schemaName, string nsName)
        {
            bool result = false;

            if (model != null && !string.IsNullOrWhiteSpace(schemaName) && !string.IsNullOrWhiteSpace(nsName))
            {
                var propertyType = (new AutoGenRulesPropertySchemaManager(model)).GetPropertyType();

                result = HandleThreatTypes(model, schemaName, nsName, null, propertyType) &&
                         HandleEntities(model, schemaName, nsName, null, propertyType) &&
                         HandleFlows(model, schemaName, nsName, null, propertyType) &&
                         HandleGroups(model, schemaName, nsName, null, propertyType) &&
                         HandleDiagrams(model, schemaName, nsName, null, propertyType) &&
                         HandleMitigations(model, schemaName, nsName, null, propertyType) &&
                         HandleEntityTemplates(model, schemaName, nsName, null, propertyType) &&
                         HandleFlowTemplates(model, schemaName, nsName, null, propertyType) &&
                         HandleTrustBoundaryTemplates(model, schemaName, nsName, null, propertyType);
            }

            return result;
        }

        public bool HasPropertyType(IThreatModel model, string schemaName, string nsName, string propertyName)
        {
            bool result = false;

            if (model != null && !string.IsNullOrWhiteSpace(schemaName) && !string.IsNullOrWhiteSpace(nsName))
            {
                var propertyType = (new AutoGenRulesPropertySchemaManager(model)).GetPropertyType();

                result = HandleThreatTypes(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleEntities(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleFlows(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleGroups(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleDiagrams(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleMitigations(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleEntityTemplates(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleFlowTemplates(model, schemaName, nsName, propertyName, propertyType) &&
                         HandleTrustBoundaryTemplates(model, schemaName, nsName, propertyName, propertyType);
            }

            return result;
        }

        public bool UpdateSchemaName([NotNull] IThreatModel model,
            [Required] string oldName, [Required] string oldNamespace, 
            [Required] string newName, [Required] string newNamespace)
        {
            var propertyType = (new AutoGenRulesPropertySchemaManager(model)).GetPropertyType();

            return UpdateThreatTypes(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateEntities(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateFlows(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateGroups(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateDiagrams(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateMitigations(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateEntityTemplates(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateFlowTemplates(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType) &&
                     UpdateTrustBoundaryTemplates(model, oldName, oldNamespace, null, newName, newNamespace, null, propertyType);
        }

        public bool UpdatePropertyTypeName([NotNull] IThreatModel model,
            [Required] string schemaName, [Required] string schemaNamespace, 
            [Required] string oldPropertyTypeName, [Required] string newPropertyTypeName)
        {
            var propertyType = (new AutoGenRulesPropertySchemaManager(model)).GetPropertyType();

            return UpdateThreatTypes(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateEntities(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateFlows(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateGroups(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateDiagrams(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateMitigations(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateEntityTemplates(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateFlowTemplates(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType) &&
                   UpdateTrustBoundaryTemplates(model, schemaName, schemaNamespace, oldPropertyTypeName, schemaName, schemaNamespace, newPropertyTypeName, propertyType);
        }

        #region Verify
        private bool HandleThreatTypes([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject threatTypeProperty)
                    {
                        if (threatTypeProperty.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }

                        var mitigations = threatType.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject
                                    mitigationProperty)
                                {
                                    if (mitigationProperty.Value is SelectionRule mitigationSelectionRule)
                                    {
                                        if (string.IsNullOrWhiteSpace(propertyName))
                                            result = mitigationSelectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                                        else
                                            result = mitigationSelectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                                        if (result)
                                            break;
                                    }
                                }
                            }

                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleEntities([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var entities = model.Entities?.ToArray();

            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    if (entity.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;

                            var threatEvents = entity.ThreatEvents?.ToArray();
                            if (threatEvents?.Any() ?? false)
                            {
                                result = HandleThreatEvents(threatEvents, schemaName, nsName, propertyName, propertyType);
                                if (result)
                                    break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleFlows([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var flows = model.DataFlows?.ToArray();

            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    if (flow.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;

                            var threatEvents = flow.ThreatEvents?.ToArray();
                            if (threatEvents?.Any() ?? false)
                            {
                                result = HandleThreatEvents(threatEvents, schemaName, nsName, propertyName, propertyType);
                                if (result)
                                    break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleGroups([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var groups = model.Groups?.ToArray();

            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    if (group.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleDiagrams([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var diagrams = model.Diagrams?.ToArray();

            if (diagrams?.Any() ?? false)
            {
                foreach (var diagram in diagrams)
                {
                    if (diagram.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleMitigations([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var mitigations = model.Mitigations?.ToArray();

            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleEntityTemplates([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var entityTemplates = model.EntityTemplates?.ToArray();

            if (entityTemplates?.Any() ?? false)
            {
                foreach (var entityTemplate in entityTemplates)
                {
                    if (entityTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleFlowTemplates([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var flowTemplates = model.FlowTemplates?.ToArray();

            if (flowTemplates?.Any() ?? false)
            {
                foreach (var flowTemplate in flowTemplates)
                {
                    if (flowTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleTrustBoundaryTemplates([NotNull] IThreatModel model, 
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var trustBoundaryTemplates = model.TrustBoundaryTemplates?.ToArray();

            if (trustBoundaryTemplates?.Any() ?? false)
            {
                foreach (var trustBoundaryTemplate in trustBoundaryTemplates)
                {
                    if (trustBoundaryTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(propertyName))
                                result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                            else
                                result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                            if (result)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool HandleThreatEvents([NotNull] IEnumerable<IThreatEvent> threatEvents,
            [Required] string schemaName, [Required] string nsName, string propertyName, [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            foreach (var threatEvent in threatEvents)
            {
                if (threatEvent.GetProperty(propertyType) is IPropertyJsonSerializableObject threatTypeProperty)
                {
                    if (threatTypeProperty.Value is SelectionRule selectionRule)
                    {
                        if (string.IsNullOrWhiteSpace(propertyName))
                            result = selectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                        else
                            result = selectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                        if (result)
                            break;
                    }

                    var mitigations = threatEvent.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject mitigationProperty)
                            {
                                if (mitigationProperty.Value is SelectionRule mitigationSelectionRule)
                                {
                                    if (string.IsNullOrWhiteSpace(propertyName))
                                        result = mitigationSelectionRule.Root?.HasSchema(schemaName, nsName) ?? false;
                                    else
                                        result = mitigationSelectionRule.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                                    if (result)
                                        break;
                                }
                            }
                        }

                        if (result)
                            break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Update
        private bool UpdateThreatTypes([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject threatTypeProperty)
                    {
                        if (threatTypeProperty.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }

                        var mitigations = threatType.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject
                                    mitigationProperty)
                                {
                                    if (mitigationProperty.Value is SelectionRule mitigationSelectionRule)
                                    {
                                        if (string.IsNullOrWhiteSpace(oldPropertyName))
                                            result |= mitigationSelectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                                        else
                                            result |= mitigationSelectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateEntities([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var entities = model.Entities?.ToArray();

            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    if (entity.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;

                            var threatEvents = entity.ThreatEvents?.ToArray();
                            if (threatEvents?.Any() ?? false)
                            {
                                result |= UpdateThreatEvents(threatEvents, oldName, oldNamespace, oldPropertyName, newName, newNamespace, newPropertyName, propertyType);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateFlows([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var flows = model.DataFlows?.ToArray();

            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    if (flow.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;

                            var threatEvents = flow.ThreatEvents?.ToArray();
                            if (threatEvents?.Any() ?? false)
                            {
                                result |= UpdateThreatEvents(threatEvents, oldName, oldNamespace, oldPropertyName, newName, newNamespace, newPropertyName, propertyType);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateGroups([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var groups = model.Groups?.ToArray();

            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    if (group.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateDiagrams([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var diagrams = model.Diagrams?.ToArray();

            if (diagrams?.Any() ?? false)
            {
                foreach (var diagram in diagrams)
                {
                    if (diagram.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateMitigations([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var mitigations = model.Mitigations?.ToArray();

            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateEntityTemplates([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var entityTemplates = model.EntityTemplates?.ToArray();

            if (entityTemplates?.Any() ?? false)
            {
                foreach (var entityTemplate in entityTemplates)
                {
                    if (entityTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateFlowTemplates([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var flowTemplates = model.FlowTemplates?.ToArray();

            if (flowTemplates?.Any() ?? false)
            {
                foreach (var flowTemplate in flowTemplates)
                {
                    if (flowTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateTrustBoundaryTemplates([NotNull] IThreatModel model, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var trustBoundaryTemplates = model.TrustBoundaryTemplates?.ToArray();

            if (trustBoundaryTemplates?.Any() ?? false)
            {
                foreach (var trustBoundaryTemplate in trustBoundaryTemplates)
                {
                    if (trustBoundaryTemplate.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (property.Value is SelectionRule selectionRule)
                        {
                            if (string.IsNullOrWhiteSpace(oldPropertyName))
                                result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                            else
                                result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdateThreatEvents([NotNull] IEnumerable<IThreatEvent> threatEvents, 
            [Required] string oldName, [Required] string oldNamespace, string oldPropertyName, 
            [Required] string newName, [Required] string newNamespace, string newPropertyName, 
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            foreach (var threatEvent in threatEvents)
            {
                if (threatEvent.GetProperty(propertyType) is IPropertyJsonSerializableObject threatTypeProperty)
                {
                    if (threatTypeProperty.Value is SelectionRule selectionRule)
                    {
                        if (string.IsNullOrWhiteSpace(oldPropertyName))
                            result |= selectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                        else
                            result |= selectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                    }

                    var mitigations = threatEvent.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            if (mitigation.GetProperty(propertyType) is IPropertyJsonSerializableObject mitigationProperty)
                            {
                                if (mitigationProperty.Value is SelectionRule mitigationSelectionRule)
                                {
                                    if (string.IsNullOrWhiteSpace(oldPropertyName))
                                        result |= mitigationSelectionRule.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                                    else
                                        result |= mitigationSelectionRule.Root?.UpdatePropertyType(oldName, oldNamespace, oldPropertyName, newPropertyName) ?? false;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}