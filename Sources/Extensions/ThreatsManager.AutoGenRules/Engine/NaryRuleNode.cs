using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public abstract class NaryRuleNode : SelectionRuleNode
    {
        [JsonProperty("children", ItemTypeNameHandling = TypeNameHandling.Objects)]
        [Child]
        private readonly AdvisableCollection<SelectionRuleNode> _children = new AdvisableCollection<SelectionRuleNode>();

        protected NaryRuleNode() 
        {
            _children.CollectionChanged += _children_CollectionChanged;
        }

        private void _children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<SelectionRuleNode> items;

            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    items = e.NewItems?.OfType<SelectionRuleNode>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            UndoRedoManager.Attach(item);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    items = e.OldItems?.OfType<SelectionRuleNode>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            UndoRedoManager.Detach(item);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    items = e.OldItems?.OfType<SelectionRuleNode>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            UndoRedoManager.Detach(item);
                        }
                    }

                    items = e.NewItems?.OfType<SelectionRuleNode>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            UndoRedoManager.Attach(item);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    items = e.OldItems?.OfType<SelectionRuleNode>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            UndoRedoManager.Detach(item);
                        }
                    }
                    break;
            }
        }

        public IList<SelectionRuleNode> Children => _children;

        public override string ToString()
        {
            return Name;
        }
    }
}
