using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing multiple values selected from the associated Property Type.
    /// </summary>
    public interface IPropertyListMulti : IProperty
    {
        /// <summary>
        /// Selected Values for the property.
        /// </summary>
        IEnumerable<IListItem> Values { get; set; }
    }
}