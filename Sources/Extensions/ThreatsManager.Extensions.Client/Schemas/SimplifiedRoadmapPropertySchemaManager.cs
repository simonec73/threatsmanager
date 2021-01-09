using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Client.Schemas
{
    public class SimplifiedRoadmapPropertySchemaManager
    {
        private const string SchemaName = "Roadmap";
        private const string PropertyName = "Status";

        private readonly IThreatModel _model;

        public SimplifiedRoadmapPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            return _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(PropertyName);
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