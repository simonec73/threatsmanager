using System.Drawing;
using DevComponents.DotNetBar;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public class RoadmapSubItem : GoListGroup 
    {
        public RoadmapSubItem() 
        {
            Selectable = false;
            Deletable = false;
            Orientation =  System.Windows.Forms.Orientation.Horizontal;
            TopLeftMargin = new SizeF(0, 0);
            BottomRightMargin = new SizeF(0, 0);

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

        public GoText Label => (GoText)this[0];

        public GoText Value => (GoText)this[1];
    }
}