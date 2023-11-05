using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using System.Drawing;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
    [Recordable(AutoRecord = false)]
    public class RecordablePointF
    {
        private RecordablePointF() { }

        public RecordablePointF(PointF point)
        {
            Position = point;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public PointF Position
        {
            get => new PointF(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
    }
}
