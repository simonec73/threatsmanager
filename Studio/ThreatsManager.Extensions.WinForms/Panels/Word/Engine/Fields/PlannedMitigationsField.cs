using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class PlannedMitigationsField : MitigationsField
    {
        protected override MitigationStatus Status => MitigationStatus.Planned;

        public override string Tooltip => "Planned mitigations for the Threat.";
 
        public override string ToString()
        {
            return "Planned Mitigations";
        }
    }
}