using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Panels.Configuration;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Extension("8CAFC5C4-339A-4782-88F8-FA4C3650AB06", 
        "Automatic initialization of the Calculated Severity",
        100, ExecutionMode.Simplified)]
    public class CalculatedSeverityInitializer : IInitializer
    {
        public void Initialize(IThreatModel model)
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
