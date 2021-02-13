using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.ProcessList
{
    public enum ProcessListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Processes not included in any Diagram")]
        NoDiagram,
        [EnumLabel("Processes with no Threat Events")]
        NoThreatEvents,
        [EnumLabel("Processes with missing Mitigations")]
        MissingMitigations
    }
}
