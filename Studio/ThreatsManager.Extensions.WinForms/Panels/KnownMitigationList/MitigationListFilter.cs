using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public enum MitigationListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Mitigations without any associated Threat Type")]
        NoThreatTypes,
        [EnumLabel("Mitigations without any associated Threat Event")]
        NoThreatEvents
    }
}
