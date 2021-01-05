using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class RoadmapPropertySchemaManager
    {
        private const string SchemaName = "Roadmap";
        private const string PropertyName = "Status";

        private readonly IThreatModel _model;

        public RoadmapPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.Description = Resources.RoadmapPropertySchemaDescription;
            result.Visible = false;
            result.System = true;
            result.Priority = 10;
            result.AutoApply = false;
            result.NotExportable = true;
            result.AppliesTo = Scope.Mitigation;

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(PropertyName) ?? schema.AddPropertyType(PropertyName, PropertyValueType.SingleLineString);
                result.Description = Resources.PropertyRoadmap;
                result.Visible = false;
                result.DoNotPrint = true;
            }

            return result;
        }
    }
}