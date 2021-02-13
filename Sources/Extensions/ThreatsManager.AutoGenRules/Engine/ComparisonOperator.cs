using ThreatsManager.Interfaces;

namespace ThreatsManager.AutoGenRules.Engine
{
    public enum ComparisonOperator
    { 
        Exact,
        [EnumLabel("Starts With")]
        StartsWith,
        Contains
    }
}
