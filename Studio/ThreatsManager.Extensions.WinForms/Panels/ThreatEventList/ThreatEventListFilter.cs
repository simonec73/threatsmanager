using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public enum ThreatEventListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Threat Events without any Mitigation")]
        NoMitigations,
        [EnumLabel("Threat Events with different names or descriptions than the respective Threat Type")]
        DifferentNameDescription,
        [EnumLabel("Threat Events with different severity than the respective Threat Type")]
        DifferentSeverity,
        [EnumLabel("Threat Events with same severity than the respective Threat Type")]
        SameSeverity
    }
}
