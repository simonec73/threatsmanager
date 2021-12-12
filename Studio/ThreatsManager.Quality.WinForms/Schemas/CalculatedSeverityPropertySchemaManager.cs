using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Schemas
{
    public class CalculatedSeverityPropertySchemaManager : IInitializableObject
    {
        private readonly IThreatModel _model;
        private static string SchemaName = "Calculated Severity";
        private static string PropertyName = "CalculatedSeverity";

        public CalculatedSeverityPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public bool IsInitialized => _model != null;

        [InitializationRequired]
        public bool IsCalculatedSeverityEnabled => _model.Schemas?
            .Any(x => string.CompareOrdinal(x.Name, SchemaName) == 0 &&
                      string.CompareOrdinal(x.Namespace,
                          Properties.Resources.DefaultNamespace) == 0) ?? false;

        [InitializationRequired]
        public bool AddSupport()
        {
            var result = false;

            if (!IsCalculatedSeverityEnabled)
            {
                var schema = GetSchema();
                _model.ApplySchema(schema.Id);
                result = true;
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveSupport()
        {
            var result = false;

            if (IsCalculatedSeverityEnabled)
            {
                var schema = GetSchema();
                result = _model.RemoveSchema(schema.Id, true);
            }

            return result;
        }

        [InitializationRequired]
        public IPropertySchema GetSchema()
        {
            var schema = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            schema.Description = Resources.CalculatedSeverityPropertySchemaDescription;
            schema.Visible = true;
            schema.AppliesTo = Scope.ThreatEvent;
            schema.System = true;
            schema.AutoApply = true;
            schema.NotExportable = true;
            schema.Priority = 500;

            var propertyType = schema.GetPropertyType(PropertyName) ?? schema.AddPropertyType(PropertyName, PropertyValueType.JsonSerializableObject);
            propertyType.DoNotPrint = true;
            propertyType.Visible = true;
            propertyType.CustomPropertyViewer = "Calculated Severity Property Viewer";
            propertyType.Description = Resources.CalculatedSeverityDescription;

            return schema;
        }

        #region Calculated Severity.
        [InitializationRequired]
        public IPropertyType GetCalculatedSeverityPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(PropertyName);
            }

            return result;
        }

        public CalculatedSeverityConfiguration GetSeverityCalculationConfig([NotNull] IThreatEvent threatEvent)
        {
            CalculatedSeverityConfiguration result = null;

            var propertyType = GetCalculatedSeverityPropertyType();
            if (propertyType != null)
            {
                var property = threatEvent.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is CalculatedSeverityConfiguration configuration)
                {
                    result = configuration;
                }
            }

            return result;
        }

        public CalculatedSeverityConfiguration SetSeverityCalculationConfig([NotNull] IThreatEvent threatEvent,
            int points, [Required] string reason)
        {
            CalculatedSeverityConfiguration result = null;

            var propertyType = GetCalculatedSeverityPropertyType();
            if (propertyType != null)
            {
                var property = threatEvent.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is CalculatedSeverityConfiguration configuration)
                {
                    result = configuration;
                }
                else
                {
                    result = new CalculatedSeverityConfiguration();
                    if (property == null)
                    {
                        property = threatEvent.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonObject)
                        {
                            jsonObject.Value = result;
                        }
                    }
                    else if (property is IPropertyJsonSerializableObject jsonObject)
                    {
                        jsonObject.Value = result;
                    }
                }

                result.Delta = points;
                result.DeltaReason = reason;
                result.DeltaSetBy = UserName.GetDisplayName();
                result.DeltaSetOn = DateTime.Now;
            }

            return result;
        }
        #endregion
    }
}
