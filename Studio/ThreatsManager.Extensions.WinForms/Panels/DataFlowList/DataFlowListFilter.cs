using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.DataFlowList
{
    public enum DataFlowListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Flows not included in any Diagram")]
        NoDiagram,
        [EnumLabel("Flows with no Threat Events")]
        NoThreatEvents,
        [EnumLabel("Flows with missing Mitigations")]
        MissingMitigations
    }
}
