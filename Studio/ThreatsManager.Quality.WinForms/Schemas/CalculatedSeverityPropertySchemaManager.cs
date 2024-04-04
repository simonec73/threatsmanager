using System;
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
        private readonly QualityConfigurationManager _config;
        private static string SchemaName = "Calculated Severity";
        private static string PropertyName = "CalculatedSeverity";

        public CalculatedSeverityPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
            _config = QualityConfigurationManager.GetInstance(model);
        }

        public bool IsInitialized => _model != null;

        public bool IsCalculatedSeverityEnabled => _config?.EnableCalculatedSeverity ?? false;

        [InitializationRequired]
        public void AddSupport()
        {
            var schema = GetSchema();
            _model.ApplySchema(schema.Id);
        }

        [InitializationRequired]
        public void RemoveSupport()
        {
            var schema = GetSchema();
            _model.RemoveSchema(schema.Id, true);
        }

        [InitializationRequired]
        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{SchemaName}' schema"))
            {
                result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.Description = Resources.CalculatedSeverityPropertySchemaDescription;
                result.Visible = true;
                result.AppliesTo = Scope.ThreatEvent;
                result.System = true;
                result.AutoApply = true;
                result.NotExportable = true;
                result.Priority = 500;

                var propertyType = result.GetPropertyType(PropertyName) ?? result.AddPropertyType(PropertyName, PropertyValueType.JsonSerializableObject);
                propertyType.DoNotPrint = true;
                propertyType.Visible = true;
                propertyType.CustomPropertyViewer = "Calculated Severity Property Viewer";
                propertyType.Description = Resources.CalculatedSeverityDescription;

                scope?.Complete();
            }

            return result;
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

            using (var scope = UndoRedoManager.OpenScope("Set Severity Calculation Config"))
            {
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

                    scope?.Complete();
                }
            }

            return result;
        }
        #endregion
    }
}
