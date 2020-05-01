using System.Drawing;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Container for images.
    /// </summary>
    public interface IImagesContainer
    {
        /// <summary>
        /// Custom big-sized image.
        /// </summary>
        /// <remarks>The size is 64x64 bits.</remarks>
        Bitmap BigImage { get; set; }

        /// <summary>
        /// Custom mid-sized image.
        /// </summary>
        /// <remarks>The size is 32x32 bits.</remarks>
        Bitmap Image { get; set; }

        /// <summary>
        /// Custom small-sized image.
        /// </summary>
        /// <remarks>The size is 16x16 bits.</remarks>
        Bitmap SmallImage { get; set; }
    }
}