using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HasOutgoingRuleNode : SelectionRuleNode
    {
        public HasOutgoingRuleNode()
        {

        }

        public HasOutgoingRuleNode(EntityType entityType)
        {
            this.Name = "Outgoing Flows";
            this.EntityType = entityType;
        }

        [JsonProperty("entityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType EntityType { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IIdentity identity)
            {
                var scopedIdentity = GetScopedIdentity(identity);

                if (scopedIdentity != null)
                {
                    if (Scope != Scope.AnyTrustBoundary)
                    {
                        result = InternalEvaluate(scopedIdentity);
                    }
                }
            }
            else
            {
                result = InternalEvaluate(context);
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IEntity entity)
            {
                switch (EntityType)
                {
                    case EntityType.ExternalInteractor:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Target is IExternalInteractor && x.SourceId == entity.Id) ?? false;
                        break;
                    case EntityType.Process:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Target is IProcess && x.SourceId == entity.Id) ?? false;
                        break;
                    case EntityType.DataStore:
                        result = entity.Model.DataFlows?
                                     .Any(x => x.Target is IDataStore && x.SourceId == entity.Id) ?? false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope.ToString()}:Has outgoing flows to {EntityType.GetEnumLabel()}";
        }
    }
}
