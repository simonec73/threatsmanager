using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class ProposedMitigationsField : MitigationsField
    {
        protected override MitigationStatus Status => MitigationStatus.Proposed;

        public override string Tooltip => "Proposed mitigations for the Threat.";
 
        public override string ToString()
        {
            return "Proposed Mitigations";
        }
    }
}