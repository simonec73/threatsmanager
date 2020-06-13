using System.ComponentModel.Composition;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.MsTmt.Schemas;

namespace ThreatsManager.MsTmt.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "35AC4E48-AEEC-48CA-922F-60EA0E2BD18F")]
    [ExportMetadata("Label", "Microsoft Threat Modeling Tool Schema Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class MsTmtSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var entitiesPropertySchemaManager = new EntitiesPropertySchemaManager(model);
            entitiesPropertySchemaManager.GetSchema();

            var dataFlowsPropertySchemaManager = new DataFlowsPropertySchemaManager(model);
            dataFlowsPropertySchemaManager.GetSchema();

            var threatsPropertySchemaManager = new ThreatsPropertySchemaManager(model);
            threatsPropertySchemaManager.GetSchema();
        }
    }
}