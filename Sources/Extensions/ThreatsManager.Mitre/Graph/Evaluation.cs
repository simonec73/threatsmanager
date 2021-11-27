using ThreatsManager.Interfaces;

namespace ThreatsManager.Mitre.Graph
{
    public enum Evaluation
    {
        Unknown,
        [EnumLabel("Very High")]
        VeryHigh,
        High,
        Medium,
        Low,
        [EnumLabel("Very Low")]
        VeryLow,
    }
}