using PostSharp.Patterns.Contracts;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Diagrams
{
    public class ThreatEventPanelItem : PanelItem
    {
        public ThreatEventPanelItem([NotNull] IThreatEvent threatEvent) : base(threatEvent.Name)
        {
            Tag = threatEvent;
        }

        public override Image Icon
        {
            get
            {
                Image result = null;

                if (Tag is IThreatEvent threatEvent)
                {
                    switch (threatEvent.GetMitigationLevel())
                    {
                        case MitigationLevel.Partial:
                            result = Icons.Resources.threat_circle_orange_small;
                            break;
                        case MitigationLevel.Complete:
                            result = Icons.Resources.threat_circle_green_small;
                            break;
                        default:
                            result = Icons.Resources.threat_circle_small;
                            break;
                    }
                }

                return result;
            }
        }
    }
}
