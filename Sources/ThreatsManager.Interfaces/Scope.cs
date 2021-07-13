using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Enumeration of the potential scopes for the Context Aware action.
    /// </summary>
    [Flags]
    public enum Scope
    {
        /// <summary>
        /// Undefined scope.
        /// </summary>
        [UiHidden]
        Undefined = 0,
        /// <summary>
        /// External Interactor.
        /// </summary>
        [EnumLabel("External Interactor")]
        ExternalInteractor = 1,
        /// <summary>
        /// Process.
        /// </summary>
        Process = 2,
        /// <summary>
        /// Data Store.
        /// </summary>
        [EnumLabel("Data Store")]
        DataStore = 4,
        /// <summary>
        /// Scope is any Entity (External Interactor, Process or Data Store).
        /// </summary>
        [UiHidden]
        Entity = ExternalInteractor | Process | DataStore,
        /// <summary>
        /// Entity Template.
        /// </summary>
        [UiHidden]
        [EnumLabel("Entity Template")]
        EntityTemplate = 8,
        /// <summary>
        /// Data Flow.
        /// </summary>
        [EnumLabel("Flow")]
        DataFlow = 16,
        /// <summary>
        /// Flow Template,
        /// </summary>
        [UiHidden]
        [EnumLabel("Flow Template")]
        FlowTemplate = 131072,
        /// <summary>
        /// Trust Boundary.
        /// </summary>
        [EnumLabel("Trust Boundary")]
        TrustBoundary = 32,
        /// <summary>
        /// Trust Boundary Template.
        /// </summary>
        [UiHidden]
        [EnumLabel("Trust Boundary Template")]
        TrustBoundaryTemplate = 4194304,
        /// <summary>
        /// Item Templates.
        /// </summary>
        [UiHidden]
        ItemTemplate = EntityTemplate | FlowTemplate | TrustBoundaryTemplate,
        /// <summary>
        /// Logical Group.
        /// </summary>
        /// <remarks>Not implemented so far.</remarks>
        [UiHidden]
        [EnumLabel("Logical Group")]
        LogicalGroup = 64,
        /// <summary>
        /// Any group, including Trust Boundaries.
        /// </summary>
        [UiHidden]
        Group = LogicalGroup | TrustBoundary,
        /// <summary>
        /// Threat Type.
        /// </summary>
        [EnumLabel("Threat Type")]
        ThreatType = 128,
        /// <summary>
        /// Threat Event.
        /// </summary>
        [EnumLabel("Threat Event")]
        ThreatEvent = 256,
        /// <summary>
        /// Threat Event Scenario.
        /// </summary>
        [EnumLabel("Threat Event Scenario")]
        ThreatEventScenario = 512,
        /// <summary>
        /// Threat Event Mitigation.
        /// </summary>
        [EnumLabel("Threat Event Mitigation")]
        ThreatEventMitigation = 1024,
        /// <summary>
        /// Threat Event Vulnerability.
        /// </summary>
        [EnumLabel("Threat Event Vulnerability")]
        ThreatEventVulnerability = 536870912,
        /// <summary>
        /// Everything related to Threats.
        /// </summary>
        [UiHidden]
        Threats = ThreatType | ThreatEvent | ThreatEventScenario | Weakness | Vulnerability | ThreatEventVulnerability,
        /// <summary>
        /// Known Mitigation.
        /// </summary>
        [EnumLabel("Mitigation")]
        Mitigation = 2048,
        /// <summary>
        /// Threat Type Mitigation.
        /// </summary>
        [EnumLabel("Threat Type Mitigation")]
        ThreatTypeMitigation = 4096,
        /// <summary>
        /// Weakness.
        /// </summary>
        Weakness = 8388608,
        /// <summary>
        /// Vulnerability.
        /// </summary>
        Vulnerability = 33554432,
        /// <summary>
        /// Weakness Mitigation.
        /// </summary>
        [EnumLabel("Weakness Mitigation")]
        WeaknessMitigation = 67108864,
        /// <summary>
        /// Vulnerability Mitigation.
        /// </summary>
        [EnumLabel("Vulnerability Mitigation")]
        VulnerabilityMitigation = 134217728,
        /// <summary>
        /// Threat Actor.
        /// </summary>
        [EnumLabel("Threat Actor")]
        ThreatActor = 8192,
        /// <summary>
        /// Severity.
        /// </summary>
        [UiHidden]
        Severity = 16384,
        /// <summary>
        /// Strength.
        /// </summary>
        [UiHidden]
        Strength = 268435456,
        /// <summary>
        /// Property Type.
        /// </summary>
        [UiHidden]
        PropertyType = 32768,
        /// <summary>
        /// Property Schema.
        /// </summary>
        [UiHidden]
        PropertySchema = 65536,
        /// <summary>
        /// Standard Diagram.
        /// </summary>
        Diagram = 262144,
        /// <summary>
        /// Entity Shape.
        /// </summary>
        [EnumLabel("Entity Shape")]
        EntityShape = 524288,
        /// <summary>
        /// Group Shape.
        /// </summary>
        [EnumLabel("Group Shape")]
        GroupShape = 1048576,
        /// <summary>
        /// Link.
        /// </summary>
        Link = 2097152,
        /// <summary>
        /// Threat Model.
        /// </summary>
        [EnumLabel("Threat Model")]
        ThreatModel = 16777216,
        /// <summary>
        /// All Property Containers.
        /// </summary>
        [UiHidden]
        PropertyContainers = Entity | ItemTemplate | DataFlow | Group | Threats | Mitigation | ThreatActor |
                               Severity | Strength | Diagram | ThreatTypeMitigation | ThreatEventMitigation |
                               EntityShape | GroupShape | Link | ThreatModel | WeaknessMitigation | VulnerabilityMitigation,
       /// <summary>
       /// Everything.
       /// </summary>
       [UiHidden]
       All = PropertyContainers | PropertyType | PropertySchema
    }
}