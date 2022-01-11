using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Schemas
{
    public class DevOpsConfigPropertySchemaManager
    {
        private IThreatModel _model;

        public DevOpsConfigPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetPropertySchema()
        {
            IPropertySchema result = _model.GetSchema(Properties.Resources.DevOpsConfigPropertySchema,
                Properties.Resources.DefaultNamespace) ?? _model.AddSchema(Properties.Resources.DevOpsConfigPropertySchema,
                Properties.Resources.DefaultNamespace);
            result.Description = Properties.Resources.DevOpsConfigPropertySchemaDescription;
            result.Visible = false;
            result.AppliesTo = Scope.ThreatModel;
            result.System = true;
            result.AutoApply = false;
            result.NotExportable = true;

            return result;
        }

        #region DevOps Connector.
        public IPropertyType GetPropertyTypeDevOpsConnector()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.DevOpsConfig) ?? schema.AddPropertyType(Properties.Resources.DevOpsConfig, PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.DevOpsConfigDescription;
            }

            return result;
        }

        public IDevOpsConnector GetDevOpsConnector(out string url, out string project)
        {
            IDevOpsConnector result = null;
            url = null;
            project = null;

            var propertyType = GetPropertyTypeDevOpsConnector();

            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is DevOpsConnection connection)
                {
                    result = ExtensionUtils.GetExtension<IDevOpsConnectorFactory>(connection.ExtensionId)?.Create();
                    if (result != null)
                    {
                        url = connection.Url;
                        project = connection.Project;

                        result.MasterParent = connection.MasterParent;
                        result.Tag = connection.Tag;
                        result.WorkItemType = connection.WorkItemType;

                        var workItemStateMappings = connection.WorkItemStateMappings;
                        if (workItemStateMappings?.Any() ?? false)
                        {
                            foreach (var mapping in workItemStateMappings)
                            {
                                result.SetWorkItemStateMapping(mapping.Field, mapping.Status);
                            }
                        }

                        var workItemFieldMappings = connection.WorkItemFieldMappings;
                        if (workItemFieldMappings?.Any() ?? false)
                        {
                            foreach (var mapping in workItemFieldMappings)
                            {
                                result.SetWorkItemFieldMapping(mapping.Field, mapping.GetMappedField(_model));
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void RegisterConnection([NotNull] IDevOpsConnector connector)
        {
            var propertyType = GetPropertyTypeDevOpsConnector();

            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);

                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    jsonSerializableObject.Value = new DevOpsConnection(connector);
                }
            }
        }

        public void UnregisterConnection()
        {
            var propertyType = GetPropertyTypeDevOpsConnector();

            if (propertyType != null)
            {
                if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                {
                    property.Value = null;
                }
            }
        }

        public DevOpsConnection ConnectionInfo
        {
            get
            {
                DevOpsConnection result = null;

                var propertyType = GetPropertyTypeDevOpsConnector();

                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is DevOpsConnection connection)
                    {
                        result = connection;
                    }
                }

                return result;
            }
        }
        #endregion

        #region Iterations.
        public IPropertyType GetPropertyTypeIterations()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.DevOpsIterations) ?? 
                         schema.AddPropertyType(Properties.Resources.DevOpsIterations, PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.DevOpsIterationsDescription;
            }

            return result;
        }

        public IEnumerable<Iteration> GetIterations()
        {
            IEnumerable<Iteration> result = null;

            var propertyType = GetPropertyTypeIterations();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Iterations iterations)
                {
                    result = iterations.Items?.AsReadOnly();
                }
            }

            return result;
        }

        public void SetIterations(IEnumerable<Iteration> iterations)
        {
            var propertyType = GetPropertyTypeIterations();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    Iterations container;
                    if (iterations?.Any() ?? false)
                        container = new Iterations(iterations);
                    else
                        container = null;

                    jsonSerializableObject.Value = container;
                }
            }
        }

        public Iteration CurrentIteration
        {
            get
            {
                Iteration result = null;

                var iterations = GetIterations()?
                    .OrderBy(x => x.Start)
                    .ToArray();
                if (iterations?.Any() ?? false)
                {
                    var count = iterations.Length;
                    var now = DateTime.Now.Date;

                    for (int i = 0; i < count; i++)
                    {
                        var current = iterations.ElementAt(i);
                        if (current.Start.HasValue && current.End.HasValue &&
                            now >= current.Start.Value.Date && now <= current.End.Value.Date)
                        {
                            result = current;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public Iteration PreviousIteration
        {
            get
            {
                Iteration result = null;

                var iterations = GetIterations()?
                    .OrderByDescending(x => x.Start)
                    .ToArray();
                if (iterations?.Any() ?? false)
                {
                    var count = iterations.Length;
                    var now = DateTime.Now.Date;

                    for (int i = 0; i < count; i++)
                    {
                        var current = iterations.ElementAt(i);
                        if (current.Start.HasValue && now < current.Start.Value.Date)
                        {
                            result = current;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public Iteration NextIteration
        {
            get
            {
                Iteration result = null;

                var iterations = GetIterations()?
                    .OrderBy(x => x.End)
                    .ToArray();
                if (iterations?.Any() ?? false)
                {
                    var count = iterations.Length;
                    var now = DateTime.Now.Date;

                    for (int i = 0; i < count; i++)
                    {
                        var current = iterations.ElementAt(i);
                        if (current.End.HasValue && now > current.End.Value.Date)
                        {
                            result = current;
                            break;
                        }
                    }
                }

                return result;
            }
        }
        #endregion

        #region Iterations Risk.
        public IPropertyType GetPropertyTypeIterationsRisk()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.DevOpsIterationRisks) ?? 
                         schema.AddPropertyType(Properties.Resources.DevOpsIterationRisks, PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.DevOpsIterationRisksDescription;
            }

            return result;
        }

        public float GetIterationRisk([NotNull] Iteration iteration)
        {
            var result = 0f;

            var propertyType = GetPropertyTypeIterationsRisk();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is IterationRisks iterationRisks)
                {
                    result = iterationRisks.Items?
                        .FirstOrDefault(x => string.CompareOrdinal(iteration.Id, x.IterationId) == 0)?
                        .Risk ?? 0f;
                }
            }

            return result;
        }

        public void SetIterationRisk([NotNull] Iteration iteration, float risk)
        {
            var propertyType = GetPropertyTypeIterationsRisk();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is IterationRisks iterationRisks)
                {
                    var iterationRisk =
                        iterationRisks.Items?.FirstOrDefault(x =>
                            string.CompareOrdinal(iteration.Id, x.IterationId) == 0);
                    if (iterationRisk != null)
                    {
                        iterationRisk.Risk = risk;
                    }
                    else
                    {
                        if (iterationRisks.Items == null)
                            iterationRisks.Items = new List<IterationRisk>();

                        iterationRisk = new IterationRisk(iteration, risk);
                        iterationRisks.Items.Add(iterationRisk);
                    }
                }
            }
        }
        #endregion
    }
}