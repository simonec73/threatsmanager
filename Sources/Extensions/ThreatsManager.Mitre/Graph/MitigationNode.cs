using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Attack;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MitigationNode : Node
    {
        public MitigationNode()
        {

        }
         
        internal MitigationNode([NotNull] MitreGraph graph, [NotNull] AttackObject attackPattern) : base(graph, "ATT&CK", attackPattern.AttackId)
        {
            if (attackPattern.Deprecated || attackPattern.Revoked)
                throw new ArgumentException(Properties.Resources.InvalidStatus, "attackPattern");

            Name = attackPattern.Name;
            Description = attackPattern.Description;
        }
    }
}
