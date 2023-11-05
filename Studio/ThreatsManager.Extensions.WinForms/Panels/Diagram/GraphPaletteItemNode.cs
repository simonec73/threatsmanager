using System.Drawing;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public sealed class GraphPaletteItemNode : GoTextNode
    {
        public GraphPaletteItemNode([NotNull] PaletteItem paletteItem)
        {
            Item = paletteItem;

            Text = paletteItem.Name;
            Label = new GoText()
            {
                Text = paletteItem.Name,
                Selectable = false,
                Width = paletteItem.Width,
                Wrapping = false,
                StringTrimming = StringTrimming.EllipsisCharacter,
                TextColor = paletteItem.TextColor,
                FontSize = 9
            };
            Shape.BrushColor = paletteItem.BackColor;
            ToolTipText = paletteItem.Name;
        }

        public PaletteItem Item { get; }
    }
}