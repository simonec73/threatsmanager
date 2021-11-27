using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.DevOps.Review
{
    public class BacklogReviewTextPropertyViewerBlock : IPropertyViewerBlock
    {
        public BacklogReviewTextPropertyViewerBlock([NotNull] ReviewInfo info)
        {
            Text = info.Text;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.String;
        public string Label => "Backlog Review";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => true;

        public bool Execute()
        {
            return false;
        }
    }
}