using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extensions used for cloning parts of the Threat Model.
    /// </summary>
    public static class CloningExtensions
    {
        /// <summary>
        /// Clone Properties between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CloneProperties(this IPropertiesContainer source, 
            [NotNull] IPropertiesContainer target)
        {
            var properties = source?.Properties?.ToArray();
            if ((properties?.Any() ?? false) &&
                target is IThreatModelChild child)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Properties"))
                {
                    if (child.Model is IThreatModel model &&
                        properties.FirstOrDefault()?.Model is IThreatModel sourceModel)
                    {
                        foreach (var property in properties)
                        {
                            if (property.PropertyType is IPropertyType sourcePropertyType &&
                                sourceModel.GetSchema(sourcePropertyType.SchemaId) is IPropertySchema sourceSchema)
                            {
                                if (model.GetSchema(sourceSchema.Name, sourceSchema.Namespace) is IPropertySchema targetSchema &&
                                    (model.Id == sourceModel.Id || !sourceSchema.NotExportable))
                                {
                                    var propertyType = targetSchema.GetPropertyType(sourcePropertyType.Name);
                                    if (propertyType != null)
                                    {
                                        var propertyTarget = target.GetProperty(propertyType);
                                        if (propertyTarget == null)
                                            propertyTarget = target.AddProperty(propertyType, property.StringValue);
                                        else
                                            propertyTarget.StringValue = property.StringValue;

                                        if (propertyTarget != null && sourceModel.Id != model.Id)
                                            propertyTarget.SetSourceInfo(model);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var property in properties)
                        {
                            if (property.PropertyType is IPropertyType propertyType)
                            {
                                var propertyTarget = target.GetProperty(propertyType);
                                if (propertyTarget == null)
                                    target.AddProperty(propertyType, property.StringValue);
                                else
                                    propertyTarget.StringValue = property.StringValue;
                            }
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Clone Threat Events between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CloneThreatEvents(this IThreatEventsContainer source,
            [NotNull] IThreatEventsContainer target)
        {
            var threatEvents = source?.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Threat Events"))
                {
                    foreach (var threatEvent in threatEvents)
                    {
                        threatEvent.Clone(target);
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Clone Vulnerabilities between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CloneVulnerabilities(this IVulnerabilitiesContainer source,
            [NotNull] IVulnerabilitiesContainer target)
        {
            var vulnerabilities = source?.Vulnerabilities?.ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Vulnerabilities"))
                {
                    foreach (var vulnerability in vulnerabilities)
                    {
                        vulnerability.Clone(target);
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Clone Property Types between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void ClonePropertyTypes(this IPropertyTypesContainer source,
            [NotNull] IPropertyTypesContainer target)
        {
            var propertyTypes = source?.PropertyTypes?.ToArray();
            if (propertyTypes?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property Types"))
                {
                    foreach (var propertyType in propertyTypes)
                    {
                        propertyType.Clone(target);
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Merge Property Types between Containers.
        /// </summary>
        /// <param name="target">Target Container.</param>
        /// <param name="source">Source Container.</param>
        public static void MergePropertyTypes(this IPropertyTypesContainer target,
            [NotNull] IPropertyTypesContainer source)
        {
            var propertyTypes = source?.PropertyTypes?.ToArray();
            if (propertyTypes?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Merge Property Types"))
                {
                    foreach (var propertyType in propertyTypes)
                    {
                        var targetPropertyType = target.GetPropertyType(propertyType.Name);
                        if (targetPropertyType == null)
                            propertyType.Clone(target);
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Clone Properties between Containers.
        /// </summary>
        /// <param name="target">Target Container.</param>
        /// <param name="source">Source Container.</param>
        public static void MergeProperties(this IPropertiesContainer target, 
            [NotNull] IPropertiesContainer source)
        {
            var properties = source?.Properties?.ToArray();
            if ((properties?.Any() ?? false) && 
                source is IThreatModelChild sourceChild &&
                sourceChild.Model is IThreatModel sourceModel &&
                target is IThreatModelChild targetChild &&
                targetChild.Model is IThreatModel targetModel)
            {
                using (var scope = UndoRedoManager.OpenScope("Merge Property Types"))
                {
                    foreach (var property in properties)
                    {
                        var sourcePropertyType = property.PropertyType;
                        if (sourcePropertyType != null)
                        {
                            var sourceSchema = sourceModel.GetSchema(sourcePropertyType.SchemaId);
                            if (sourceSchema != null)
                            {
                                var targetSchema = targetModel.GetSchema(sourceSchema.Name, sourceSchema.Namespace) ??
                                                   sourceSchema.Clone(targetModel);

                                var targetPropertyType = targetSchema.GetPropertyType(sourcePropertyType.Name) ??
                                                         sourcePropertyType.Clone(targetSchema);

                                var targetProperty = target.GetProperty(targetPropertyType);
                                if (targetProperty == null)
                                {
                                    var newProperty = target.AddProperty(targetPropertyType, property.StringValue);
                                    if (newProperty != null && sourceModel.Id != targetModel.Id)
                                        newProperty.SetSourceInfo(sourceModel);
                                }
                                else
                                {
                                    if (targetProperty is IPropertyJsonSerializableObject jsonSerializableObject &&
                                        jsonSerializableObject.Value is IMergeable mergeable &&
                                        property is IPropertyJsonSerializableObject sourceJsonSerializableObject &&
                                        sourceJsonSerializableObject.Value is IMergeable sourceMergeable)
                                    {
                                        mergeable.Merge(sourceMergeable);
                                    }
                                }
                            }
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Clone an Entity converting its type.
        /// </summary>
        /// <param name="source">Source entity to be cloned.</param>
        /// <param name="entityType">Target entity type.</param>
        /// <param name="template">[Optional] Template to use for the target entity.</param>
        /// <returns>Cloned and converting entity.</returns>
        public static IEntity CloneAndConvert(this IEntity source, 
            EntityType entityType, IEntityTemplate template = null)
        {
            IEntity result = null;

            if (source != null && source.Model is IThreatModel model)
            {
                var newName = $"{source.Name} (copy)";
                using (var scope = UndoRedoManager.OpenScope("Clone and convert Entity"))
                {
                    if (template != null && template.EntityType == entityType)
                    {
                        // Creation of a clone of the source entity eventually changing its type and applied template.

                        switch (entityType)
                        {
                            case EntityType.ExternalInteractor:
                                result = model.AddEntity<IExternalInteractor>(newName, template);
                                break;
                            case EntityType.Process:
                                result = model.AddEntity<IProcess>(newName, template);
                                break;
                            case EntityType.DataStore:
                                result = model.AddEntity<IDataStore>(newName, template);
                                break;
                        }
                    }
                    else
                    {
                        // Creation of a clone of the source entity eventually changing its type.
                        var existingTemplate = source.GetEntityType() == entityType ?
                            source.Template : null;

                        switch (entityType)
                        {
                            case EntityType.ExternalInteractor:
                                result = model.AddEntity<IExternalInteractor>(newName, existingTemplate);
                                break;
                            case EntityType.Process:
                                result = model.AddEntity<IProcess>(newName, existingTemplate);
                                break;
                            case EntityType.DataStore:
                                result = model.AddEntity<IDataStore>(newName, existingTemplate);
                                break;
                        }
                    }

                    if (result != null)
                    {
                        result.Description = source.Description;
                        if (source.Parent != null)
                            result.SetParent(source.Parent);
                        result.BigImage = source.BigImage;
                        result.Image = source.Image;
                        result.SmallImage = source.SmallImage;
                        source.CopyProperties(result);
                        source.CopyThreatEvents(result);
                        source.CopyVulnerabilities(result);
                        source.CopyFlows(result);

                        scope?.Complete();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copy Properties between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        /// <remarks>This function differs from <see cref="CloneProperties(IPropertiesContainer, IPropertiesContainer)"/>
        /// in that it checks if the target container is supported by the Property Schema, before copying the properties.</remarks>
        public static void CopyProperties(this IPropertiesContainer source,
            [NotNull] IPropertiesContainer target)
        {
            var properties = source?.Properties?.ToArray();
            if ((properties?.Any() ?? false) &&
                target is IThreatModelChild child)
            {
                if (child.Model is IThreatModel model &&
                    properties.FirstOrDefault()?.Model is IThreatModel sourceModel)
                {
                    using (var scope = UndoRedoManager.OpenScope("Copy Properties"))
                    {
                        foreach (var property in properties)
                        {
                            if (property.PropertyType is IPropertyType sourcePropertyType &&
                                sourceModel.GetSchema(sourcePropertyType.SchemaId) is IPropertySchema sourceSchema)
                            {
                                if (model.GetSchema(sourceSchema.Name, sourceSchema.Namespace) is IPropertySchema targetSchema &&
                                    (model.Id == sourceModel.Id || !sourceSchema.NotExportable) &&
                                    targetSchema.AppliesTo.HasFlag(target.PropertiesScope))
                                {
                                    var propertyType = targetSchema.GetPropertyType(sourcePropertyType.Name);
                                    if (propertyType != null)
                                    {
                                        var propertyTarget = target.GetProperty(propertyType);
                                        if (propertyTarget == null)
                                            target.AddProperty(propertyType, property.StringValue);
                                        else
                                            propertyTarget.StringValue = property.StringValue;
                                    }
                                }
                            }
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Copy Threat Events between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        private static void CopyThreatEvents(this IThreatEventsContainer source,
            [NotNull] IThreatEventsContainer target)
        {
            var threatEvents = source.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Copy Threat Events"))
                {
                    foreach (var threatEvent in threatEvents)
                    {
                        var newTe = target.AddThreatEvent(threatEvent.ThreatType);
                        if (newTe != null)
                        {
                            newTe.Name = threatEvent.Name;
                            newTe.Description = threatEvent.Description;
                            newTe.Severity = threatEvent.Severity;
                            threatEvent.CopyThreatEventMitigations(newTe);
                            threatEvent.CopyThreatEventScenarios(newTe);
                            threatEvent.CopyVulnerabilities(newTe);
                            threatEvent.CopyProperties(newTe);
                        }
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Copy Threat Event Mitigations between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CopyThreatEventMitigations(this IThreatEventMitigationsContainer source,
            [NotNull] IThreatEventMitigationsContainer target)
        {
            var mitigations = source.Mitigations?.ToArray();
            if (mitigations?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Copy Threat Event Mitigations"))
                {
                    foreach (var mitigation in mitigations)
                    {
                        var newM = target.AddMitigation(mitigation.Mitigation, mitigation.Strength, 
                            mitigation.Status, mitigation.Directives);
                        if (newM != null)
                        {
                            mitigation.CopyProperties(newM);
                        }
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Copy Threat Event Scenarios between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CopyThreatEventScenarios(this IThreatEventScenariosContainer source,
            [NotNull] IThreatEventScenariosContainer target)
        {
            var scenarios = source.Scenarios?.ToArray();
            if (scenarios?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Copy Threat Event Scenarios"))
                {
                    foreach (var scenario in scenarios)
                    {
                        var newS = target.AddScenario(scenario.Actor, scenario.Severity, scenario.Name);
                        if (newS != null)
                        {
                            newS.Description = scenario.Description;
                            newS.Motivation = scenario.Motivation;
                            scenario.CopyProperties(newS);
                        }
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Copy Vulnerabilities between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CopyVulnerabilities(this IVulnerabilitiesContainer source,
            [NotNull] IVulnerabilitiesContainer target)
        {
            var vulnerabilities = source.Vulnerabilities?.ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Copy Vulnerabilities"))
                {
                    foreach (var vulnerability in vulnerabilities)
                    {
                        var newVuln = target.AddVulnerability(vulnerability.Weakness);
                        if (newVuln != null)
                        {
                            newVuln.Name = vulnerability.Name;
                            newVuln.Description = vulnerability.Description;
                            newVuln.Severity = vulnerability.Severity;
                            vulnerability.CopyVulnerabilityMitigations(newVuln);
                            vulnerability.CopyProperties(newVuln);
                        }
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Copy Vulnerabilities Mitigations between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CopyVulnerabilityMitigations(this IVulnerabilityMitigationsContainer source,
            [NotNull] IVulnerabilityMitigationsContainer target)
        {
            var mitigations = source.Mitigations?.ToArray();
            if (mitigations?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Copy Vulnerability Mitigations"))
                {
                    foreach (var mitigation in mitigations)
                    {
                        var newM = target.AddMitigation(mitigation.Mitigation, mitigation.Strength,
                            mitigation.Status, mitigation.Directives);
                        if (newM != null)
                        {
                            mitigation.CopyProperties(newM);
                        }
                    }
                    scope?.Complete();
                }
            }
        }

        /// <summary>
        /// Copy all Flows involving the Source Entity to the Target Entity.
        /// </summary>
        /// <param name="source">Source Entity.</param>
        /// <param name="target">Target Entity</param>
        public static void CopyFlows(this IEntity source, [NotNull] IEntity target)
        {
            if (source != null && source.Model is IThreatModel model)
            {
                var outFlows = model.DataFlows?
                .Where(x => x.SourceId == source.Id)
                .ToArray();
                if (outFlows?.Any() ?? false)
                {
                    foreach (var flow in outFlows)
                    {
                        IDataFlow newFlow = null;
                        if (flow.Template == null)
                            newFlow = model.AddDataFlow(flow.Name, target.Id, flow.TargetId);
                        else
                            newFlow = model.AddDataFlow(flow.Name, target.Id, flow.TargetId, flow.Template);
                        if (newFlow != null)
                        {
                            newFlow.FlowType = flow.FlowType;
                            flow.CopyThreatEvents(newFlow);
                            flow.CopyVulnerabilities(newFlow);
                            flow.CopyProperties(newFlow);
                        }
                    }
                }

                var inFlows = model.DataFlows?
                    .Where(x => x.TargetId == source.Id)
                    .ToArray();
                if (inFlows?.Any() ?? false)
                {
                    foreach (var flow in inFlows)
                    {
                        IDataFlow newFlow = null;
                        if (flow.Template == null)
                            newFlow = model.AddDataFlow(flow.Name, flow.SourceId, target.Id);
                        else
                            newFlow = model.AddDataFlow(flow.Name, flow.SourceId, target.Id, flow.Template);
                        if (newFlow != null)
                        {
                            newFlow.FlowType = flow.FlowType;
                            flow.CopyThreatEvents(newFlow);
                            flow.CopyVulnerabilities(newFlow);
                            flow.CopyProperties(newFlow);
                        }
                    }
                }
            }
        }
    }
}
