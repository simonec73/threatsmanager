using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Diagrams.StandardMarker
{
    [Extension("472DE02F-00F9-48CE-8F9B-6E699E3030D3", "Standard Marker Factory", 10, ExecutionMode.Simplified)]
    public class StandardMarkerProviderFactory : IMarkerProviderFactory
    {
        public string Name => "Standard";

        public Scope ContextScope => Scope.ThreatEvent;

        public IMarkerProvider Create(object item)
        {
            IMarkerProvider result = null;

            if (item is IThreatEventsContainer container)
                result = new StandardMarkerProvider(container);

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
