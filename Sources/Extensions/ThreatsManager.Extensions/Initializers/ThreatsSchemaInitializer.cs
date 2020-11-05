using System.ComponentModel.Composition;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "3FCE7492-5173-451B-90AD-4AB51EF8D1D4")]
    [ExportMetadata("Label", "Threats Schema Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class ThreatsSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new ThreatsPropertySchemaManager(model);
            schemaManager.GetSchema();
        }
    }
}