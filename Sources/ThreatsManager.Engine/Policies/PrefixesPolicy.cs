using System.Collections.Generic;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    /// <summary>
    /// Additional prefixes allowed for the Extension libraries.
    /// </summary>
    /// <remarks>
    /// It is not necessary to configure Prefixes for the default prefix used for the standard Extension libraries(“ThreatsManager”).
    /// It includes an allowed prefix in each line. Do not include quotes or double quotes.
    /// This policy integrates and extends the corresponding setting in TMS, it does not replace it.
    /// </remarks>
    public class PrefixesPolicy : Policy
    {
        protected override string PolicyName => "Prefixes";

        public IEnumerable<string> Prefixes => StringArrayValue;
    }
}
