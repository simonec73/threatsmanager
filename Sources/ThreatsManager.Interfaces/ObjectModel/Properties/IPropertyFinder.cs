using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface used to find a property.
    /// </summary>
    public interface IPropertyFinder
    {
        /// <summary>
        /// Finds a property given its Identifier.
        /// </summary>
        /// <param name="id">Identifier of the Property.</param>
        /// <returns>Property, if found, otherwise null.</returns>
        /// <remarks>The property is searched in any object associated to the Property Finder.</remarks>
        IProperty FindProperty(Guid id);
    }
}