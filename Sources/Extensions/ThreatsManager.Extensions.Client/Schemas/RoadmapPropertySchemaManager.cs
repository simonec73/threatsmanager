using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

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

        public RoadmapStatus GetStatus(IMitigation mitigation)
        {
            var result = GetStatusFromProperties(mitigation);

            var tems = mitigation.GetThreatEventMitigations()?.ToArray();
            if (!(tems?.Any() ?? false))
                result = RoadmapStatus.NotAssessed;

            if (result == RoadmapStatus.NotAssessed)
            {
                if (tems?.Any() ?? false)
                {
                    if (tems.All(x =>
                        x.Status == MitigationStatus.Existing || x.Status == MitigationStatus.Implemented))
                    {
                        result = RoadmapStatus.NoActionRequired;
                    }
                }
            }

            return result;
        }

        private RoadmapStatus GetStatusFromProperties(IMitigation mitigation)
        {
            RoadmapStatus result = RoadmapStatus.NotAssessed;

            var propertyType = GetPropertyType();
            if (propertyType != null)
            {
                var property = mitigation.GetProperty(propertyType);
                if (property != null &&
                    Enum.TryParse<RoadmapStatus>(property.StringValue, out var status))
                {
                    result = status;
                }
            }

            return result;
        }
    }
}