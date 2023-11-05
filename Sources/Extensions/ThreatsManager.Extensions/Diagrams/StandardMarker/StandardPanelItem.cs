using PostSharp.Patterns.Contracts;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Diagrams.StandardMarker
{
    public class StandardPanelItem : PanelItem
    {
        public StandardPanelItem([NotNull] IThreatEvent threatEvent) : base(threatEvent.Name)
        {
            BackColor = ThreatModelManager.ThreatsColor;
            TextColor = Color.White;
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

        public override ClickAction ActionOnClick => ClickAction.ShowObject;
    }
}
