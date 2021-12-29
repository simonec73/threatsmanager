using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.MsTmt.Extensions;
using ThreatsManager.MsTmt.Model;
using ThreatsManager.MsTmt.Model.AutoThreatGen;
using ThreatsManager.MsTmt.Properties;
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
        /// <param name="itemTypes">[out] Counter of the Item Types.</param>
        /// <param name="threatTypes">[out] Counter of the Threat Types.</param>
        /// <param name="customThreatTypes">[out] Counter of the custom Threat Types, managed through the Unassigned Threat Handler.</param>
        /// <param name="threats">[out] Counter of the generated Threat Events.</param>
        /// <param name="missingThreats">[out] Counter of the Threat Events that has not been possible to generate.</param>
        public void Import([NotNull] IThreatModel threatModel, [Required] string fileName, float dpiFactor,
            Func<IThreatModel, Threat, IThreatType, IPropertySchema, bool> unassignedThreatHandler,
            out int diagrams, out int externalInteractors, out int processes, out int dataStores, out int flows,
            out int trustBoundaries, out int itemTypes, out int threatTypes, out int customThreatTypes, out int threats, 
            out int missingThreats)
        {
            var model = new ThreatModel(fileName);

            ImportModelInfo(model, threatModel);
            itemTypes = ImportBaseElementTemplates(model, threatModel);
            itemTypes += ImportBaseFlowTemplates(model, threatModel);
            itemTypes += ImportEntityTemplates(model, threatModel);
            itemTypes += ImportFlowTemplates(model, threatModel);
            ImportElements(model, threatModel, dpiFactor, out diagrams, 
                out externalInteractors, out processes, out dataStores, out trustBoundaries);
            ParentElements(model, threatModel);
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
                                var property = threatEvent.GetProperty(propertyType);
                                if (property == null)
                                    threatEvent.AddProperty(propertyType, threat.GetValueFromLabel(label));
                                else
                                    property.StringValue = threat.GetValueFromLabel(label);
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

        private int ImportBaseElementTemplates([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;

            var elements = source.ElementTypes?
                .Where(x => x.IsGeneric)
                .ToArray();

            if (elements?.Any() ?? false)
            {
                var baseEISchema = new BaseExternalInteractorPropertySchemaManager(target).GetSchema();
                AddStandardPropertyTypes(baseEISchema);
                var basePSchema = new BaseProcessPropertySchemaManager(target).GetSchema();
                AddStandardPropertyTypes(basePSchema);
                var baseDSSchema = new BaseDataStorePropertySchemaManager(target).GetSchema();
                AddStandardPropertyTypes(baseDSSchema);
                var baseTBSchema = new BaseTrustBoundaryPropertySchemaManager(target).GetSchema();
                AddStandardPropertyTypes(baseTBSchema);

                if (HasStandardStructure(elements))
                {
                    IPropertySchema schema = null;

                    foreach (var element in elements)
                    {
                        switch (element.ElementType)
                        {
                            case ElementType.StencilRectangle:
                                schema = baseEISchema;
                                break;
                            case ElementType.StencilEllipse:
                                schema = basePSchema;
                                break;
                            case ElementType.StencilParallelLines:
                                schema = baseDSSchema;
                                break;
                            case ElementType.BorderBoundary:
                            case ElementType.LineBoundary:
                                schema = baseTBSchema;
                                break;
                        }

                        if (schema != null)
                        {
                            var properties = element.Properties?.ToArray();
                            AddProperties(schema, null, properties);
                        }
                    }
                }
                else // We need to import all base elements!
                {
                    result = ImportEntityTemplates(elements, target);
                }
            }

            return result;
        }

        private int ImportBaseFlowTemplates([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;

            var connectors = source.FlowTypes?
                .Where(x => x.IsGeneric)
                .ToArray();

            if (connectors?.Any() ?? false)
            {
                var schema = new BaseFlowPropertySchemaManager(target).GetSchema();
                AddStandardPropertyTypes(schema);

                if (HasStandardStructure(connectors))
                {
                    //if (!(schema.PropertyTypes?.Any(x =>
                    //        string.CompareOrdinal("Out of Scope", x.Name) == 0) ?? false))
                    //{
                    //    var outOfScope = schema.AddPropertyType("Out of Scope",
                    //        PropertyValueType.Boolean);
                    //    if (outOfScope != null)
                    //    {
                    //        outOfScope.Priority = -2;
                    //    }
                    //}

                    //if (!(schema.PropertyTypes?.Any(x =>
                    //        string.CompareOrdinal("Reason For Out Of Scope", x.Name) == 0) ?? false))
                    //{
                    //    var reasonOutOfScope =
                    //        schema.AddPropertyType("Reason For Out Of Scope",
                    //            PropertyValueType.String);
                    //    if (reasonOutOfScope != null)
                    //        reasonOutOfScope.Priority = -1;
                    //}

                    foreach (var connector in connectors)
                    {
                        AddProperties(schema, null, connector.Properties?.ToArray());
                    }
                }
                else
                {
                    result = ImportFlowTemplates(connectors, target);
                }
            }

            return result;
        }

        private int ImportEntityTemplates([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;
            var elements = source.ElementTypes?
                .Where(x => !x.IsGeneric)
                .ToArray();

            if (elements?.Any() ?? false)
            {
                result = ImportEntityTemplates(elements, target);
            }

            return result;
        }

        private int ImportEntityTemplates([NotNull] IEnumerable<ElementTypeInfo> elements, [NotNull] IThreatModel target)
        {
            int result = 0;

            var schemaManager = new ObjectPropertySchemaManager(target);

            IEntityTemplate entityTemplate;
            ITrustBoundaryTemplate trustBoundaryTemplate;
            Interfaces.Scope scope;
            IPropertySchema baseSchema;
            var baseEISchema = new BaseExternalInteractorPropertySchemaManager(target).GetSchema();
            var basePSchema = new BaseProcessPropertySchemaManager(target).GetSchema();
            var baseDSSchema = new BaseDataStorePropertySchemaManager(target).GetSchema();
            var baseTBSchema = new BaseTrustBoundaryPropertySchemaManager(target).GetSchema();

            foreach (var element in elements)
            {
                IItemTemplate parent = null;
                IPropertySchema schema = null;
                if (!element.IsGeneric)
                {
                    var parentTypeId = element.ParentTypeId;
                    if (!string.IsNullOrWhiteSpace(parentTypeId))
                    {
                        parent = GetEntityTemplate(parentTypeId, target, schemaManager);
                        if (parent == null)
                            parent = GetTrustBoundaryTemplate(parentTypeId, target, schemaManager);

                        if (parent != null)
                        {
                            schema = target.GetSchema(parent.Name, Resources.DefaultNamespace);
                        }
                    }
                }

                switch (element.ElementType)
                {
                    case ElementType.StencilRectangle:
                        entityTemplate = target.AddEntityTemplate(element.Name, element.Description, 
                            element.Image, element.Image, element.Image,
                            EntityType.ExternalInteractor);
                        trustBoundaryTemplate = null;
                        scope = Scope.ExternalInteractor;
                        baseSchema = baseEISchema;
                        result++;
                        break;
                    case ElementType.StencilEllipse:
                        entityTemplate = target.AddEntityTemplate(element.Name, element.Description, 
                            element.Image, element.Image, element.Image,
                            EntityType.Process);
                        trustBoundaryTemplate = null;
                        scope = Scope.Process;
                        baseSchema = basePSchema;
                        result++;
                        break;
                    case ElementType.StencilParallelLines:
                        entityTemplate = target.AddEntityTemplate(element.Name, element.Description, 
                            element.Image, element.Image, element.Image,
                            EntityType.DataStore);
                        trustBoundaryTemplate = null;
                        scope = Scope.DataStore;
                        baseSchema = baseDSSchema;
                        result++;
                        break;
                    case ElementType.LineBoundary:
                    case ElementType.BorderBoundary:
                        entityTemplate = null;
                        trustBoundaryTemplate = target.AddTrustBoundaryTemplate(element.Name, element.Description);
                        scope = Scope.TrustBoundary;
                        baseSchema = baseTBSchema;
                        result++;
                        break;
                    default:
                        entityTemplate = null;
                        trustBoundaryTemplate = null;
                        scope = Scope.Undefined;
                        baseSchema = null;
                        break;
                }

                if (entityTemplate != null)
                {
                    schemaManager.SetObjectId(entityTemplate, element.TypeId);
                    if (baseSchema != null)
                    {
                        InitializeBaseSchema(entityTemplate, baseSchema);
                        var properties = element.Properties?.ToArray();
                        AddProperties(target, element.Name, scope, baseSchema, entityTemplate, properties);
                    }

                    if (schema != null)
                    {
                        InitializeBaseSchema(entityTemplate, schema);
                        var properties = element.Properties?.ToArray();
                        AddProperties(target, element.Name, scope, schema, entityTemplate, properties);
                    }
                } 
                else if (trustBoundaryTemplate != null)
                {
                    schemaManager.SetObjectId(trustBoundaryTemplate, element.TypeId);

                    var properties = element.Properties?.ToArray();

                    if (baseSchema != null)
                    {
                        InitializeBaseSchema(trustBoundaryTemplate, baseSchema);
                        AddProperties(target, element.Name, scope, baseSchema, trustBoundaryTemplate, properties);
                    }

                    if (schema != null)
                    {
                        InitializeBaseSchema(entityTemplate, schema);
                        AddProperties(target, element.Name, scope, schema, entityTemplate, properties);
                    }
                }
            }

            return result;
        }

        private int ImportFlowTemplates([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;
            var connectors = source.FlowTypes?
                .Where(x => !x.IsGeneric)
                .ToArray();

            if (connectors?.Any() ?? false)
            {
                result = ImportFlowTemplates(connectors, target);
            }

            return result;
        }

        private int ImportFlowTemplates([NotNull] IEnumerable<ElementTypeInfo> connectors, [NotNull] IThreatModel target)
        {
            int result = 0;

            var schemaManager = new ObjectPropertySchemaManager(target);
            var baseSchema = new BaseFlowPropertySchemaManager(target).GetSchema();

            foreach (var connector in connectors)
            {
                IItemTemplate parent = null;
                IPropertySchema schema = null;
                if (!connector.IsGeneric)
                {
                    var parentTypeId = connector.ParentTypeId;
                    if (!string.IsNullOrWhiteSpace(parentTypeId))
                    {
                        parent = GetFlowTemplate(parentTypeId, target, schemaManager);

                        if (parent != null)
                        {
                            schema = target.GetSchema(parent.Name, Resources.DefaultNamespace);
                        }
                    }
                }

                var template = target.AddFlowTemplate(connector.Name, connector.Description);
                if (template != null)
                {
                    result++;
                    schemaManager.SetObjectId(template, connector.TypeId);

                    var properties = connector.Properties?.ToArray();

                    if (baseSchema != null)
                    {
                        InitializeBaseSchema(template, baseSchema);
                        AddProperties(target, connector.Name, Scope.DataFlow, baseSchema, template, properties);
                    }

                    if (schema != null)
                    {
                        InitializeBaseSchema(template, schema);
                        AddProperties(target, connector.Name, Scope.DataFlow, schema, template, properties);
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

                IEntity entity;
                ITrustBoundary boundary;
                IEntityTemplate entityTemplate;
                ITrustBoundaryTemplate trustBoundaryTemplate;
                IPropertySchema schema;
                IPropertySchema secondarySchema;
                var baseEISchema = new BaseExternalInteractorPropertySchemaManager(target).GetSchema();
                var basePSchema = new BaseProcessPropertySchemaManager(target).GetSchema();
                var baseDSSchema = new BaseDataStorePropertySchemaManager(target).GetSchema();
                var baseTBSchema = new BaseTrustBoundaryPropertySchemaManager(target).GetSchema();

                foreach (var element in elements)
                {
                    var elementName = element.Value.Name;
                    if (string.IsNullOrWhiteSpace(elementName))
                        elementName = "<unnamed>";
                    switch (element.Value.ElementType)
                    {
                        case ElementType.StencilRectangle:
                            entityTemplate = target.EntityTemplates?.FirstOrDefault(x =>
                                string.CompareOrdinal(schemaManager.GetObjectId(x), element.Value.TypeId) == 0);
                            entity = entityTemplate != null ? entityTemplate.CreateEntity(elementName) : 
                                target.AddEntity<IExternalInteractor>(elementName);
                            boundary = null;
                            schema = baseEISchema;
                            secondarySchema = entityTemplate != null ? 
                                target.GetSchema(entityTemplate.Name, Resources.DefaultNamespace) : null;
                            externalInteractors++;
                            break;
                        case ElementType.StencilEllipse:
                            entityTemplate = target.EntityTemplates?.FirstOrDefault(x =>
                                string.CompareOrdinal(schemaManager.GetObjectId(x), element.Value.TypeId) == 0);
                            entity = entityTemplate != null ? entityTemplate.CreateEntity(elementName) : 
                                target.AddEntity<IProcess>(elementName);
                            boundary = null;
                            schema = basePSchema;
                            secondarySchema = entityTemplate != null ? 
                                target.GetSchema(entityTemplate.Name, Resources.DefaultNamespace) : null;
                            processes++;
                            break;
                        case ElementType.StencilParallelLines:
                            entityTemplate = target.EntityTemplates?.FirstOrDefault(x =>
                                string.CompareOrdinal(schemaManager.GetObjectId(x), element.Value.TypeId) == 0);
                            entity = entityTemplate != null ? entityTemplate.CreateEntity(elementName) : 
                                target.AddEntity<IDataStore>(elementName);
                            boundary = null;
                            schema = baseDSSchema;
                            secondarySchema = entityTemplate != null ? 
                                target.GetSchema(entityTemplate.Name, Resources.DefaultNamespace) : null;
                            dataStores++;
                            break;
                        case ElementType.BorderBoundary:
                        case ElementType.LineBoundary:
                            trustBoundaryTemplate = target.TrustBoundaryTemplates?.FirstOrDefault(x =>
                                string.CompareOrdinal(schemaManager.GetObjectId(x), element.Value.TypeId) == 0);
                            entity = null;
                            boundary = trustBoundaryTemplate != null ? trustBoundaryTemplate.CreateTrustBoundary(elementName) :
                                target.AddGroup<ITrustBoundary>(elementName);
                            schema = baseTBSchema;
                            secondarySchema = trustBoundaryTemplate != null ? 
                                target.GetSchema(trustBoundaryTemplate.Name, Resources.DefaultNamespace) : null;
                            trustBoundaries++;
                            break;
                        default:
                            entity = null;
                            boundary = null;
                            schema = null;
                            secondarySchema = null;
                            break;
                    }

                    if (entity != null)
                    {
                        schemaManager.SetObjectId(entity, element.Value.TypeId);
                        schemaManager.SetInstanceId(entity, element.Key.ToString());
                        var properties = element.Value.Properties?.ToArray();
                        AddProperties(target, schema, secondarySchema, entity, properties);

                        var diagram = target.Diagrams?.FirstOrDefault(x =>
                            string.CompareOrdinal(x.Name, element.Value.Page) == 0);
                        if (diagram == null)
                        {
                            diagram = target.AddDiagram(element.Value.Page);
                            diagrams++;
                        }

                        diagram.AddEntityShape(entity.Id, 
                            new PointF((element.Value.Position.X + element.Value.Size.Width / 2f) * dpiFactor, (element.Value.Position.Y + element.Value.Size.Height / 2f) * dpiFactor));
                    } else if (boundary != null)
                    {
                        schemaManager.SetObjectId(boundary, element.Value.TypeId);
                        schemaManager.SetInstanceId(boundary, element.Key.ToString());
                        var properties = element.Value.Properties?.ToArray();
                        AddProperties(target, schema, secondarySchema, entity, properties);

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

        private void ParentElements([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            var schemaManager = new ObjectPropertySchemaManager(target);

            var trustBoundaries = source.Elements?
                .Where(x => x.Value.ElementType == ElementType.BorderBoundary)
                .OrderByDescending(x => x.Value.Size.Width * x.Value.Size.Height)
                .ToArray();

            var count = trustBoundaries?.Length ?? 0;
            if (count > 1)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    var parent = trustBoundaries[i];

                    for (int j = i + 1; j < count; j++)
                    {
                        var child = trustBoundaries[j];
                        if (IsParent(parent.Value, child.Value))
                        {
                            var p = GetTrustBoundary(parent.Key.ToString(), target, schemaManager);
                            var c = GetTrustBoundary(child.Key.ToString(), target, schemaManager);
                            if (p != null && c != null)
                            {
                                c.SetParent(p);
                            }
                        }
                    }
                }
            }

            if (count > 0)
            {
                var entities = source.Elements?
                    .Where(x => x.Value.ElementType == ElementType.StencilRectangle ||
                                x.Value.ElementType == ElementType.StencilEllipse ||
                                x.Value.ElementType == ElementType.StencilParallelLines)
                    .ToArray();

                if (entities?.Any() ?? false)
                {
                    trustBoundaries = trustBoundaries                
                        .OrderBy(x => x.Value.Size.Width * x.Value.Size.Height)
                        .ToArray();

                    foreach (var entity in entities)
                    {
                        string parentKey = null;

                        foreach (var trustBoundary in trustBoundaries)
                        {
                            if (IsParent(trustBoundary.Value, entity.Value))
                            {
                                parentKey = trustBoundary.Key.ToString();
                                break;
                            }
                        }

                        if (parentKey !=null)
                        {
                            var parent = GetTrustBoundary(parentKey, target, schemaManager);
                            if (parent != null)
                            {
                                var e = GetEntity(entity.Key.ToString(), target, schemaManager);
                                if (e != null)
                                {
                                    e.SetParent(parent);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int ImportDataFlows([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            int result = 0;

            var flows = source.Flows?.ToArray();

            if (flows?.Any() ?? false)
            {
                var schemaManager = new ObjectPropertySchemaManager(target);
                var baseSchema = new BaseFlowPropertySchemaManager(target).GetSchema();

                foreach (var flow in flows)
                {
                    var sourceEntity = GetEntity(flow.SourceGuid.ToString(), target, schemaManager);
                    var targetEntity = GetEntity(flow.TargetGuid.ToString(), target, schemaManager);
                    if (sourceEntity != null && targetEntity != null)
                    {
                        var flowTemplate = target.FlowTemplates?.FirstOrDefault(x =>
                            string.CompareOrdinal(schemaManager.GetObjectId(x), flow.TypeId) == 0);
                        var secondarySchema = flowTemplate != null ?
                            target.GetSchema(flowTemplate.Name, Resources.DefaultNamespace) : null;
                        var dataFlow = flowTemplate != null ? flowTemplate.CreateFlow(flow.Name, sourceEntity.Id, targetEntity.Id) :
                            target.AddDataFlow(flow.Name, sourceEntity.Id, targetEntity.Id);
                        if (dataFlow != null)
                        {
                            schemaManager.SetObjectId(dataFlow, flow.TypeId);
                            schemaManager.SetInstanceId(dataFlow, flow.FlowId.ToString());

                            var properties = flow.Properties?.ToArray();
                            AddProperties(target, baseSchema, secondarySchema, dataFlow, properties);
                            result++;
                            
                            IDiagram diagram = target.Diagrams?.FirstOrDefault(x =>
                                string.CompareOrdinal(x.Name, flow.PageName) == 0);
                            diagram?.AddLink(dataFlow);
                        }
                    }
                }
            }

            return result;
        }

        private static void AddProperties([NotNull] IPropertySchema schema, 
            IPropertiesContainer container, [NotNull] IEnumerable<Property> properties)
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

                    if (propertyType != null && container != null)
                    {
                        var value = property.Value;
                        if (string.IsNullOrWhiteSpace(value) && propertyType is IListPropertyType listPropertyType)
                        {
                            value = listPropertyType.Values.FirstOrDefault()?.Id;
                        }

                        var containerProperty = container.GetProperty(propertyType);
                        if (containerProperty == null)
                            container.AddProperty(propertyType, value);
                        else
                            containerProperty.StringValue = value;
                    }
                }
            }
        }

        private static void AddStandardPropertyTypes([NotNull] IPropertySchema schema)
        {
            var outOfScope = schema.GetPropertyType("Out of Scope") ??
                             schema.AddPropertyType("Out of Scope", PropertyValueType.Boolean);
            var reason = schema.GetPropertyType("Reason For Out Of Scope") ??
                         schema.AddPropertyType("Reason For Out Of Scope", PropertyValueType.String);
        }

        private static void InitializeBaseSchema([NotNull] IItemTemplate template, [NotNull] IPropertySchema baseSchema)
        {
            var propertyTypes = baseSchema.PropertyTypes?.ToArray();
            if (propertyTypes?.Any() ?? false)
            {
                foreach (var propertyType in propertyTypes)
                {
                    string value = null;
                    if (propertyType is IListPropertyType listPropertyType)
                    {
                        value = listPropertyType.Values.FirstOrDefault()?.Id;
                    }

                    var containerProperty = template.GetProperty(propertyType);
                    if (containerProperty == null)
                        template.AddProperty(propertyType, value);
                    else
                        containerProperty.StringValue = value;
                }
            }
        }

        private static void AddProperties([NotNull] IThreatModel model,
            [NotNull] string schemaName, Scope scope, [NotNull] IPropertySchema baseSchema,
            IPropertiesContainer container, [NotNull] IEnumerable<Property> properties)
        {
            IPropertySchema schema = null;

            foreach (var property in properties)
            {
                if (!string.IsNullOrWhiteSpace(property.Name))
                {
                    var propertyType = baseSchema.GetPropertyType(property.Name);
                    if (propertyType == null)
                    {
                        if (schema == null)
                            schema = new TmtPropertySchemaManager(model, schemaName, scope).GetSchema();

                        propertyType = schema.GetPropertyType(property.Name);
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
                    }

                    if (propertyType != null && container != null)
                    {
                        var value = property.Value;
                        if (string.IsNullOrWhiteSpace(value) && propertyType is IListPropertyType listPropertyType)
                        {
                            value = listPropertyType.Values.FirstOrDefault()?.Id;
                        }

                        var containerProperty = container.GetProperty(propertyType);
                        if (containerProperty == null)
                            container.AddProperty(propertyType, value);
                        else
                            containerProperty.StringValue = value;
                    }
                }
            }
        }

        private static void AddProperties([NotNull] IThreatModel model,
            [NotNull] IPropertySchema baseSchema, IPropertySchema secondarySchema,
            IPropertiesContainer container, [NotNull] IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                if (!string.IsNullOrWhiteSpace(property.Name))
                {
                    var propertyType = baseSchema.GetPropertyType(property.Name) ??
                        secondarySchema?.GetPropertyType(property.Name);

                    if (propertyType != null && container != null)
                    {
                        var value = property.Value;
                        if (string.IsNullOrWhiteSpace(value) && propertyType is IListPropertyType listPropertyType)
                        {
                            value = listPropertyType.Values.FirstOrDefault()?.Id;
                        }

                        var containerProperty = container.GetProperty(propertyType);
                        if (containerProperty == null)
                            container.AddProperty(propertyType, value);
                        else
                            containerProperty.StringValue = value;
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
                    var tt = source.GetThreatType(threatPerType.Key);
                    IThreatType threatType = null;
                    if (!string.IsNullOrWhiteSpace(tt?.Name))
                    {
                        ISeverity severity;
                        if (Enum.TryParse<DefaultSeverity>(tt.Priority, out var severityId))
                        {
                            severity = target.GetMappedSeverity((int) severityId);
                        }
                        else
                        {
                            severity = target.GetMappedSeverity((int) DefaultSeverity.High);
                        }

                        threatType = target.AddThreatType(tt.Name, severity);
                        if (threatType != null)
                        {
                            threatType.Description = tt.Description;
                            var threatTypeProperties = tt.Properties?.ToArray();
                            if (threatTypeProperties?.Any() ?? false)
                            {
                                foreach (var property in threatTypeProperties)
                                {
                                    if (property != null)
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
                                                    threatType.AddProperty(propertyType, property.Values?.FirstOrDefault());
                                                }

                                                break;
                                        }
                                    }
                                }
                            }

                            threatTypes++;
                        }
                        else
                        {
                            threatType = target.ThreatTypes?
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, tt.Name) == 0);
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
                        CreateGenerationRule(source, tt, threatType);

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

        private void CreateGenerationRule([NotNull] ThreatModel model, 
            [NotNull] ThreatType source, [NotNull] IThreatType target)
        {
            SelectionRuleNode include = AnalyzeGenerationRule(model, target.Model, source.IncludeFilter);
            SelectionRuleNode exclude = AnalyzeGenerationRule(model, target.Model, source.ExcludeFilter);
            SelectionRule rule = null;

            var andNode = new AndRuleNode()
            {
                Name = "AND"
            };

            if (include != null)
            {
                andNode.Children.Add(include);
            }

            if (exclude != null)
            {
                andNode.Children.Add(new NotRuleNode()
                {
                    Name = "NOT",
                    Child = exclude
                });
            }

            if (andNode.Children.Any())
            {
                andNode.Children.Add(new BooleanRuleNode("Out of Scope", Resources.DefaultNamespace, Resources.TmtFlowPropertySchema, false)
                {
                    Scope = AutoGenRules.Engine.Scope.Object
                });

                rule = new SelectionRule()
                {
                    Root = andNode
                };

                var schemaManager = new AutoGenRulesPropertySchemaManager(target.Model);
                var propertyType = schemaManager.GetPropertyType();
                var property = target.GetProperty(propertyType) ?? target.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                    jsonSerializableObject.Value = rule;
            }
        }

        private SelectionRuleNode AnalyzeGenerationRule(ThreatModel source, IThreatModel target, string ruleText)
        {
            SelectionRuleNode result = null;

            if (!string.IsNullOrWhiteSpace(ruleText))
            {
                ICharStream stream = CharStreams.fromString(ruleText);
                ITokenSource lexer = new TmtLexer(stream, TextWriter.Null, TextWriter.Null);
                ITokenStream tokens = new CommonTokenStream(lexer);
                TmtParser parser = new TmtParser(tokens) {BuildParseTree = true};
                IParseTree tree = parser.parse();
                RuleVisitor visitor = new RuleVisitor(source, target);
                visitor.Visit(tree);
                result = visitor.Rule;
            }

            return result;
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

        private bool HasStandardStructure([NotNull] IEnumerable<ElementTypeInfo> elements)
        {
            return !(elements.Where(x => x.ElementType == ElementType.StencilRectangle).Count() > 1 ||
                   elements.Where(x => x.ElementType == ElementType.StencilEllipse).Count() > 1 ||
                   elements.Where(x => x.ElementType == ElementType.StencilParallelLines).Count() > 1 ||
                   elements.Where(x => x.ElementType == ElementType.BorderBoundary).Count() > 1 ||
                   elements.Where(x => x.ElementType == ElementType.Connector).Count() > 1);
        }

        private bool IsSpecial([Required] string propertyName)
        {
            return string.CompareOrdinal(propertyName, "Title") == 0 ||
                   string.CompareOrdinal(propertyName, "UserThreatDescription") == 0 ||
                   string.CompareOrdinal(propertyName, "Priority") == 0;
        }

        private bool IsParent([NotNull] ElementInfo parent, [NotNull] ElementInfo child)
        {
            bool result = false;

            if (parent.ElementType == ElementType.BorderBoundary && string.CompareOrdinal(parent.Page, child.Page) == 0)
            {
                if (child.ElementType == ElementType.StencilRectangle ||
                    child.ElementType == ElementType.StencilEllipse ||
                    child.ElementType == ElementType.StencilParallelLines)
                {
                    result = parent.Position.X <= child.Position.X && parent.Position.Y <= child.Position.Y &&
                             parent.Position.X + parent.Size.Width >= child.Position.X &&
                             parent.Position.Y + parent.Size.Height >= child.Position.Y;
                }
                else if (child.ElementType == ElementType.BorderBoundary)
                {
                    result = parent.Position.X <= child.Position.X && parent.Position.Y <= child.Position.Y &&
                             parent.Position.X + parent.Size.Width >= child.Position.X + child.Size.Width &&
                             parent.Position.Y + parent.Size.Height >= child.Position.Y + child.Size.Height;
                }
            }

            return result;
        }
        
        private IEntityTemplate GetEntityTemplate([Required] string typeId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            IEntityTemplate result = null;

            var templates = model.EntityTemplates?.ToArray();
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var id = schemaManager.GetObjectId(template);
                    if (string.CompareOrdinal(id, typeId) == 0)
                    {
                        result = template;
                        break;
                    }
                }
            }

            return result;
        }
        
        private IFlowTemplate GetFlowTemplate([Required] string typeId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            IFlowTemplate result = null;

            var templates = model.FlowTemplates?.ToArray();
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var id = schemaManager.GetObjectId(template);
                    if (string.CompareOrdinal(id, typeId) == 0)
                    {
                        result = template;
                        break;
                    }
                }
            }

            return result;
        }
        
        private ITrustBoundaryTemplate GetTrustBoundaryTemplate([Required] string typeId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            ITrustBoundaryTemplate result = null;

            var templates = model.TrustBoundaryTemplates?.ToArray();
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var id = schemaManager.GetObjectId(template);
                    if (string.CompareOrdinal(id, typeId) == 0)
                    {
                        result = template;
                        break;
                    }
                }
            }

            return result;
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
                    var id = schemaManager.GetInstanceId(entity);
                    if (string.CompareOrdinal(id, msEntityId) == 0)
                    {
                        result = entity;
                        break;
                    }
                }
            }

            return result;
        }

        private ITrustBoundary GetTrustBoundary([Required] string msGroupId, [NotNull] IThreatModel model,
            [NotNull] ObjectPropertySchemaManager schemaManager)
        {
            ITrustBoundary result = null;

            var groups = model.Groups?.OfType<ITrustBoundary>().ToArray();
            if (groups != null)
            {
                foreach (var group in groups)
                {
                    var id = schemaManager.GetInstanceId(group);
                    if (string.CompareOrdinal(id, msGroupId) == 0)
                    {
                        result = group;
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
                    var id = schemaManager.GetInstanceId(dataFlow);
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
