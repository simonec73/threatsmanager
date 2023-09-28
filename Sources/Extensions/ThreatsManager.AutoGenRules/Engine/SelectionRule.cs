using System;
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
    public class SelectionRule : IMergeable<SelectionRule>, IThreatModelAware
    {
        [JsonProperty("node", TypeNameHandling = TypeNameHandling.Objects)]
        [Reference]
        private SelectionRuleNode _node { get; set; }

        [JsonProperty("modelId")]
        private Guid _modelId { get; set; }

        [property: NotRecorded]
        public SelectionRuleNode Root 
        {
            get => _node;

            set
            {
                _node = value;
                if (_node != null)
                    _node.ModelId = ModelId;
            }
        }

        public Guid ModelId
        {
            get => _modelId;

            set
            {
                if (_modelId != value)
                    _modelId = value;
                if (_node != null) _node.ModelId = _modelId;
            }
        }

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

                var model = ThreatModelManager.Get(ModelId);
                UndoRedoManager.Attach(toBeMerged.Root, model);
                var children = toBeMerged.Root.Traverse();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        UndoRedoManager.Attach(child, model);
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
