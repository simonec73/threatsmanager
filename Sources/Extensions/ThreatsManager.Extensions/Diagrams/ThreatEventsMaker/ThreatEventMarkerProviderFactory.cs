using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Diagrams.ThreatEventsMaker
{
    [Extension("22FDD800-43DE-472F-BB89-68020202C8F1", "Threat Event Marker Factory", 12, ExecutionMode.Simplified)]
    public class ThreatEventMarkerProviderFactory : IMarkerProviderFactory
    {
        public string Name => "Threat Event";

        public Scope ContextScope => Scope.ThreatEvent;

        public IMarkerProvider Create(object item)
        {
            IMarkerProvider result = null;

            if (item is IThreatEventsContainer container)
                result = new ThreatEventMarkerProvider(container);

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
