using ThreatsManager.Utilities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using System.Drawing;

namespace ThreatsManager.Extensions.Diagrams
{
    public class ThreatTypePaletteItem : PaletteItem
    {
        public ThreatTypePaletteItem(string name) : base(name)
        {
            BackColor = ThreatModelManager.ThreatsColor;
            TextColor = Color.White;
        }

        public ThreatTypePaletteItem(string name, string description) : this(name)
        {
            Tooltip = description;
        }

        public override void Apply(object target)
        {
            if (target is IThreatEventsContainer container && Tag is IThreatType threatType)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Threat Event on container"))
                {
                    container.AddThreatEvent(threatType);
                    scope?.Complete();
                }
            }
        }
    }
}
