using PostSharp.Patterns.Recording;
using System.Drawing;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
    [Recordable]
    public class RecordablePointF
    {
        private RecordablePointF() { }

        public RecordablePointF(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
