using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.TrustBoundaryList
{
    public enum TrustBoundaryListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("Trust Boundary not included in any Diagram")]
        NoDiagram
    }
}
