using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class ApprovedMitigationsField : MitigationsField
    {
        protected override MitigationStatus Status => MitigationStatus.Approved;

        public override string Tooltip => "Approved mitigations for the Threat.";

        public override string ToString()
        {
            return "Approved Mitigations";
        }
    }
}