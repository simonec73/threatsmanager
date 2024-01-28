using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.ImportedList
{
    public enum ImportedListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Items not applied anywhere")]
        NotApplied,
        [EnumLabel("Items applied somewhere")]
        Applied
    }
}
