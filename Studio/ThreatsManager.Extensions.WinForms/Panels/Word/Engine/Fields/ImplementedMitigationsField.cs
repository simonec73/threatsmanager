using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class ImplementedMitigationsField : MitigationsField
    {
        protected override MitigationStatus Status => MitigationStatus.Implemented;

        public override string Tooltip => "Recently implemented mitigations for the Threat.";
 
        public override string ToString()
        {
            return "Implemented Mitigations";
        }
    }
}