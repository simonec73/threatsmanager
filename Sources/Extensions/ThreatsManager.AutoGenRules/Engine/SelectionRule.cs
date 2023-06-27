using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.AutoGenRules.Properties;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class SelectionRule : IMergeable<SelectionRule>
    {
        [JsonProperty("node", TypeNameHandling = TypeNameHandling.Objects)]
        [Child]
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

                UndoRedoManager.Attach(toBeMerged.Root);
                var children = toBeMerged.Root.Traverse();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        UndoRedoManager.Attach(child);
                    }
                }
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
