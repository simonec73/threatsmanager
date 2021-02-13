using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by List Providers, which provide values to IPropertyLists
    /// </summary>
    [ExtensionDescription("List Provider")]
    public interface IListProviderExtension : IExtension
    {
        /// <summary>
        /// Get the enumeration of the available values.
        /// </summary>
        /// <param name="context">Parameter used to contextualize the retrieval of the Available items.</param>
        /// <remarks>The parameter is typically provided by the IListPropertyType or IListMultiPropertyType.</remarks>
        IEnumerable<IListItem> GetAvailableItems(string context);
    }
}