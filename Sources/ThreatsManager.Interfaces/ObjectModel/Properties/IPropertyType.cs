using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property Type, which is the definition of the Property.
    /// </summary>
    public interface IPropertyType : IIdentity, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Identifier of the Schema to which the Property belongs to.
        /// </summary>
        Guid SchemaId { get; }

        /// <summary>
        /// Priority of the Property Type.
        /// </summary>
        /// <remarks>1 has the highest priority. This is required for visibility.</remarks>
        int Priority { get; set; }

        /// <summary>
        /// Flag that specifies if the Property Type is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Flag that specifies if the Property Type can be printed in Reports. 
        /// </summary>
        bool DoNotPrint { get; set; }

        /// <summary>
        /// Flag that specifies if the Property Type can be edited in the Item Editor. 
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Label of the Custom Property Viewer Extension to use to show the Property.
        /// </summary>
        /// <remarks>If not specified, the property will be shown using the standard viewer.</remarks>
        string CustomPropertyViewer { get; set; }
 
        /// <summary>
        /// Creates a duplicate of the current Property Type and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Property Type.</returns>
        IPropertyType Clone(IPropertyTypesContainer container);
    }
}