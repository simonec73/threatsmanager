using System;
using System.Drawing;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class AdjustFactorPropertyViewerBlock : IPropertyViewerBlock
    {
        public AdjustFactorPropertyViewerBlock(CalculatedSeverityConfiguration config)
        {
            if (config != null)
            {
                Text = Enum.IsDefined(typeof(DeltaValue), config.Delta)
                    ? ((DeltaValue) config.Delta).GetEnumLabel()
                    : $"{config.Delta} points";
            }
        }

        public bool Execute()
        {
            return false;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.SingleLineLabel;
        public string Label => "Adjustment Factor";
        public string Text { get; set; }
        public Bitmap Image { get; }
        public bool Printable => false;
    }
}
