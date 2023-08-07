using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Diagrams
{
    [Extension("472DE02F-00F9-48CE-8F9B-6E699E3030D3", "Threat Events Marker Factory", 10, ExecutionMode.Simplified)]
    public class ThreatEventsMarkerFactory : IMarkerProviderFactory
    {
        public string Name => "Threat Events";
            
        public IMarkerProvider Create(object item)
        {
            IMarkerProvider result = null;

            if (item is IThreatEventsContainer container)
                result = new ThreatEventsMarker(container);

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
