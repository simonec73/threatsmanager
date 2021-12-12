using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.WeaknessList
{
    public enum WeaknessListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Weaknesses without any associated standard Mitigation")]
        NoMitigations,
        [EnumLabel("Weaknesses without any associated Vulnerability")]
        NoVulnerabilities
    }
}
