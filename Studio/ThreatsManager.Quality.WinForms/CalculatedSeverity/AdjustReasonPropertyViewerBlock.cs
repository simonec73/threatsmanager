using System.Drawing;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class AdjustReasonPropertyViewerBlock : IPropertyViewerBlock
    {
        public AdjustReasonPropertyViewerBlock(CalculatedSeverityConfiguration config)
        {
            Text = config?.DeltaReason;
        }

        public bool Execute()
        {
            return false;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.SingleLineLabel;
        public string Label => "Reason";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => false;
    }
}
