using System;
using System.Collections.Generic;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public class FlowInfo : Info
    {
        public FlowInfo(Guid flowId, Guid sourceId, Guid targetId, string typeId,
            [Required] string pageName, IEnumerable<XmlNode> properties) : base(properties)
        {
            FlowId = flowId;
            SourceGuid = sourceId;
            TargetGuid = targetId;
            TypeId = typeId;
            PageName = pageName;
        }

        public string TypeId { get; private set; } 
        public Guid FlowId { get; private set; }
        public Guid SourceGuid { get; private set; }
        public Guid TargetGuid { get; private set; }
        public string PageName { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
