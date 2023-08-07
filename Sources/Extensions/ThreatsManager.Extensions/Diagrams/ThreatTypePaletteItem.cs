using ThreatsManager.Utilities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Diagrams
{
    public class ThreatTypePaletteItem : PaletteItem
    {
        public ThreatTypePaletteItem(string name) : base(name) { }

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
