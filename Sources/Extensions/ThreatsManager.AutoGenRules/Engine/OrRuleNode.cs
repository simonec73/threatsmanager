using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OrRuleNode : NaryRuleNode
    {
        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if ((Children != null) && (Children.Count > 0))
            {
                foreach (SelectionRuleNode child in Children)
                {
                    result |= child.Evaluate(context);
                }
            }

            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Children?.Any() ?? false)
            {
                if (Children.Count > 1)
                    builder.Append("(");

                bool first = true;

                foreach (var child in Children)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(" OR ");
                    }

                    builder.Append(child.ToString());
                }

                if (Children.Count > 1)
                    builder.Append(")");
            }

            return builder.ToString();
        }
    }
}
