using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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
    }
}
