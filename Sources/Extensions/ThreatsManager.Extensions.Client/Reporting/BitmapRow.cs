using System.Drawing;

namespace ThreatsManager.Extensions.Reporting
{
    public class BitmapRow : ItemRow
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label of the Row.</param>
        /// <param name="bitmap">Image to be shown.</param>
        /// <param name="caption">[Optional] Caption of the image.</param>
        public BitmapRow(string label, Bitmap bitmap, string caption = null) : base(label)
        {
            Image = bitmap;
            Caption = caption;
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public override bool Visible => Image != null;

        /// <summary>
        /// Image of the Row.
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// Caption of the Image.
        /// </summary>
        public string Caption { get; private set; }
    }
}
