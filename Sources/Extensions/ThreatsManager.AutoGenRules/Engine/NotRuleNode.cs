using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NotRuleNode : UnaryRuleNode
    {
        public override bool Evaluate([NotNull] object context)
        {
            return !(Child?.Evaluate(context) ?? false);
        }

        public override string ToString()
        {
            string result = null;

            if (Child != null)
            {
                result = $"NOT {Child.ToString()}";
            }

            return result;
        }
    }
}
