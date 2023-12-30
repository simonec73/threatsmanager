using System.Collections.Generic;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    /// <summary>
    /// Additional folders where the Extension libraries must be searched.
    /// </summary>
    /// <remarks>It includes a folder in each line.Do not include quotes or double quotes.
    /// This policy integrates and extends the corresponding setting in TMS, it does not replace it.
    /// </remarks>
    public class FoldersPolicy : Policy
    {
        protected override string PolicyName => "Folders";

        public IEnumerable<string> Folders => StringArrayValue;
    }
}
