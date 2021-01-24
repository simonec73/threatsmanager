using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.MitigationList
{
    public enum MitigationListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Mitigations which have no defined Status")]
        UndefinedMitigations
    }
}
