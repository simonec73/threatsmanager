using PostSharp.Patterns.Contracts;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Diagrams.ThreatEventsMaker
{
    public class ThreatEventPanelItem : PanelItem
    {
        public ThreatEventPanelItem([NotNull] IThreatEvent threatEvent) : base(threatEvent.Name)
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

                if (Tag is IThreatEvent threatEvent && threatEvent.Severity is ISeverity severity)
                {
                    try
                    {
                        var temp = new Bitmap(16, 16);
                        using (Graphics g = Graphics.FromImage(temp))
                        {
                            using (var brush = new SolidBrush(Color.FromKnownColor(severity.BackColor)))
                            {
                                g.FillEllipse(brush, 0, 0, 16, 16);
                            }
                        }

                        result = temp;
                    }
                    catch
                    {
                        // Ignore and return the not resized bitmap.
                    }
                }

                return result;
            }
        }

        public override ClickAction ActionOnClick => ClickAction.ShowObject;
    }
}
