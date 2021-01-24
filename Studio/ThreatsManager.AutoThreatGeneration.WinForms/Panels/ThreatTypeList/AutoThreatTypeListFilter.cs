using ThreatsManager.Interfaces;

namespace ThreatsManager.AutoThreatGeneration.Panels.ThreatTypeList
{
    public enum AutoThreatTypeListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Threat Types without any associated generation rule")]
        NoGenRule,
        [EnumLabel("Threat Types with mitigations having no associated generation rule")]
        NoMitigationGenRule
    }
}
