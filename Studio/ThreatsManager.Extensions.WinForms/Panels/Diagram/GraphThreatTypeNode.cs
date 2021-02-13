using System.Drawing;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public sealed class GraphThreatTypeNode : GoTextNode
    {
        public GraphThreatTypeNode([NotNull] IThreatType threatType)
        {
            ThreatType = threatType;

            Text = threatType.Name;
            Label = new GoText()
            {
                Text = threatType.Name,
                Selectable = false,
                Width = 500,
                Wrapping = false,
                StringTrimming = StringTrimming.EllipsisCharacter,
                TextColor = Color.White,
                FontSize = 9
            };
            Shape.BrushColor = ThreatModelManager.ThreatsColor;
            ToolTipText = threatType.Name;
        }

        public IThreatType ThreatType { get; }
    }
}