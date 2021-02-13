using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("C082B942-5A6A-462A-9D02-EE7B20C1FBB1", "Diagram Schema Initializer", 10, ExecutionMode.Simplified)]
    public class DiagramSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new DiagramPropertySchemaManager(model);
            schemaManager.GetLinksSchema();
        }
    }
}