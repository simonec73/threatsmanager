using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public class ThreatSourceNodeParentChild
    {
        public ThreatSourceNodeParentChild([NotNull] ThreatSource threatSource, 
            [Required] string viewId, [NotNull] ThreatSourceNode node)
        {
            Node = node;
            var children = threatSource.GetChildren(viewId, node);
            if (children != null)
                Children = children.Select(x => new ThreatSourceNodeParentChild(threatSource, viewId, x));
        }

        public ThreatSourceNode Node { get; private set; }
        public IEnumerable<ThreatSourceNodeParentChild> Children { get; private set; }

        public override string ToString()
        {
            return Node?.Name ?? base.ToString();
        }
    }
}