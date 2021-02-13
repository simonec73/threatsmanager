using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.ExternalInteractorList
{
    public enum ExternalInteractorListFilter
    {
        [EnumLabel("No special filter applied")]
        NoFilter,
        [EnumLabel("External Interactors not included in any Diagram")]
        NoDiagram,
        [EnumLabel("External Interactors with no Threat Events")]
        NoThreatEvents,
        [EnumLabel("External Interactors with missing Mitigations")]
        MissingMitigations
    }
}
