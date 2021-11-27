using ThreatsManager.Interfaces;

namespace ThreatsManager.Mitre.Graph
{
    public enum RelationshipType
    {
        [EnumLabel("is child of")]
        ChildOf,
        [EnumLabel("is parent of")]
        ParentOf,
        [EnumLabel("starts with")]
        StartsWith,
        [EnumLabel("starts")]
        Starts,
        [EnumLabel("can follow")]
        CanFollow,
        [EnumLabel("can precede")]
        CanPrecede,
        [EnumLabel("is required by")]
        RequiredBy,
        [EnumLabel("requires")]
        Requires,
        [EnumLabel("can also be")]
        CanAlsoBe,
        [EnumLabel("is peer of")]
        PeerOf,
        [EnumLabel("abstracts")]
        Abstracts,
        [EnumLabel("is an example of")]
        IsAnExampleOf,
        [EnumLabel("leverages")]
        Leverages,
        [EnumLabel("is leveraged by")]
        IsLeveragedBy,
        [EnumLabel("mitigates")]
        Mitigates,
        [EnumLabel("is mitigated by")]
        IsMitigatedBy
    }
}