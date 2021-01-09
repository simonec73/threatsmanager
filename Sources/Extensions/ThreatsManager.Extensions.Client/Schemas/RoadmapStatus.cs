using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Client.Schemas
{
    public enum RoadmapStatus
    {
        [EnumLabel("Not Assessed")]
        NotAssessed,
        [EnumLabel("Short Term")]
        ShortTerm,
        [EnumLabel("Mid Term")]
        MidTerm,
        [EnumLabel("Long Term")]
        LongTerm,
        [EnumLabel("No Action Required")]
        NoActionRequired
    }
}