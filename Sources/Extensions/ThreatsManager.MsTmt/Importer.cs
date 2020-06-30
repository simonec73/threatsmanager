using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.MsTmt.Extensions;
using ThreatsManager.MsTmt.Model;
using ThreatsManager.MsTmt.Schemas;
using ThreatsManager.Utilities;
using Scope = ThreatsManager.Interfaces.Scope;

namespace ThreatsManager.MsTmt
{
    /// <summary>
    /// Main Importer class.
    /// </summary>
    public class Importer
    { 
        /// <summary>
        /// Import a TMT file inside a Threat Model.
        /// </summary>
        /// <param name="threatModel">Destination Threat Model.</param>
        /// <param name="fileName">Name of the TMT file to be imported.</param>
        /// <param name="dpiFactor">Factor for the Dpi representation. 1 represents the standard 96 dpi, 2 doubles it.</param>
        /// <param name="unassignedThreatHandler">Function to be manage custom Threats, that have not been assigned to any flow.
        /// <para>If null, the custom Threats will be associated to the Threat Model itself.</para></param>
        /// <param name="diagrams">[out] Counter of the Diagrams.</param>
        /// <param name="externalInteractors">[out] Counter of the External Interactors.</param>
        /// <param name="processes">[out] Counter of the Processes.</param>
        /// <param name="dataStores">[out] Counter of the Data Stores.</param>
        /// <param name="flows">[out] Counter of the Flows.</param>
        /// <param name="trustBoundaries">[out] Counter of the Trust Boundaries.</param>
        /// <param name="entityTypes">[out] Counter of the Entity Types.</param>
        /// <param name="threatTypes">[out] Counter of the Threat Types.</param>
        /// <param name="customThreatTypes">[out] Counter of the custom Threat Types, managed through the Unassigned Threat Handler.</param>
        /// <param name="threats">[out] Counter of the generated Threat Events.</param>
        /// <param name="missingThreats">[out] Counter of the Threat Events that has not been possible to generate.</param>
        public void Import([NotNull] IThreatModel threatModel, [Required] string fileName, float dpiFactor,
            Func<IThreatModel, Threat, IThreatType, IPropertySchema, bool> unassignedThreatHandler,
            out int diagrams, out int externalInteractors, out int processes, out int dataStores, out int flows,
            out int trustBoundaries, out int entityTypes, out int threatTypes, out int customThreatTypes, out int threats, 
            out int missingThreats)
        {
            var model = new ThreatModel(fileName);

            ImportModelInfo(model, threatModel);
            entityTypes = ImportEntityTemplates(model, threatModel);
            ImportElements(model, threatModel, dpiFactor, out diagrams, 
                out externalInteractors, out processes, out dataStores, out trustBoundaries);
            flows = ImportDataFlows(model, threatModel);
            ImportThreats(model, threatModel, unassignedThreatHandler, out threatTypes, out customThreatTypes, out threats, out missingThreats);
        }

        /// <summary>
        /// Auxiliary function to add properties from the Threat to the Threat Event.
        /// </summary>
        /// <param name="threatEvent">Threat Event to be updated.</param>
        /// <param name="threat">Source Threat.</param>
        /// <param name="schema">Property Schema for the new Property Types.</param>
        public static void AddProperties([NotNull] IThreatEvent threatEvent, 
            [NotNull] Threat threat, [NotNull] IPropertySchema schema)
        {
            var names = threat.ParameterNames?.ToArray();
            var labels = threat.ParameterLabels?.ToArray();

            if ((names?.Any() ?? false) && (labels?.Any() ?? false) && names.Length == labels.Length)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    var name = names[i];

                    switch (name)
                    {
                        case "Title":
                            threatEvent.Name = threat.GetValue(name);
                            break;
                        case "UserThreatDescription":
                            threatEvent.Description = threat.GetValue(name);
                            break;
                        case "Priority":
                            var severityValue = threat.GetValue(name);
                            if (!string.IsNullOrWhiteSpace(severityValue) &&
                                Enum.TryParse<DefaultSeverity>(severityValue, out var defaultSeverity))
                            {
                                var severity = threatEvent.Model?.GetMappedSeverity((int) defaultSeverity);
                                if (severity != null)
                                    threatEvent.Severity = severity;
                            }
                            break;
                        default:
                            var label = labels[i];
                            var propertyType = schema.GetPropertyType(label);
                            if (propertyType != null)
                            {
                                threatEvent.AddProperty(propertyType, threat.GetValueFromLabel(label));
                            }
                            break;
                    }
                }
            }
        }

        private void ImportModelInfo([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            target.Owner = source.Owner;
            target.Name = source.ThreatModelName;
            target.Description = source.HighLevelSystemDescription;
            var assumptions = source.Assumptions?.Trim(' ', '\r','\n').Split('\n');
            if (assumptions?.Any() ?? false)
            {
                foreach (var assumption in assumptions)
                {
                    if (!string.IsNullOrWhiteSpace(assumption.Trim('\r')))
                        target.AddAssumption(assumption.Trim(' ', '\r'));
                }
            }
            var dependencies = source.ExternalDependencies?.Trim(' ', '\r','\n').Split('\n');
            if (dependencies?.Any() ?? false)
            {
                foreach (var dependency in dependencies)
                {
                    if (!string.IsNullOrWhiteSpace(dependency.Trim('\r')))
                        target.AddDependency(dependency.Trim(' ', '\r'));
                }
            }
            var contributors = source.Contributors?.Trim(' ', '\r','\n').Split(',', ';');
            if (contributors?.Any() ?? false)
            {
                foreach (var contributor in contributors)
                {
                    if (!string.IsNullOrWhiteSpace(contributor.Trim('\r')))
                        target.AddContributor(contributor.Trim(' ', '\r'));
                }
            }
        }

        private int ImportEntityTemplates([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;
            var elements = source.ElementTypes?
                .ToArray();

            if (elements?.Any() ?? false)
            {
                var schemaManager = new ObjectPropertySchemaManager(target);
                var schema = schemaManager.GetSchema();

                IEntityTemplate entityTemplate;
                Interfaces.Scope scope;

                foreach (var element in elements)
                {
                    switch (element.ElementType)
                    {
                        case ElementType.StencilRectangle:
                            entityTemplate = target.AddEntityTemplate(element.Name, element.Description, element.Image, element.Image, element.Image,
                                EntityType.ExternalInteractor);
                            scope = Scope.ExternalInteractor;
                            result++;
                            break;
                        case ElementType.StencilEllipse:
                            entityTemplate = target.AddEntityTemplate(element.Name, element.Description, element.Image, element.Image, element.Image,
                                EntityType.Process);
                            scope = Scope.Process;
                            result++;
                            break;
                        case ElementType.StencilParallelLines:
                            entityTemplate = target.AddEntityTemplate(element.Name, element.Description, element.Image, element.Image, element.Image,
                                EntityType.DataStore);
                            result++;
                            scope = Scope.DataStore;
                            break;
                        default:
                            entityTemplate = null;
                            scope = Scope.Undefined;
                            break;
                    }

                    if (entityTemplate != null)
                    {
                        schemaManager.SetObjectId(entityTemplate, element.TypeId);
                        var properties = element.Properties?.ToArray();

                        var elementSchemaManager = new TmtPropertySchemaManager(target, element.Name, scope);
                        var elementSchema = elementSchemaManager.GetSchema();

                        var outOfScope = elementSchema.AddPropertyType("Out of Scope", PropertyValueType.Boolean);
                        entityTemplate.AddProperty(outOfScope, false.ToString());
                        var reasonOutOfScope =
                            elementSchema.AddPropertyType("Reason For Out Of Scope", PropertyValueType.String);

                        if (!string.IsNullOrWhiteSpace(element.ParentTypeId) &&
                            source.ElementTypes.FirstOrDefault(x =>
                                string.CompareOrdinal(x.TypeId, element.ParentTypeId) == 0) is ElementTypeInfo parent)
                        {
                            var parentProperties = parent.Properties?.ToArray();
                            if (parentProperties?.Any() ?? false)
                                AddProperties(elementSchema, entityTemplate, parentProperties);
                        }

                        AddProperties(elementSchema, entityTemplate, properties);
                    }
                }
            }

            return result;
        }

        private void ImportElements([NotNull] ThreatModel source, [NotNull] IThreatModel target, float dpiFactor,
            out int diagrams, out int externalInteractors, out int processes, out int dataStores, out int trustBoundaries)
        {
            diagrams = 0;
            externalInteractors = 0;
            processes = 0;
            dataStores = 0;
            trustBoundaries = 0;

            var elements = source.Elements?
                .OrderBy(x => (int)x.Value.ElementType)
                .ToArray();

            if (elements?.Any() ?? false)
            {
                var schemaManager = new ObjectPropertySchemaManager(target);
                var schema = schemaManager.GetSchema();

                var entities = new List<IEntity>();
                var groups = new List<ITrustBoundary>();

                foreach (var element in elements)
                {
                    IEntity entity = null;
                    ITrustBoundary boundary = null;
                    Interfaces.Scope scope = Scope.Undefined;

                    switch (element.Value.ElementType)
                    {
                        case ElementType.StencilRectangle:
                            entity = target.AddEntity<IExternalInteractor>(element.Value.Name);
                            scope = Scope.ExternalInteractor;
                            externalInteractors++;
                            break;
                        case ElementType.StencilEllipse:
                            entity = target.AddEntity<IProcess>(element.Value.Name);
                            scope = Scope.Process;
                            processes++;
                            break;
                        case ElementType.StencilParallelLines:
                            entity = target.AddEntity<IDataStore>(element.Value.Name);
                            scope = Scope.DataStore;
                            dataStores++;
                            break;
                        case ElementType.BorderBoundary:
                            boundary = target.AddGroup<ITrustBoundary>(element.Value.Name);
                            scope = Scope.TrustBoundary;
                            trustBoundaries++;
                            break;
                        case ElementType.LineBoundary:
                            boundary = target.AddGroup<ITrustBoundary>(element.Value.Name);
                            scope = Scope.TrustBoundary;
                            trustBoundaries++;
                            break;
                    }

                    if (entity != null)
                    {
                        entities.Add(entity);

                        schemaManager.SetObjectId(entity, element.Key.ToString());
                        var properties = element.Value.Properties?.ToArray();

                        var elementType = source.ElementTypes.FirstOrDefault(x =>
                            string.CompareOrdinal(x.TypeId, element.Value.TypeId) == 0);
                        if (elementType != null)
                        {
                            entity.BigImage = elementType.Image;
                            entity.Image = elementType.Image;
                            entity.SmallImage = elementType.Image;
                            var elementSchemaManager = new TmtPropertySchemaManager(target, elementType.Name, scope);
                            AddProperties(elementSchemaManager.GetSchema(), entity, properties);
                        }

                        var diagram = target.Diagrams?.FirstOrDefault(x =>
                            string.CompareOrdinal(x.Name, element.Value.Page) == 0);
                        if (diagram == null)
                        {
                            diagram = target.AddDiagram(element.Value.Page);
                            diagrams++;
                        }

                        diagram.AddEntityShape(entity.Id, new PointF((element.Value.Position.X + element.Value.Size.Width / 2f) * dpiFactor, (element.Value.Position.Y + element.Value.Size.Height / 2f) * dpiFactor));
                    } else if (boundary != null)
                    {
                        groups.Add(boundary);

                        schemaManager.SetObjectId(boundary, element.Key.ToString());
                        var properties = element.Value.Properties?.ToArray();

                        var elementType = source.ElementTypes.FirstOrDefault(x =>
                            string.CompareOrdinal(x.TypeId, element.Value.TypeId) == 0);
                        if (elementType != null)
                        {
                            var elementSchemaManager = new TmtPropertySchemaManager(target, elementType.Name, scope);
                            AddProperties(elementSchemaManager.GetSchema(), boundary, properties);
                        }

                        if (element.Value.ElementType == ElementType.BorderBoundary)
                        {
                            var diagram = target.Diagrams?.FirstOrDefault(x =>
                                string.CompareOrdinal(x.Name, element.Value.Page) == 0);
                            if (diagram == null)
                            {
                                diagram = target.AddDiagram(element.Value.Page);
                                diagrams++;
                            }

                            diagram.AddGroupShape(boundary.Id,
                                new PointF((element.Value.Position.X + element.Value.Size.Width / 2f) * dpiFactor, (element.Value.Position.Y + element.Value.Size.Height / 2f) * dpiFactor),
                                new SizeF(element.Value.Size.Width * dpiFactor, element.Value.Size.Height * dpiFactor));
                        }
                    }
                }
            }
        }

        private int ImportDataFlows([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;

            var flows = source.Flows;
            if (flows?.Any() ?? false)
            {
                var schemaManager = new ObjectPropertySchemaManager(target);

                foreach (var flow in flows)
                {
                    var sourceEntity = GetEntity(flow.SourceGuid.ToString(), target, schemaManager);
                    var targetEntity = GetEntity(flow.TargetGuid.ToString(), target, schemaManager);
                    if (sourceEntity != null && targetEntity != null)
                    {
                        var dataFlow = target.AddDataFlow(flow.Name, sourceEntity.Id, targetEntity.Id);
                        if (dataFlow != null)
                        {
                            schemaManager.SetObjectId(dataFlow, flow.FlowId.ToString());

                            var properties = flow.Properties?.ToArray();

                            if (properties?.Any() ?? false)
                            {
                                var elementType = source.ElementTypes.FirstOrDefault(x =>
                                    string.CompareOrdinal(x.TypeId, flow.TypeId) == 0);
                                if (elementType != null)
                                {
                                    var flowSchemaManager = new TmtPropertySchemaManager(target, elementType.Name, Scope.DataFlow);
                                    var flowSchema = flowSchemaManager.GetSchema();
                                    var outOfScope = flowSchema.AddPropertyType("Out of Scope", PropertyValueType.Boolean);
                                    if (outOfScope != null)
                                    {
                                        outOfScope.Priority = -2;
                                        dataFlow.AddProperty(outOfScope, false.ToString());
                                    }
                                    var reasonOutOfScope =
                                        flowSchema.AddPropertyType("Reason For Out Of Scope", PropertyValueType.String);
                                    if (reasonOutOfScope != null)
                                        reasonOutOfScope.Priority = -1;
                                    if (source.ElementTypes.FirstOrDefault(x =>
                                            string.CompareOrdinal(elementType.ParentTypeId, x.TypeId) == 0) is
                                        ElementTypeInfo parent)
                                    {
                                        AddProperties(flowSchema, dataFlow, parent.Properties);
                                    }

                                    AddProperties(flowSchema, dataFlow, properties);
                                }
                            }
                        }
                        
                        IDiagram diagram = target.Diagrams?.FirstOrDefault(x =>
                            string.CompareOrdinal(x.Name, flow.PageName) == 0);
                        diagram?.AddLink(dataFlow);
                        result++;
                    }
                }
            }

            return result;
        }

        private static void AddProperties([NotNull] IPropertySchema schema, 
            [NotNull] IPropertiesContainer container, [NotNull] IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                if (!string.IsNullOrWhiteSpace(property.Name))
                {
                    var propertyType = schema.GetPropertyType(property.Name);
                    if (propertyType == null)
                    {
                        switch (property.Type)
                        {
                            case PropertyType.String:
                                propertyType = schema.AddPropertyType(property.Name,
                                    PropertyValueType.String);
                                break;
                            case PropertyType.Boolean:
                                propertyType = schema.AddPropertyType(property.Name,
                                    PropertyValueType.Boolean);
                                break;
                            case PropertyType.List:
                                propertyType =
                                    schema.AddPropertyType(property.Name, PropertyValueType.List);
                                if (propertyType is IListPropertyType listPropertyType)
                                {
                                    listPropertyType.Context = property.Values.TagConcat();
                                    listPropertyType.SetListProvider(new ListProviderExtension());
                                }
                                break;
                        }
                    }

                    if (propertyType != null)
                    {
                        var containerProperty = container.GetProperty(propertyType);
                        if (containerProperty == null)
                            container.AddProperty(propertyType, property.Value);
                        else
                            containerProperty.StringValue = property.Value;
                    }
                }
            }
        }

        private void ImportThreats([NotNull] ThreatModel source, [NotNull] IThreatModel target,
            Func<IThreatModel, Threat, IThreatType, IPropertySchema, bool> unassignedThreatHandler,
            out int threatTypes, out int customThreatTypes, out int threats, out int missingThreats)
        {
            threatTypes = 0;
            customThreatTypes = 0;
            threats = 0;
            missingThreats = 0;

            var threatsPerType = source.ThreatsPerType;

            if (threatsPerType?.Any() ?? false)
            {
                var schema = new ThreatsPropertySchemaManager(target).GetSchema();
                InitializeThreatsPropertySchema(schema, source.Properties);
                var schemaManager = new ObjectPropertySchemaManager(target);

                var defaultSeverity = target.GetMappedSeverity(0);

                foreach (var threatPerType in threatsPerType)
                {
                    var threatTypeName = source.GetThreatTypeName(threatPerType.Key);
                    IThreatType threatType = null;
                    if (!string.IsNullOrWhiteSpace(threatTypeName))
                    {
                        ISeverity severity;
                        if (Enum.TryParse<DefaultSeverity>(source.GetThreatTypePriority(threatPerType.Key), out var severityId))
                        {
                            severity = target.GetMappedSeverity((int) severityId);
                        }
                        else
                        {
                            severity = target.GetMappedSeverity((int) DefaultSeverity.High);
                        }

                        threatType = target.AddThreatType(threatTypeName, severity);
                        if (threatType != null)
                        {
                            threatType.Description = source.GetThreatTypeDescription(threatPerType.Key);

                            var threatTypeProperties = source.GetThreatTypeProperties(threatPerType.Key);
                            if (threatTypeProperties?.Any() ?? false)
                            {
                                foreach (var property in threatTypeProperties)
                                {
                                    switch (property.Name)
                                    {
                                        case "Title":
                                            break;
                                        case "UserThreatDescription":
                                            break;
                                        case "Priority":
                                            break;
                                        default:
                                            var propertyType = schema.GetPropertyType(property.Label);
                                            if (propertyType != null)
                                            {
                                                threatType.AddProperty(propertyType, property.Values.FirstOrDefault());
                                            }

                                            break;
                                    }
                                }
                            }

                            threatTypes++;
                        }
                        else
                        {
                            threatType = target.ThreatTypes?
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, threatTypeName) == 0);
                        }
                    }
                    else
                    {
                        var internalThreats = threatPerType.Value;
                        if (internalThreats.Any())
                        {
                            foreach (var internalThreat in internalThreats)
                            {
                                threatType = target.AddThreatType(internalThreat.ToString(), defaultSeverity);
                                if (threatType != null)
                                {
                                    threatType.Description = internalThreat.GetValueFromLabel("Description");
                                    customThreatTypes++;
                                }
                                else
                                {
                                    threatType = target.ThreatTypes?.FirstOrDefault(x =>
                                        string.CompareOrdinal(x.Name, internalThreat.ToString()) == 0);
                                }

                                break;
                            }
                        }
                    }

                    if (threatType != null)
                    {
                        foreach (var threat in threatPerType.Value)
                        {
                            var flow = GetDataFlow(threat.FlowGuid.ToString(), target, schemaManager);
                            if (flow != null)
                            {
                                var threatEvent = flow.AddThreatEvent(threatType);

                                if (threatEvent != null)
                                {
                                    AddProperties(threatEvent, threat, schema);
                                    threats++;
                                }
                                else
                                    missingThreats++;
                            }
                            else
                            {
                                if (unassignedThreatHandler != null)
                                {
                                    if (unassignedThreatHandler(target, threat, threatType, schema))
                                        threats++;
                                    else
                                        missingThreats++;
                                }
                                else
                                {
                                    var threatEvent = target.AddThreatEvent(threatType);

                                    if (threatEvent != null)
                                    {
                                        AddProperties(threatEvent, threat, schema);
                                        threats++;
                                    }
                                    else
                                        missingThreats++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void InitializeThreatsPropertySchema([NotNull] IPropertySchema schema, 
            [NotNull] IEnumerable<PropertyDefinition> sourceProperties)
        {
            if (sourceProperties.Any())
            {
                foreach (var property in sourceProperties)
                {
                    if (!property.HideFromUi && !IsSpecial(property.Name))
                    {
                        var propertyType = schema.GetPropertyType(property.Label);
                        if (propertyType == null)
                        {
                            if (property.Values?.Any() ?? false)
                            {
                                propertyType = schema.AddPropertyType(property.Label, PropertyValueType.List);
                                if (propertyType is IListPropertyType listPropertyType)
                                {
                                    listPropertyType.SetListProvider(new ListProviderExtension());
                                    listPropertyType.Context = property.Values.TagConcat();
                                }
                            }
                            else
                            {
                                schema.AddPropertyType(property.Label, PropertyValueType.String);
                            }
                        }
                    }
                }
            }
        }

        private bool IsSpecial([Required] string propertyName)
        {
            return string.CompareOrdinal(propertyName, "Title") == 0 ||
                   string.CompareOrdinal(propertyName, "UserThreatDescription") == 0 ||
                   string.CompareOrdinal(propertyName, "Priority") == 0;
        }

        private IEntity GetEntity([Required] string msEntityId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            IEntity result = null;

            var entities = model.Entities?.ToArray();
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    var id = schemaManager.GetObjectId(entity);
                    if (string.CompareOrdinal(id, msEntityId) == 0)
                    {
                        result = entity;
                        break;
                    }
                }
            }

            return result;
        }

        private IDataFlow GetDataFlow([Required] string msDataFlowId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            IDataFlow result = null;

            var dataFlows = model.DataFlows?.ToArray();
            if (dataFlows != null)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var id = schemaManager.GetObjectId(dataFlow);
                    if (string.CompareOrdinal(id, msDataFlowId) == 0)
                    {
                        result = dataFlow;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
