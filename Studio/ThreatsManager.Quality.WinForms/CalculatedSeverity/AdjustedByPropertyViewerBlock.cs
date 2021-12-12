using System.Drawing;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class AdjustedByPropertyViewerBlock : IPropertyViewerBlock
    {
        public AdjustedByPropertyViewerBlock(CalculatedSeverityConfiguration config)
        {
            Text = config?.DeltaSetBy;
        }

        public bool Execute()
        {
            return false;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.SingleLineLabel;
        public string Label => "Adjusted By";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => false;
    }
}
