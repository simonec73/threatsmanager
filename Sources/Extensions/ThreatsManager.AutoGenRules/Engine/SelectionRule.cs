using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Properties;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SelectionRule : IMergeable<SelectionRule>
    {
        [JsonProperty("node")]
        public SelectionRuleNode Root { get; set; }

        public void Merge([NotNull] SelectionRule toBeMerged)
        {
            if (toBeMerged?.Root != null)
            {
                var or = new OrRuleNode()
                {
                    Name = "OR"
                };
                or.Children.Add(Root);
                or.Children.Add(toBeMerged.Root);
                Root = or;
            }
        }

        public void Merge(IMergeable toBeMerged)
        {
            if (toBeMerged is SelectionRule selectionRule)
                Merge(selectionRule);
        }

        public override string ToString()
        {
            string result = Root?.ToString();

            if (result == null)
                result = Resources.LabelSelectionRule;

            return result;
        }

        public bool Evaluate([NotNull] object context)
        {
            return Root?.Evaluate(context) ?? false;
        }
    }
}
