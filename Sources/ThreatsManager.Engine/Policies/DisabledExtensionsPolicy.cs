using System.Collections.Generic;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    public class DisabledExtensionsPolicy : Policy
    {
        protected override string PolicyName => "DisabledExtensions";

        public IEnumerable<string> DisabledExtensions => StringArrayValue;
    }
}
