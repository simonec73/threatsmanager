using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with lists grouped by a given property.
    /// </summary>
    public interface IGroupedListPlaceholder : IListPlaceholder
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
    }
}