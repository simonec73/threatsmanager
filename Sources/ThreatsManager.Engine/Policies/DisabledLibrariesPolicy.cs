using System.Collections.Generic;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    public class DisabledLibrariesPolicy : Policy
    {
        protected override string PolicyName => "DisabledLibraries";

        public IEnumerable<string> DisabledLibraries => StringArrayValue;
    }
}
