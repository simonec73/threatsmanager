using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Extensions to update Property Schemas referenced by IPropertyJsonSerializableObjects.
    /// </summary>
    [ExtensionDescription("Property Schema Updater")]
    public interface IPropertySchemasUpdater : IExtension
    {
        /// <summary>
        /// Checks if the Property Schema is referenced anywhere.
        /// </summary>
        /// <param name="model">Threat Model which needs to be evaluated.</param>
        /// <param name="schemaName">Name of the Property Schema.</param>
        /// <param name="nsName">Namespace of the Property Schema.</param>
        /// <returns></returns>
        bool HasPropertySchema(IThreatModel model, string schemaName, string nsName);

        /// <summary>
        /// Checks if the Property Type is referenced anywhere.
        /// </summary>
        /// <param name="model">Threat Model which needs to be evaluated.</param>
        /// <param name="schemaName">Name of the Property Schema.</param>
        /// <param name="nsName">Namespace of the Property Schema.</param>
        /// <param name="propertyName">Name of the Property Type.</param>
        /// <returns></returns>
        bool HasPropertyType(IThreatModel model, string schemaName, string nsName, string propertyName);

        /// <summary>
        /// Modify references to a Property Schema.
        /// </summary>
        /// <param name="model">Threat Model which needs to be updated.</param>
        /// <param name="oldName">Old name of the Property Schema.</param>
        /// <param name="oldNamespace">Old namespace of the Property Schema.</param>
        /// <param name="newName">New name of the Property Schema.</param>
        /// <param name="newNamespace">New namespace of the Property Schema.</param>
        /// <returns>True if the change occurred successfully, false otherwise.</returns>
        bool UpdateSchemaName(IThreatModel model, string oldName, string oldNamespace, string newName, string newNamespace);

        /// <summary>
        /// Modify references to a Property Type.
        /// </summary>
        /// <param name="model">Threat Model which needs to be updated.</param>
        /// <param name="schemaName">Name of the Property Schema containing the Property Type.</param>
        /// <param name="schemaNamespace">Extensions of the Property Schema containing the Property Type.</param>
        /// <param name="oldPropertyTypeName">Old name of the Property Type.</param>
        /// <param name="newPropertyTypeName">New name of the Property Type.</param>
        /// <returns>True if the change occurred successfully, false otherwise.</returns>
        bool UpdatePropertyTypeName(IThreatModel model, string schemaName, string schemaNamespace, 
            string oldPropertyTypeName, string newPropertyTypeName);
    }
}