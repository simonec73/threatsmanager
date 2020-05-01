using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property Type specialized for lists that allow selecting a single value.
    /// </summary>
    public interface IListPropertyType : IPropertyType
    {
        /// <summary>
        /// Enumeration of the possible values that can be assigned.
        /// </summary>
        IEnumerable<IListItem> Values { get; }

        /// <summary>
        /// Additional parameter used by the List Provider to get context info about its operation.
        /// </summary>
        string Context { get; set; }

        /// <summary>
        /// Set the provider of the values to be used by the property.
        /// </summary>
        /// <param name="listProvider">Provider of the available values.</param>
        void SetListProvider(IListProviderExtension listProvider);
    }
}