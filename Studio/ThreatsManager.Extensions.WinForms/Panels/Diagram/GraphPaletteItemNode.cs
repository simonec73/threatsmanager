using System.Drawing;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;

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
            ToolTipText = paletteItem.Tooltip;
        }

        public PaletteItem Item { get; }
    }
}