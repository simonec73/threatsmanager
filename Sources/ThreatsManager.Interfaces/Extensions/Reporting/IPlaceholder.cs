using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions.Reporting
{
    /// <summary>
    /// Base interface for Placeholders.
    /// </summary>
    public interface IPlaceholder
    {
        /// <summary>
        /// Suffix for the Placeholder.
        /// </summary>
        string Suffix { get; }
    }
}