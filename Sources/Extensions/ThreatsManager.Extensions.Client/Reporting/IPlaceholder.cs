using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Base interface for Placeholders.
    /// </summary>
    public interface IPlaceholder
    {
        /// <summary>
        /// Name of the Placeholder.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Label of the Placeholder.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Section of the Placeholder.
        /// </summary>
        PlaceholderSection Section { get; }

        /// <summary>
        /// Image associated to the Placeholder. 
        /// </summary>
        /// <remarks>It is optional. Size must be 16x16 pixels.</remarks>
        Bitmap Image { get; }
    }
}