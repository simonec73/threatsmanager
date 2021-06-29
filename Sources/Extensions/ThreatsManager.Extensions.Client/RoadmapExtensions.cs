using System;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions
{
    public static class RoadmapExtensions
    {
        public static RoadmapStatus GetStatus(this IMitigation mitigation)
        {
            return mitigation.GetStatus(out var automatedCalculation);
        }

        public static RoadmapStatus GetStatus(this IMitigation mitigation, out bool automatedCalculation)
        {
            var result = GetStatusFromProperties(mitigation);
            automatedCalculation = false;

            var tems = mitigation.GetThreatEventMitigations();
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
                        automatedCalculation = true;
                    }
                }
            }

            return result;
        }

        public static void SetStatus(this IMitigation mitigation, RoadmapStatus status)
        {
            var model = mitigation?.Model;

            if (model != null)
            {
                var schemaManager = new RoadmapPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                if (propertyType != null)
                {
                    var property = mitigation.GetProperty(propertyType);
                    if (property == null)
                    {
                        mitigation.AddProperty(propertyType, status.ToString());
                    }
                    else
                    {
                        property.StringValue = status.ToString();
                    }
                }
            }
        }

        private static RoadmapStatus GetStatusFromProperties(IMitigation mitigation)
        {
            RoadmapStatus result = RoadmapStatus.NotAssessed;

            var model = mitigation?.Model;

            if (model != null)
            {
                var schemaManager = new RoadmapPropertySchemaManager(model);
                var propertyType = schemaManager.GetPropertyType();
                if (propertyType != null)
                {
                    var property = mitigation.GetProperty(propertyType);
                    if (property != null &&
                        Enum.TryParse<RoadmapStatus>(property.StringValue, out var status))
                    {
                        result = status;
                    }
                }
            }

            return result;
        }
    }
}
