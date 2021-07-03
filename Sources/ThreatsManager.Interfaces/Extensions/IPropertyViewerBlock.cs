using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// An Action 
    /// </summary>
    public interface IPropertyViewerBlock
    {
        /// <summary>
        /// Type of Block to be shown.
        /// </summary>
        PropertyViewerBlockType BlockType { get; }

        /// <summary>
        /// Label of the Block.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Text of the Block.
        /// </summary>
        /// <remarks>For some Block Types, this is editable.</remarks>
        string Text { get; set; }

        /// <summary>
        /// Bitmap of the Block.
        /// </summary>
        /// <remarks>It is typically 64x64 pixels.</remarks>
        Bitmap Image { get; }

        /// <summary>
        /// True if the block can be printed.
        /// </summary>
        bool Printable { get; }

        /// <summary>
        /// Executes the Action associated with the Block.
        /// </summary>
        /// <returns>True if the execution succeeded, false otherwise.</returns>
        bool Execute();
    }
}