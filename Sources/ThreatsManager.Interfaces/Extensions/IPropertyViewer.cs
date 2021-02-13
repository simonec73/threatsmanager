using System.Collections.Generic;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Viewer to show the the content of a specific Property.
    /// </summary>
    /// <remarks>The Property is shown as a combination of one or more Blocks.</remarks>
    public interface IPropertyViewer
    {
        /// <summary>
        /// Blocks to be shown.
        /// </summary>
        /// <remarks>Some Property Viewer types allow only a single Block.
        /// If the Context has not been set correctly, this property returns null.</remarks>
        IEnumerable<IPropertyViewerBlock> Blocks { get; }
    }
}