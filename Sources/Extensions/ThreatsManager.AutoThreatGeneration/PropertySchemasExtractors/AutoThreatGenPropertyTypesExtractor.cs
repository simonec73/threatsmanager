using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoThreatGeneration.PropertySchemasExtractors
{
    [Extension("276398CB-9D5B-4D89-B8FD-BF1B61CD98D6", "Auto Threat Gen Property Schema Extractor", 20, ExecutionMode.Simplified)]
    public class AutoThreatGenPropertyTypesExtractor : IPropertySchemasExtractor
    {
        public IEnumerable<IPropertySchema> GetPropertySchemas([NotNull] IPropertyJsonSerializableObject jsonSerializableObject)
        {
            IEnumerable<IPropertySchema> result = null;

            if (jsonSerializableObject.Value is SelectionRule selectionRule)
            {
                result = jsonSerializableObject.Model.Traverse(selectionRule.Root);
            }

            return result;
        }
    }
}
