using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Extensions.Relationships;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Schemas
{
    public class RelationshipSchemaManager
    {
        private const string SchemaName = "Mitigation Relationships";

        private readonly IThreatModel _model;

        public RelationshipSchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{SchemaName}' schema"))
            {
                result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.Mitigation;
                result.AutoApply = false;
                result.Priority = 20;
                result.Visible = false;
                result.System = true;
                result.NotExportable = false;
                result.Description = Resources.RelationshipSchemaDescription;
                scope?.Complete();
            }

            return result;
        }

        private IPropertyType GetAlternativesPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Alternative Mitigations Property Type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType("Alternatives") ?? schema.AddPropertyType("Alternatives", PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.Alernatives;
                    scope?.Complete();
                }
            }

            return result;
        }

        private IPropertyType GetComplementaryPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Complementary Property Type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType("Complementary") ?? schema.AddPropertyType("Complementary", PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.Alernatives;
                    scope?.Complete();
                }
            }

            return result;
        }

        public RelationshipDetails GetAlternatives(IMitigation mitigation)
        {
            RelationshipDetails result = null;

            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Get Alternatives"))
                {
                    var propertyType = GetAlternativesPropertyType();
                    if (propertyType != null)
                    {
                        var property = mitigation.GetProperty(propertyType);
                        if (property != null && 
                            property is IPropertyJsonSerializableObject serializableObject &&
                            serializableObject.Value is RelationshipDetails details)
                        {
                            result = details;
                            scope?.Complete();
                        }
                    }
                }
            }

            return result;
        }

        public RelationshipDetails GetComplementary(IMitigation mitigation)
        {
            RelationshipDetails result = null;

            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Get Complementary"))
                {
                    var propertyType = GetComplementaryPropertyType();
                    if (propertyType != null)
                    {
                        var property = mitigation.GetProperty(propertyType);
                        if (property != null &&
                            property is IPropertyJsonSerializableObject serializableObject &&
                            serializableObject.Value is RelationshipDetails details)
                        {
                            result = details;
                            scope?.Complete();
                        }
                    }
                }
            }

            return result;
        }

        public void SetAlternatives(IMitigation mitigation, RelationshipDetails details)
        {
            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Alternatives"))
                {
                    var propertyType = GetAlternativesPropertyType();
                    if (propertyType != null)
                    {
                        var property = mitigation.GetProperty(propertyType) ??
                            mitigation.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject serializableObject)
                        {
                            serializableObject.Value = details;
                            scope?.Complete();
                        }
                    }
                }
            }
        }

        public void SetComplementary(IMitigation mitigation, RelationshipDetails details)
        {
            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Complementary"))
                {
                    var propertyType = GetComplementaryPropertyType();
                    if (propertyType != null)
                    {
                        var property = mitigation.GetProperty(propertyType) ??
                            mitigation.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject serializableObject)
                        {
                            serializableObject.Value = details;
                            scope?.Complete();
                        }
                    }
                }
            }
        }
    }
}
