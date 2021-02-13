using System;
using System.Drawing;
using DevComponents.DotNetBar;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public class GraphRealLink : GoLink {
        public GraphRealLink()
        {
            this.AdjustingStyle = GoLinkAdjustingStyle.Scale;
        }

        // Whenever the user resizes a link interactively, don't automatically recalculate
        // the stroke all the time.
        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint,
            int whichHandle, GoInputState evttype, SizeF min, SizeF max) {
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            this.AdjustingStyle = GoLinkAdjustingStyle.Scale;
        }

        public override IGoHandle CreateResizeHandle(int handleid)
        {
            var result = base.CreateResizeHandle(handleid) as GoHandle;
            if (handleid != 0)
                result.Size = new SizeF(6 * Dpi.Factor.Width, 6 * Dpi.Factor.Height);

            return result;
        }
    }
}