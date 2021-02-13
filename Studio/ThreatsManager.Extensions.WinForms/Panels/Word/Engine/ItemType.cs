using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal enum ItemType
    {
        Undefined = 0,
        [EnumLabel("External Interactors")]
        [EnumOrder(0)]
        ExternalInteractor = 1,
        [EnumLabel("Processes")]
        [EnumOrder(1)]
        Process = 2,
        [EnumLabel("Data Stores")]
        [EnumOrder(2)]
        DataStore = 3,
        [EnumLabel("Flows")]
        [EnumOrder(3)]
        DataFlow = 7,
        [EnumLabel("Trust Boundaries")]
        [EnumOrder(4)]
        TrustBoundary = 8,
        [EnumLabel("Threat Types")]
        [EnumOrder(5)]
        ThreatType = 4,
        [EnumLabel("Threat Events")]
        [EnumOrder(6)]
        ThreatEvent = 5,
        [EnumLabel("Mitigations")]
        [EnumOrder(7)]
        Mitigation = 6
    }
}