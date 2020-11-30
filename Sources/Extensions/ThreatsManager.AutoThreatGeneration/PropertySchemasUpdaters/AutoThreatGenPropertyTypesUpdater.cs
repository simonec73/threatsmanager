using System.ComponentModel.Composition;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.AutoThreatGeneration.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoThreatGeneration.PropertySchemasUpdaters
{
    [Extension("B8EE3575-CB38-4E76-8CB8-8E37AF85C507", "Auto Threat Gen Property Schema Updater", 20, ExecutionMode.Simplified)]
    public class AutoThreatGenPropertyTypesUpdater : IPropertySchemasUpdater
    {
        public bool HasPropertySchema(IThreatModel model, string schemaName, string nsName)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var propertyType = (new AutoThreatGenPropertySchemaManager(model)).GetPropertyType();
                if (propertyType != null)
                {
                    foreach (var threatType in threatTypes)
                    {
                        if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject
                            threatTypeProperty)
                        {
                            if (threatTypeProperty.Value is SelectionRule selectionRule)
                            {
                                result |= selectionRule.Root.HasSchema(schemaName, nsName);
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
                                            result |= mitigationSelectionRule.Root.HasSchema(schemaName, nsName);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return result;
        }

        public bool HasPropertyType(IThreatModel model, string schemaName, string nsName, string propertyName)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var propertyType = (new AutoThreatGenPropertySchemaManager(model)).GetPropertyType();
                if (propertyType != null)
                {
                    foreach (var threatType in threatTypes)
                    {
                        if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject
                            threatTypeProperty)
                        {
                            if (threatTypeProperty.Value is SelectionRule selectionRule)
                            {
                                result |= selectionRule.Root
                                    .HasPropertyType(schemaName, nsName, propertyName);
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
                                            result |= mitigationSelectionRule.Root
                                                .HasPropertyType(schemaName, nsName, propertyName);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return result;
        }

        public bool UpdateSchemaName([NotNull] IThreatModel model,
            [Required] string oldName, [Required] string oldNamespace, 
            [Required] string newName, [Required] string newNamespace)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var propertyType = (new AutoThreatGenPropertySchemaManager(model)).GetPropertyType();
                if (propertyType != null)
                {
                    foreach (var threatType in threatTypes)
                    {
                        if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject
                            threatTypeProperty)
                        {
                            if (threatTypeProperty.Value is SelectionRule selectionRule)
                            {
                                result |= selectionRule.Root.UpdateSchema(oldName, oldNamespace, newName, newNamespace);
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
                                            result |= mitigationSelectionRule.Root.UpdateSchema(oldName, oldNamespace,
                                                newName, newNamespace);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return result;
        }

        public bool UpdatePropertyTypeName([NotNull] IThreatModel model,
            [Required] string schemaName, [Required] string schemaNamespace, 
            [Required] string oldPropertyTypeName, [Required] string newPropertyTypeName)
        {
            bool result = false;

            var threatTypes = model.ThreatTypes?.ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var propertyType = (new AutoThreatGenPropertySchemaManager(model)).GetPropertyType();
                if (propertyType != null)
                {
                    foreach (var threatType in threatTypes)
                    {
                        if (threatType.GetProperty(propertyType) is IPropertyJsonSerializableObject
                            threatTypeProperty)
                        {
                            if (threatTypeProperty.Value is SelectionRule selectionRule)
                            {
                                result |= selectionRule.Root
                                    .UpdatePropertyType(schemaName, schemaNamespace, oldPropertyTypeName, newPropertyTypeName);
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
                                            result |= mitigationSelectionRule.Root
                                                .UpdatePropertyType(schemaName, schemaNamespace, oldPropertyTypeName, newPropertyTypeName);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return result;
        }
    }
}