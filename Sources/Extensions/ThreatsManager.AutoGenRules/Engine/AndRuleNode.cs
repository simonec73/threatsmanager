﻿using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class AndRuleNode : NaryRuleNode
    {
        public AndRuleNode() : base() 
        {
            Name = "AND";
        }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = true;

            if (Children?.Any() ?? false)
            {
                foreach (SelectionRuleNode child in Children)
                {
                    result &= child.Evaluate(context);
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
                        builder.Append(" AND ");
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
