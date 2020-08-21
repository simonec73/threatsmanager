using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace ThreatsManager.MsTmt.Model
{
    public class ElementTypeInfo : TypeInfo
    {
        public ElementTypeInfo(ElementType elementType, string typeId, string parentTypeId, string name, string description, 
            Bitmap image, IEnumerable<XmlNode> properties, bool isGeneric, bool isTemplate) : base(properties, isTemplate)
        {
            ElementType = elementType;
            TypeId = typeId;
            ParentTypeId = parentTypeId;
            Name = name;
            Description = description;
            Image = image;
            IsGeneric = isGeneric;
        }

        public ElementType ElementType { get; private set; }

        public string TypeId { get; private set; }

        public string ParentTypeId { get; private set; }

        public string Name { get; private set; }

        public Bitmap Image { get; private set; }

        public string Description { get; private set; }

        public bool IsGeneric { get; private set; }
    }
}