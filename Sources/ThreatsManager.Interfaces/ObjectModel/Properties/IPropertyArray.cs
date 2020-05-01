using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing an array of values, serialized as strings.
    /// </summary>
    public interface IPropertyArray : IProperty
    {
        /// <summary>
        /// Value of the Property.
        /// </summary>
        IEnumerable<string> Value { get; set; }
    }
}