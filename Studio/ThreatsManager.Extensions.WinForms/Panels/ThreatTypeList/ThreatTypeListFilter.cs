using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public enum ThreatTypeListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Threat Types without any associated standard Mitigation")]
        NoMitigations,
        [EnumLabel("Threat Types without any associated Threat Event")]
        NoThreatEvents
    }
}
