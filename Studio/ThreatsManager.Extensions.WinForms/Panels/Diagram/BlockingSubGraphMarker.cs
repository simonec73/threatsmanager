using System;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class BlockingSubGraphMarker : GoRectangle, IDisposable
    {
        public BlockingSubGraphMarker() 
        {
            Printable = false;
            Resizable = false;
            Deletable = false;
            Copyable = false;
            Selectable = false;
            Brush = null;
            PenColor = Color.LightGray;
            Visible = (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);

            MarkerStatusTrigger.MarkerStatusUpdated += MarkerStatusTriggerOnMarkerStatusUpdated;
        }

        public void Dispose()
        {
            MarkerStatusTrigger.MarkerStatusUpdated -= MarkerStatusTriggerOnMarkerStatusUpdated;
        }

        private void MarkerStatusTriggerOnMarkerStatusUpdated(MarkerStatus status)
        {
            Visible = (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
        }

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

        // only seen when the mouse is over it
        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view) {
            if (from == this)
            {
                if (Parent.Selectable)
                    Brush = null;
                else
                    BrushColor = Color.LightGray;
            }
            if (to == this)
                BrushColor = Parent.Selectable ? Color.LightGreen : Color.Gray;
            return true;
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            if (!evt.Alt && !evt.Control && !evt.DoubleClick && !evt.Shift && (evt.Buttons == MouseButtons.Left))
            {
                Parent.Selectable = !Parent.Selectable;
            }

            return base.OnSingleClick(evt, view);
        }
    }
}