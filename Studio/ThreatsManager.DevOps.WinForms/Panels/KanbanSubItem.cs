using System.Drawing;
using DevComponents.DotNetBar;
using Northwoods.Go;

namespace ThreatsManager.DevOps.Panels
{
    public class KanbanSubItem : GoListGroup 
    {
        public KanbanSubItem() 
        {
            Selectable = false;
            Deletable = false;
            Orientation =  System.Windows.Forms.Orientation.Horizontal;
            TopLeftMargin = new SizeF(0, 0);
            BottomRightMargin = new SizeF(0, 0);

            var img = new GoImage
            {
                Selectable = false,
                AutoRescales = false,
                AutoResizes = false,
                Size = new SizeF(16 * Dpi.Factor.Width, 16 * Dpi.Factor.Height)
            };
            Add(img);
            var t = new GoText
            {
                FontSize = 9,
                AutoResizes = false,
                Selectable = false,
                Editable = false,
                Multiline = true,
                Wrapping = true,
                WrappingWidth = 150
            };
            Add(t);
            var t2 = new GoText
            {
                FontSize = 9,
                Selectable = false,
                Editable = false,
                Multiline = true,
                Wrapping = true,
                WrappingWidth = 200
            };
            Add(t2);
        }

        // parts
        public GoImage Image => (GoImage)this[0];

        public GoText Label => (GoText)this[1];

        public GoText Value => (GoText)this[2];
    }
}