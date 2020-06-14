using ThreatsManager.Interfaces;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    public enum ComparisonOperator
    { 
        Exact,
        [EnumLabel("Starts With")]
        StartsWith,
        Contains
    }
}
