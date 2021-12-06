using System.Drawing;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class AdjustedOnPropertyViewerBlock : IPropertyViewerBlock
    {
        public AdjustedOnPropertyViewerBlock(CalculatedSeverityConfiguration config)
        {
            Text = config?.DeltaSetOn.ToLongDateString();
        }

        public bool Execute()
        {
            return false;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.SingleLineLabel;
        public string Label => "Adjusted On";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => false;
    }
}
