using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace SampleWinFormExtensions.Schemas
{
    public class DefinitionsPropertySchemaManager
    {
        private const string Name = "Definitions";
        private const string SchemaName = "Definitions";
        private const string Namespace = "http://example.com/";

        private readonly IThreatModel _model;

        public DefinitionsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Namespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Namespace);
                result.AppliesTo = Scope.ThreatModel;
                result.AutoApply = false;
                result.Priority = 10;
                result.Visible = false;
                result.System = true;
                result.Description = "This is a description for the Property Schema.";
            }

            return result;
        }

        public IPropertyType DefinitionsPropertyType
        {
            get
            {
                var schema = GetSchema();
                var result = schema?.GetPropertyType(Name);
                if (result == null)
                {
                    result = schema.AddPropertyType(Name, PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.Description = "This is a description for the Property Type";
                }

                return result;
            }
        }
    }
}