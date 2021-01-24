using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class ExistingMitigationsField : MitigationsField
    {
        protected override MitigationStatus Status => MitigationStatus.Existing;

        public override string Tooltip => "Existing mitigations for the Threat.";

        public override string ToString()
        {
            return "Existing Mitigations";
        }
    }
}