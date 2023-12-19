using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    public class RequireCertsPolicy : Policy
    {
        protected override string PolicyName => "RequireCerts";

        public bool? RequireCerts => BoolValue;
    }
}
