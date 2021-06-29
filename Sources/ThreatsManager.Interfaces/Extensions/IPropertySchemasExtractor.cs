using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Extensions to extract the Property Schemas referenced by IPropertyJsonSerializableObjects.
    /// </summary>
    [ExtensionDescription("Property Schemas Extractor")]
    public interface IPropertySchemasExtractor : IExtension
    {
        /// <summary>
        /// Get Property Schemas referenced by the input object.
        /// </summary>
        /// <param name="jsonSerializableObject">Object to be analyzed.</param>
        /// <returns>List of the identified Property Schemas.</returns>
        IEnumerable<IPropertySchema> GetPropertySchemas(IPropertyJsonSerializableObject jsonSerializableObject);
    }
}
