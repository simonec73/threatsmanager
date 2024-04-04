using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Panels.Configuration;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Extension("83DE050B-333E-4843-93B4-A47143A0D0F6",
        "Threat Model enrichment with the Calculated Severity",
        100, ExecutionMode.Simplified)]
    public class CalculatedSeverityPostLoadProcessor : IPostLoadProcessor
    {
        public void Process(IThreatModel model)
        {
            var configuration = QualityConfigurationManager.GetInstance(model);
            if (configuration.EnableCalculatedSeverity)
            {
                var schemaManager = new CalculatedSeverityPropertySchemaManager(model);
                schemaManager.AddSupport();
            }
        }
    }
}
