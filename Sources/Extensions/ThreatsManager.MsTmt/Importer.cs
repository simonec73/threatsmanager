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
        /// <param name="unassignedThreatHandler">Function to be manage custom Threats, that have not been assigned to any flow.</param>
        /// <param name="externalInteractors">[out] Counter of the External Interactors.</param>
        /// <param name="process">[out] Counter of the Processes.</param>
        /// <param name="dataStore">[out] Counter of the Data Stores.</param>
        /// <param name="flows">[out] Counter of the Flows.</param>
        /// <param name="threatTypes">[out] Counter of the Threat Types.</param>
        /// <param name="customThreatTypes">[out] Counter of the custom Threat Types, managed through the Unassigned Threat Handler.</param>
        /// <param name="threats">[out] Counter of the generated Threat Events.</param>
        /// <param name="missingThreats">[out] Counter of the Threat Events that has not been possible to generate.</param>
        public void Import([NotNull] IThreatModel threatModel, [Required] string fileName,
            Func<IThreatModel, Threat, IThreatType, IPropertySchema, bool> unassignedThreatHandler,
            out int externalInteractors, out int process, out int dataStore, out int flows,
            out int threatTypes, out int customThreatTypes, out int threats, out int missingThreats)
        {
            var model = new ThreatModel(fileName);

            ImportModelInfo(model, threatModel);
            ImportElements(model, threatModel, out externalInteractors, out process, out dataStore);
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

        private void ImportElements([NotNull] ThreatModel source, [NotNull] IThreatModel target,
            out int externalInteractors, out int process, out int dataStore)
        {
            externalInteractors = 0;
            process = 0;
            dataStore = 0;

            var elements = source.Elements?.ToArray();

            if (elements?.Any() ?? false)
            {
                var schemaManager = new EntitiesPropertySchemaManager(target);
                var schema = schemaManager.GetSchema();

                foreach (var element in elements)
                {
                    IEntity entity;

                    switch (element.Value.ElementType)
                    {
                        case ElementType.StencilRectangle:
                            entity = target.AddEntity<IExternalInteractor>(element.Value.Name);
                            externalInteractors++;
                            break;
                        case ElementType.StencilEllipse:
                            entity = target.AddEntity<IProcess>(element.Value.Name);
                            process++;
                            break;
                        case ElementType.StencilParallelLines:
                            entity = target.AddEntity<IDataStore>(element.Value.Name);
                            dataStore++;
                            break;
                        default:
                            entity = null;
                            break;
                    }

                    if (entity != null)
                    {
                        schemaManager.SetMsTmtEntityId(entity, element.Key.ToString());
                        var properties = element.Value.Properties?.ToArray();
                        AddProperties(schema, entity, properties);

                        IDiagram diagram = target.Diagrams?.FirstOrDefault(x =>
                            string.CompareOrdinal(x.Name, element.Value.Page) == 0);
                        if (diagram == null)
                        {
                            diagram = target.AddDiagram(element.Value.Page);
                        }
                        diagram.AddEntityShape(entity.Id, new PointF(element.Value.Position.X * 2f, element.Value.Position.Y * 2f));
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
                var entitiesSchemaManager = new EntitiesPropertySchemaManager(target);
                var dataFlowsSchemaManager = new DataFlowsPropertySchemaManager(target);
                var schema = dataFlowsSchemaManager.GetSchema();

                foreach (var flow in flows)
                {
                    var sourceEntity = GetEntity(flow.SourceGuid.ToString(), target, entitiesSchemaManager);
                    var targetEntity = GetEntity(flow.TargetGuid.ToString(), target, entitiesSchemaManager);
                    if (sourceEntity != null && targetEntity != null)
                    {
                        var dataFlow = target.AddDataFlow(flow.Name, sourceEntity.Id, targetEntity.Id);
                        if (dataFlow != null)
                        {
                            dataFlowsSchemaManager.SetMsTmtDataFlowId(dataFlow, flow.FlowId.ToString());

                            var properties = flow.Properties?.ToArray();
                            AddProperties(schema, dataFlow, properties);
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
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    var current = container.GetProperty(propertyType) ??
                                  container.AddProperty(propertyType, property.Value);
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
                var dataFlowsSchemaManager = new DataFlowsPropertySchemaManager(target);

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
                            var flow = GetDataFlow(threat.FlowGuid.ToString(), target, dataFlowsSchemaManager);
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
                                if (unassignedThreatHandler(target, threat, threatType, schema))
                                    threats++;
                                else
                                    missingThreats++;
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
            [NotNull] EntitiesPropertySchemaManager schemaManager)
        {
            IEntity result = null;

            var entities = model.Entities?.ToArray();
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    var id = schemaManager.GetMsTmtEntityId(entity);
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
            [NotNull] DataFlowsPropertySchemaManager schemaManager)
        {
            IDataFlow result = null;

            var dataFlows = model.DataFlows?.ToArray();
            if (dataFlows != null)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var id = schemaManager.GetMsTmtDataFlowId(dataFlow);
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
