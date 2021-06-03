using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Cwe;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MitreGraph
    {
        [JsonProperty("sources")]
        public List<Source> Sources { get; private set; }

        [JsonProperty("nodes")]
        public List<Node> Nodes { get; private set; }

        #region Public member functions.
        public void RegisterSource([Required] string source, [Required] string version, DateTime timestamp)
        {
            if (Sources == null)
                Sources = new List<Source>();

            if (Sources.All(x => string.CompareOrdinal(source, x.Name) != 0 ||
                                 string.CompareOrdinal(version, x.Version) != 0 ||
                                 timestamp != x.Timestamp))
            {
                Sources.Add(new Source(source, version, timestamp));
            }
        }

        public Node GetNode([Required] string source, [Required] string id)
        {
            if (Sources?.All(x => string.CompareOrdinal(source, x.Name) != 0) ?? true)
                return null;

            return Nodes?.FirstOrDefault(x => string.CompareOrdinal(source, x.Source) == 0 &&
                                              string.CompareOrdinal(id, x.Id) == 0);
        }

        public T GetNode<T>([Required] string id) where T : Node
        {
            return Nodes?.OfType<T>().FirstOrDefault(x => string.CompareOrdinal(id, x.Id) == 0);
        }

        public IEnumerable<Node> GetRelated([NotNull] Node node)
        {
            return Nodes?.Where(x => x.Relationships?
                .Any(y => string.CompareOrdinal(y.CounterpartySource, node.Source) == 0 &&
                          string.CompareOrdinal(y.CounterpartyId, node.Id) == 0) ?? false);
        }

        public IDictionary<string, List<string>> GetContexts(ContextType contextType)
        {
            IDictionary<string, List<string>> result = null;

            var nodes = Nodes?.OfType<WeaknessNode>()
                .Where(x => x.Contexts?.Any(y => y.ContextType == contextType) ?? false).ToArray();
            if (nodes?.Any() ?? false)
            {
                Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

                foreach (var node in nodes)
                {
                    var contexts = node.Contexts?.Where(x => x.ContextType == contextType).ToArray();
                    if (contexts?.Any() ?? false)
                    {
                        foreach (var context in contexts)
                        {
                            List<string> list;
                            if (dict.ContainsKey(context.Class))
                            {
                                list = dict[context.Class];
                            }
                            else
                            {
                                list = new List<string>();
                                dict.Add(context.Class, list);
                            }

                            if (!list.Contains(context.Name))
                                list.Add(context.Name);
                        }
                    }
                }

                result = dict;
            }

            return result;
        }

        public IEnumerable<WeaknessNode> GetWeaknessNodes(IEnumerable<string> classNames, IEnumerable<string> names)
        {
            var cs = classNames?.ToArray();
            var ns = names?.ToArray();

            return Nodes?.OfType<WeaknessNode>().Where(x =>
                (!(cs?.Any() ?? false) || (x.Contexts?.Any(y => cs.Contains(y.Class)) ?? false)) &&
                (!(ns?.Any() ?? false) || (x.Contexts?.Any(y => ns.Contains(y.Name)) ?? false))
            );
        }
        #endregion

        #region Internal methods used to build the Graph.
        internal Node CreateNode<T>(T source) where T : class
        {
            Node result = null;

            if (source is WeaknessType weakness)
            {
                result = new WeaknessNode(this, weakness);
            }
            else if (source is ObservedExampleTypeObserved_Example weaknessExample)
            {
                result = new ExternalNode(this, "CVE", weaknessExample.Reference,
                    weaknessExample.Link, weaknessExample.Description.ConvertToString());
            }
            else if (source is CategoryType category)
            {
                result = new CategoryNode(this, category);
            }
            else if (source is ViewType view)
            {
                result = new ViewNode(this, view);
            }

            if (result != null)
            {
                if (Nodes == null)
                    Nodes = new List<Node>();
                Nodes.Add(result);
            }

            return result;
        }

        internal void ReconcileRelationships()
        {
            if (Nodes?.Any() ?? false)
            {
                foreach (var node in Nodes)
                {
                    Reconcile(node);
                }
            }
        }
        #endregion

        #region Private member functions.
        private void Reconcile([NotNull] Node node)
        {
            var relationships = node.Relationships?.ToArray();
            if (relationships?.Any() ?? false)
            {
                foreach (var relationship in relationships)
                {
                    if (relationship.ViewId == null || GetNode<ViewNode>(relationship.ViewId) != null)
                    {
                        var counterparty = GetNode(relationship.CounterpartySource, relationship.CounterpartyId);
                        if (counterparty == null)
                        {
                            // The counterparty node does not exist, therefore the view must be removed.
                            node.Relationships?.Remove(relationship);
                        }
                        else
                        {
                            RelationshipType corresponding;
                            switch (relationship.Type)
                            {
                                case RelationshipType.ChildOf:
                                    corresponding = RelationshipType.ParentOf;
                                    break;
                                case RelationshipType.ParentOf:
                                    corresponding = RelationshipType.ChildOf;
                                    break;
                                case RelationshipType.StartsWith:
                                    corresponding = RelationshipType.Starts;
                                    break;
                                case RelationshipType.Starts:
                                    corresponding = RelationshipType.StartsWith;
                                    break;
                                case RelationshipType.CanFollow:
                                    corresponding = RelationshipType.CanPrecede;
                                    break;
                                case RelationshipType.CanPrecede:
                                    corresponding = RelationshipType.CanFollow;
                                    break;
                                case RelationshipType.RequiredBy:
                                    corresponding = RelationshipType.Requires;
                                    break;
                                case RelationshipType.Requires:
                                    corresponding = RelationshipType.RequiredBy;
                                    break;
                                case RelationshipType.CanAlsoBe:
                                    corresponding = RelationshipType.CanAlsoBe;
                                    break;
                                case RelationshipType.PeerOf:
                                    corresponding = RelationshipType.PeerOf;
                                    break;
                                case RelationshipType.Abstracts:
                                    corresponding = RelationshipType.IsAnExampleOf;
                                    break;
                                case RelationshipType.IsAnExampleOf:
                                    corresponding = RelationshipType.Abstracts;
                                    break;
                                case RelationshipType.Leverages:
                                    corresponding = RelationshipType.IsLeveragedBy;
                                    break;
                                case RelationshipType.IsLeveragedBy:
                                    corresponding = RelationshipType.Leverages;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            if (!counterparty.Relationships?.Any(x => x.Type == corresponding &&
                                                                      string.CompareOrdinal(x.CounterpartySource,
                                                                          node.Source) == 0 &&
                                                                      string.CompareOrdinal(x.CounterpartyId,
                                                                          node.Id) ==
                                                                      0) ?? false)
                            {
                                // The relationship is missing, therefore it must be added.
                                counterparty.AddRelationship(corresponding, node.Source, node.Id, relationship.ViewId);
                            }
                        }
                    }
                    else
                    {
                        // The relationship belongs to a view that does not exist, therefore it must be removed.
                        node.Relationships?.Remove(relationship);
                    }
                }
            }
        }
        #endregion
    }
}