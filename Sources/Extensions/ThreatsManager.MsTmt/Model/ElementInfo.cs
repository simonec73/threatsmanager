using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public class ElementInfo : Info
    {
        public ElementInfo([Required] string page, int top, int left, int width, int height, 
            ElementType elementType, string typeId,
            [NotNull] IEnumerable<XmlNode> properties) : base(properties)
        {
            Page = page;
            Position = new PointF(left, top);
            Size = new SizeF(width, height);
            ElementType = elementType;
            TypeId = typeId;
        }

        public ElementType ElementType { get; private set; }

        public string TypeId { get; private set; }

        public string Page { get; private set; }

        public PointF Position { get; private set; }

        public SizeF Size { get; private set; }
    }
}
