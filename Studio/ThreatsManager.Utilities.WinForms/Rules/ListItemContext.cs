using System.Collections.Generic;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal abstract class ListItemContext : ButtonItemContext
    {
        public ListItemContext(IEnumerable<string> values, Scope scope) : base(scope)
        {
            Values = values;
        }

        public IEnumerable<string> Values { get; private set; }
    }
}