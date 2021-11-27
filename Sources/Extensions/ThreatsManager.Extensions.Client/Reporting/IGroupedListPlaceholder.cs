using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with lists grouped by a given property.
    /// </summary>
    public interface IGroupedListPlaceholder : IPlaceholder
    {
        /// <summary>
        /// The namespace of the Property Schema.
        /// </summary>
        string SchemaNamespace { get; set; }

        /// <summary>
        /// The name of the Property Schema.
        /// </summary>
        string SchemaName { get; set; }

        /// <summary>
        /// The name of the Property.
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// The name of the style to use for each grouped item.
        /// </summary>
        string ItemStyle { get; set; }

        /// <summary>
        /// If true, the placeholder must be shown in tabular form.
        /// </summary>
        /// <remarks>If the list is in tabular form, then each row shows the label, otherwise it is hidden.</remarks>
        bool Tabular { get; }

        /// <summary>
        /// Returns the list of properties which are shown by the placeholder. 
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Property Types included with the list, described as a Key Value pairs, where the key is a label for the property.</returns>
        IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties(IThreatModel model);

        /// <summary>
        /// Get the items to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Items to be represented in the list generation, grouped by the selected property.</returns>
        IEnumerable<KeyValuePair<string, IEnumerable<ListItem>>> GetList(IThreatModel model);
    }
}