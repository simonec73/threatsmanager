using System.ComponentModel.Composition;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "048EEEB8-77AE-444E-9AE3-EDF6D13AF371")]
    [ExportMetadata("Label", "Capec Schema Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Expert)]
    public class CapecSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new CapecPropertySchemaManager(model);
            schemaManager.GetSchema();
        }
    }
}