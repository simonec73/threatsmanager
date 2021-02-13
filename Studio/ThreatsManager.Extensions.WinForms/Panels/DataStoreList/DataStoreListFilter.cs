using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.DataStoreList
{
    public enum DataStoreListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Data Stores not included in any Diagram")]
        NoDiagram,
        [EnumLabel("Data Stores with no Threat Events")]
        NoThreatEvents,
        [EnumLabel("Data Stores with missing Mitigations")]
        MissingMitigations
    }
}
