using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Interfaces;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    internal class AssetPropertySchemaManager
    {
        private const string Name = "Assets";
        private const string SchemaName = "Assets";
        private const string Namespace = "http://example.com/";

        private readonly IThreatModel _model;

        public AssetPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Namespace);
            if (result == null)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Assets schema"))
                {
                    result = _model.AddSchema(SchemaName, Namespace);
                    result.AppliesTo = Scope.ThreatModel | Scope.ExternalInteractor | Scope.Process | Scope.DataStore;
                    result.AutoApply = false;
                    result.Priority = 10;
                    result.Visible = false;
                    result.System = true;
                    result.Description = "This is a description for the Property Schema.";
                    scope?.Complete();
                }
            }

            return result;
        }

        public IPropertyType AssetPropertyType
        {
            get
            {
                var schema = GetSchema();
                var result = schema?.GetPropertyType(Name);
                if (result == null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Add Assets Property Type"))
                    {
                        result = schema.AddPropertyType(Name, PropertyValueType.JsonSerializableObject);
                        result.Visible = false;
                        result.Description = "This is a description for the Property Type";
                        scope?.Complete();
                    }
                }

                return result;
            }
        }
    }
}
