using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Mitre.Attack;
using ThreatsManager.Mitre.Graph;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Mitre
{
    public class AttackEngine : IInitializableObject
    {
        private readonly Bundle _catalog;
        private readonly string _name;
        private readonly string _version;

        public AttackEngine([Required] string name, [Required] string version, [Required] string json)
        {
            _name = name;
            _version = version;

            if (json.Length > 0)
            {
                using (var textReader = new StringReader(json))
                using (var reader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer();
                    _catalog = serializer.Deserialize<Bundle>(reader);
                }
            }
        }

        public bool IsInitialized => _catalog != null;

        [InitializationRequired]
        public string Version => _version;

        [InitializationRequired]
        public void EnrichGraph([NotNull] MitreGraph graph)
        {
            graph.RegisterSource(_name, _version, _catalog.GetLastChangeDateTime());

            var attackPatterns = _catalog.Objects?
                .Where(x => !x.Deprecated && !x.Revoked && string.CompareOrdinal(x.Type, "attack-pattern") == 0)
                .ToArray();
            if (attackPatterns?.Any() ?? false)
            {
                foreach (var attackPattern in attackPatterns)
                    graph.CreateNode(attackPattern);
            }

            var mitigations = _catalog.Objects?
                .Where(x => !x.Deprecated && !x.Revoked && string.CompareOrdinal(x.Type, "course-of-action") == 0)
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                    graph.CreateNode(mitigation);
            }

            var relationships = _catalog.Objects?
                .Where(x => !x.Deprecated && !x.Revoked && string.CompareOrdinal(x.Type, "relationship") == 0)
                .ToArray();
            if (relationships?.Any() ?? false)
            {
                foreach (var relationship in relationships)
                {
                    var source = graph.GetNode("ATT&CK", relationship.Source);
                    var target = graph.GetNode("ATT&CK", relationship.Target);
                    if (source != null && target != null)
                    {
                        switch (relationship.Relationship)
                        {
                            case "mitigates":
                                source.AddRelationship(RelationshipType.Mitigates, target);
                                target.AddRelationship(RelationshipType.IsMitigatedBy, source);
                                break;
                            case "subtechnique-of":
                                source.AddRelationship(RelationshipType.ChildOf, target);
                                target.AddRelationship(RelationshipType.ParentOf, source);
                                break;
                        }
                    }
                }
            }
        }
    }
}
