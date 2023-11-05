using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("F4A50612-74F0-483B-A0A0-FEC09486963B", "Associated Diagram Initializer", 10, ExecutionMode.Simplified)]
    public class AssociatedDiagramInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
            var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
        }
    }
}