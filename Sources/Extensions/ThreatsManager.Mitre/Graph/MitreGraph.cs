using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Attack;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MitreGraph
    {
        #region Public properties.
        [JsonProperty("sources")]
        public List<Source> Sources { get; private set; }

        [JsonProperty("nodes")]
        public List<Node> Nodes { get; private set; }
        #endregion

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

        public void ReconcileRelationships()
        {
            if (Nodes?.Any() ?? false)
            {
                foreach (var node in Nodes)
                {
                    Reconcile(node);
                }
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

        #region Serialize/deserialize.
        public byte[] Serialize()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(writer, this);
            }

            var buf = Encoding.Unicode.GetBytes(sb.ToString());

            var result = new byte[buf.Length + 2];
            result[0] = 0xFF;
            result[1] = 0xFE;
            buf.CopyTo(result, 2);

            return result;
        }

        public void Serialize([Required] string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var file = File.OpenWrite(fileName))
            {
                using (var writer = new BinaryWriter(file))
                {
                    var serialization = Serialize();
                    writer.Write(serialization);
                }
            }
        }

        public static MitreGraph Deserialize(byte[] json)
        {
            MitreGraph result = null;

            if (json.Length > 0)
            {
                string jsonText;

                if (json[0] == 0xFF)
                    jsonText = Encoding.Unicode.GetString(json, 2, json.Length - 2);
                else
                    jsonText = Encoding.Unicode.GetString(json);

                using (var textReader = new StringReader(jsonText))
                using (var reader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    };
                    result = serializer.Deserialize<MitreGraph>(reader);
                }
            }

            return result;
        }

        public static MitreGraph Deserialize([Required] string fileName)
        {
            MitreGraph result;

            using (var file = File.OpenRead(fileName))
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytes = ms.ToArray();
                    result = Deserialize(bytes);
                }
            }

            return result;
        }
        #endregion

        #region Internal methods used to build the Graph.
        internal Node CreateNode<T>(T source) where T : class
        {
            Node result = null;

            if (source is Cwe.WeaknessType weakness)
            {
                result = new WeaknessNode(this, weakness);
            }
            else if (source is Cwe.ObservedExampleTypeObserved_Example weaknessExample)
            {
                result = new ExternalNode(this, "CVE", weaknessExample.Reference,
                    weaknessExample.Link, weaknessExample.Description.ConvertToString());
            }
            else if (source is Cwe.CategoryType cweCategory)
            {
                result = new CategoryNode(this, cweCategory);
            }
            else if (source is Cwe.ViewType cweView)
            {
                result = new ViewNode(this, cweView);
            }
            else if (source is Capec.CategoryType capecCategory)
            {
                result = new CategoryNode(this, capecCategory);
            }
            else if (source is Capec.ViewType capecView)
            {
                result = new ViewNode(this, capecView);
            }
            else if (source is Capec.AttackPatternType capecAttackPattern)
            {
                result = new AttackPatternNode(this, capecAttackPattern);
            }
            else if (source is AttackObject attackObject)
            {
                switch (attackObject.Type)
                {
                    case "attack-pattern":
                        result = new AttackPatternNode(this, attackObject);
                        break;
                    case "course-of-action":
                        result = new MitigationNode(this, attackObject);
                        break;
                }
            }

            if (result != null)
            {
                if (Nodes == null)
                    Nodes = new List<Node>();
                Nodes.Add(result);
            }

            return result;
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