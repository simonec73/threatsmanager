using System;
using System.Collections.Generic;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public class FlowInfo : Info
    {
        public FlowInfo(string name) : base(name)
        {
            
        }

        public FlowInfo(Guid flowId, Guid sourceId, Guid targetId, 
            [Required] string pageName, IEnumerable<XmlNode> nodes) : base(nodes)
        {
            FlowId = flowId;
            SourceGuid = sourceId;
            TargetGuid = targetId;
            PageName = pageName;

        }

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
