using System;
using System.Drawing;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class WarningMarker : GoImage
    {
        public WarningMarker() {
            Printable = false;
            Resizable = false;
            Deletable = false;
            Copyable = false;
            Selectable = false;
            Image = Properties.Resources.report_problem_big;
            Size = new SizeF(32.0f, 32.0f);
            Visible = false;
        }

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

        public string Tooltip { get; set; }

        public override string GetToolTip(GoView view)
        {
            return Tooltip;
        }
    }
}