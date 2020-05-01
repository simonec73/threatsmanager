using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface implemented by the containers of Property Schemas.
    /// </summary>
    public interface IPropertySchemasContainer
    {
        /// <summary>
        /// Enumeration of Property Schemas.
        /// </summary>
        IEnumerable<IPropertySchema> Schemas { get; }

        /// <summary>
        /// Gets the Property Schema whose name and namespace is passed as argument.
        /// </summary>
        /// <param name="name">Name of the Schema.</param>
        /// <param name="nspace">Namespace of the Schema.</param>
        /// <returns>Property Schema if found, otherwise null.</returns>
        IPropertySchema GetSchema(string name, string nspace);

        /// <summary>
        /// Gets the Property Schema whose identifier is passed as argument.
        /// </summary>
        /// <param name="schemaId">Identifier of the Schema.</param>
        /// <returns>Property Schema if found, otherwise null.</returns>
        IPropertySchema GetSchema(Guid schemaId);

        /// <summary>
        /// Applies the selected Schema to all objects that are defined in the Threat Model
        /// and that have not yet received it. 
        /// </summary>
        /// <param name="schemaId">Identifier of the Schema.</param>
        void ApplySchema(Guid schemaId);

        /// <summary>
        /// Apply the AutoApply Properties Schemas to the specified Properties Container.  
        /// </summary>
        /// <param name="container">Container to be extended.</param>
        /// <returns>True if at least a Property Schema has been automatically applied, otherwise false.</returns>
        bool AutoApplySchemas(IPropertiesContainer container);

        /// <summary>
        /// Adds the Property Schema passed as argument to the container.
        /// </summary>
        /// <param name="propertySchema">Property Schema to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IPropertySchema propertySchema);

        /// <summary>
        /// Adds a new Property Schema to the container.
        /// </summary>
        /// <param name="name">Name of the Property Schema.</param>
        /// <param name="nspace">Namespace of the Property Schema.</param>
        /// <returns>New instance of the Property Schema.</returns>
        /// <remarks>If the container already contains a Property Schema with the same name and namespace,
        /// then the method will not create a new Property Schema and it will return null.</remarks>
        IPropertySchema AddSchema(string name, string nspace);

        /// <summary>
        /// Removes the Property Schema whose name and namespace is passed as argument.
        /// </summary>
        /// <param name="name">Name of the Schema.</param>
        /// <param name="nspace">Namespace of the Schema.</param>
        /// <param name="force">Boolean that indicates if the Property Schema must be removed even if it is in use. By default, it is false.</param>
        /// <returns>True if the Property Schema has been removed, otherwise false.</returns>
        bool RemoveSchema(string name, string nspace, bool force = false);
 
        /// <summary>
        /// Removes the Property Schema whose identifier is passed as argument.
        /// </summary>
        /// <param name="schemaId">Identifier of the Schema.</param>
        /// <param name="force">Boolean that indicates if the Property Schema must be removed even if it is in use. By default, it is false.</param>
        /// <returns>True if the Property Schema has been removed, otherwise false.</returns>
        bool RemoveSchema(Guid schemaId, bool force = false);

        /// <summary>
        /// Gets a Property Type from a Schema contained within the Container.
        /// </summary>
        /// <param name="propertyTypeId">Identifier of the Property Type.</param>
        /// <returns>Property Type if found, otherwise null.</returns>
        IPropertyType GetPropertyType(Guid propertyTypeId);
    }
}