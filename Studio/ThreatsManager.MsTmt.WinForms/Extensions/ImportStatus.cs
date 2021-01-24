using System.ComponentModel;

namespace ThreatsManager.MsTmt.Extensions
{
    public class ImportStatus
    {
        public ImportStatus()
        {

        }

        [Category("Diagrams")]
        public int Diagrams { get; set; }
        [Category("Entities")]
        public int ExternalInteractors { get; set; }
        [Category("Entities")]
        public int Processes { get; set; }
        [Category("Entities")]
        public int DataStores { get; set; }
        [Category("Flows")]
        public int DataFlows { get; set; }
        [Category("Trust Boundaries")]
        public int TrustBoundaries { get; set; }
        [Category("Entity Types")]
        public int EntityTypes { get; set; }
        [Category("Threats")]
        public int ThreatTypes { get; set; }
        [Category("Threats")]
        public int CustomThreatTypes { get; set; }
        [Category("Threats")]
        public int Threats { get; set; }
        [Category("Threats")]
        public int MissingThreats { get; set; }
    }
}