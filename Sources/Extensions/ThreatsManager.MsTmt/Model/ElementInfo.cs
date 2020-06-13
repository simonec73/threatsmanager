using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public class ElementInfo : Info
    {
        public ElementInfo([Required] string page, int top, int left, [NotNull] IEnumerable<XmlNode> nodes, ElementType elementType) : base(nodes)
        {
            Page = page;
            Position = new PointF(left, top);
            ElementType = elementType;
        }

        public ElementType ElementType { get; private set; }

        public string Page { get; private set; }

        public PointF Position { get; private set; }
    }
}
