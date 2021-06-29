using System.Collections.Generic;
using System.ComponentModel;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with lists.
    /// </summary>
    [ExtensionDescription("Reporting Placeholder for Lists")]
    public interface IListPlaceholder : IPlaceholder, IExtension
    {
        /// <summary>
        /// If true, the placeholder must be shown in tabular form.
        /// </summary>
        /// <remarks>If the list is in tabular form, then each row shows the label, otherwise it is hidden.</remarks>
        bool Tabular { get; }

        /// <summary>
        /// Get the items to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Items to be represented in the list generation.</returns>
        IEnumerable<ListItem> GetList(IThreatModel model);
    }
}
