using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HasIncomingRuleNode : SelectionRuleNode
    {
        public HasIncomingRuleNode()
        {

        }

        public HasIncomingRuleNode(EntityType entityType)
        {
            this.Name = "Incoming Flows";
            this.EntityType = entityType;
        }

        [JsonProperty("entityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType EntityType { get; set; }

        public override bool Evaluate([NotNull] IIdentity identity)
        {
            bool result = false;

            var scopedIdentity = GetScopedIdentity(identity);

            if (scopedIdentity != null)
            {
                if (Scope != Scope.AnyTrustBoundary)
                {
                    result = InternalEvaluate(scopedIdentity);
                }
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] IIdentity identity)
        {
            bool result = false;

            if (identity is IEntity entity)
            {
                switch (EntityType)
                {
                    case EntityType.ExternalInteractor:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Source is IExternalInteractor && x.TargetId == identity.Id) ?? false;
                        break;
                    case EntityType.Process:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Source is IProcess && x.TargetId == identity.Id) ?? false;
                        break;
                    case EntityType.DataStore:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Source is IDataStore && x.TargetId == identity.Id) ?? false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope.ToString()}:Has incoming flows from {EntityType.GetEnumLabel()}";
        }
    }
}
