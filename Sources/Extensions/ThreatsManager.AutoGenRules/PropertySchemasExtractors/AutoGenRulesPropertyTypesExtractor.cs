using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoGenRules.PropertySchemasExtractors
{
    [Extension("276398CB-9D5B-4D89-B8FD-BF1B61CD98D6", "Auto Gen Rules Property Schema Extractor", 20, ExecutionMode.Simplified)]
    public class AutoGenRulesPropertyTypesExtractor : IPropertySchemasExtractor
    {
        public IEnumerable<IPropertySchema> GetPropertySchemas([NotNull] IPropertyJsonSerializableObject jsonSerializableObject)
        {
            IEnumerable<IPropertySchema> result = null;

            if (jsonSerializableObject.Value is SelectionRule selectionRule && selectionRule.Root != null)
            {
                result = jsonSerializableObject.Model.Traverse(selectionRule.Root);
            }

            return result;
        }
    }
}
