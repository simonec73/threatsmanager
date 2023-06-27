using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
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
