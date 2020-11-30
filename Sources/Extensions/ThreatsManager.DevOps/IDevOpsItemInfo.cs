using System.Text.RegularExpressions;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Information related to an item stored in the DevOps system.
    /// </summary>
    public interface IDevOpsItemInfo
    {
        /// <summary>
        /// Identifier of the Item.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Name of the Item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Url of the Item.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Serialize the content of the object.
        /// </summary>
        /// <returns>Serialized object.</returns>
        string Serialize();
    }
}