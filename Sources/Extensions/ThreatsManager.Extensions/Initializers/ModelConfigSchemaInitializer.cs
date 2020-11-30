using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("6E637EFC-7AFC-4895-8794-F650DA4ED2FF", "Threat Model Configuration Schema Initializer", 10, ExecutionMode.Simplified)]
    public class ModelConfigSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new ModelConfigPropertySchemaManager(model);
            var schema = schemaManager.GetSchema();

            var horizontalPT = schema.GetPropertyType("Diagram Layout Horizontal Spacing");
            var horizontal = model.GetProperty(horizontalPT);
            if (horizontal == null)
            {
                model.AddProperty(horizontalPT, "200");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(horizontal.StringValue))
                    horizontal.StringValue = "200";
            }

            var verticalPT = schema.GetPropertyType("Diagram Layout Vertical Spacing");
            var vertical = model.GetProperty(verticalPT);
            if (vertical == null)
            {
                model.AddProperty(verticalPT, "100");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(vertical.StringValue))
                    vertical.StringValue = "100";
            }
        }
    }
}