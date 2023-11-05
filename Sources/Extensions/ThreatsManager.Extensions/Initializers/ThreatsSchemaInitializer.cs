using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("3FCE7492-5173-451B-90AD-4AB51EF8D1D4", "Threats Schema Initializer", 10, ExecutionMode.Simplified)]
    public class ThreatsSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            using (var scope = UndoRedoManager.OpenScope("Initialize Threats Property Schema"))
            {
                var schemaManager = new ThreatsPropertySchemaManager(model);
                schemaManager.GetSchema();

                scope?.Complete();
            }
        }
    }
}