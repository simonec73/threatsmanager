using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EntityTemplateRuleNode : SelectionRuleNode
    {
        public EntityTemplateRuleNode()
        {
        }

        public EntityTemplateRuleNode(IEntityTemplate entityTemplate)
        {
            this.Name = "Entity Type";
            this.EntityTemplate = entityTemplate?.Id ?? Guid.Empty;
        }

        [JsonProperty("entityTemplate")]
        public Guid EntityTemplate { get; set; }

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
                result = entity.Template?.Id == EntityTemplate;
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope}:Is an instance of Entity Template with ID '{EntityTemplate:D}";
        }
    }
}
