using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.DevOps.Review
{
    public class BacklogReviewSettledOnPropertyViewerBlock : IPropertyViewerBlock
    {
        public BacklogReviewSettledOnPropertyViewerBlock([NotNull] ReviewInfo info)
        {
            Text = info.SettledOn.ToString();
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.DateTimePicker;
        public string Label => "Settled On";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => true;

        public bool Execute()
        {
            return false;
        }
    }
}