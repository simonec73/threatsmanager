using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class Node
    {
        protected Node()
        {

        }

        public Node([NotNull] MitreGraph graph, [Required] string source, [Required] string id)
        {
            Graph = graph;
            Source = source;
            Id = id;
        }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("desc")]
        public string Description { get; protected set; }

        [JsonProperty("source")]
        public string Source { get; private set; }

        public MitreGraph Graph { get; }

        [JsonProperty("relationships")]
        public List<Relationship> Relationships { get; private set; }

        public Relationship AddRelationship(RelationshipType type, [Required] string source, [Required] string id, string viewId = null)
        {
            Relationship result = null;

            if (Relationships == null)
                Relationships = new List<Relationship>();

            if (!Relationships.Any(x => x.Type == type && string.CompareOrdinal(source, x.CounterpartySource) == 0 &&
                                        string.CompareOrdinal(x.CounterpartyId, id) == 0))
            {
                result = new Relationship(type, source, id, viewId);
                Relationships.Add(result);
            }

            return result;
        }

        public Relationship AddRelationship(RelationshipType type, [NotNull] Node node)
        {
            return AddRelationship(type, node.Source, node.Id);
        }

        internal bool RemoveRelationship(RelationshipType type, string source, string id)
        {
            var relationship = Relationships?.FirstOrDefault(x =>
                x.Type == type && string.CompareOrdinal(source, x.CounterpartySource) == 0 &&
                string.CompareOrdinal(x.CounterpartyId, id) == 0);

            return RemoveRelationship(relationship);
        }

        internal bool RemoveRelationship(Relationship relationship)
        {
            var result = false;

            if (relationship != null && Relationships.Contains(relationship))
            {
                Relationships.Remove(relationship);
                result = true;
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
