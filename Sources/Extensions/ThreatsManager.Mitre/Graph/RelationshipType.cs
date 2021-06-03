using ThreatsManager.Interfaces;

namespace ThreatsManager.Mitre.Graph
{
    public enum RelationshipType
    {
        [EnumLabel("Child of")]
        ChildOf,
        [EnumLabel("Parent of")]
        ParentOf,
        [EnumLabel("Starts with")]
        StartsWith,
        [EnumLabel("Starts")]
        Starts,
        [EnumLabel("Can follow")]
        CanFollow,
        [EnumLabel("Can precede")]
        CanPrecede,
        [EnumLabel("Required by")]
        RequiredBy,
        Requires,
        [EnumLabel("Can also be")]
        CanAlsoBe,
        [EnumLabel("Peer of")]
        PeerOf,
        Abstracts,
        [EnumLabel("Is an example of")]
        IsAnExampleOf,
        Leverages,
        [EnumLabel("Is leveraged by")]
        IsLeveragedBy
    }
}