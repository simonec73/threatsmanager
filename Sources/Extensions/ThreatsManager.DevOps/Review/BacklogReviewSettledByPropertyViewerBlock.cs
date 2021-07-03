using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.DevOps.Review
{
    public class BacklogReviewSettledByPropertyViewerBlock : IPropertyViewerBlock
    {
        public BacklogReviewSettledByPropertyViewerBlock([NotNull] ReviewInfo info)
        {
            Text = info.SettledBy;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.SingleLineString;
        public string Label => "Settled By";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => true;

        public bool Execute()
        {
            return false;
        }
    }
}