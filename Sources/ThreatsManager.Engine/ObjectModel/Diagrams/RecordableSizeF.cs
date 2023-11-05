using PostSharp.Patterns.Recording;
using System.Drawing;
using System.Windows;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
    [Recordable(AutoRecord = false)]
    public class RecordableSizeF
    {
        private RecordableSizeF() { }

        public RecordableSizeF(SizeF size)
        {
            Height = size.Height;
            Width = size.Width;
        }

        public float Height { get; set; }
        public float Width { get; set; }
        public SizeF Size 
        {
            get => new SizeF(Width, Height);
            set
            {
                Height = value.Height;
                Width = value.Width;
            }
        }
    }
}
