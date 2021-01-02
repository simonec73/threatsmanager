using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EntityTemplateRuleNode : SelectionRuleNode
    {
        public EntityTemplateRuleNode()
        {
        }

        public EntityTemplateRuleNode(IEntityTemplate entityTemplate)
        {
            this.Name = "Entity Template";
            this.EntityTemplate = entityTemplate?.Id ?? Guid.Empty;
        }

        [JsonProperty("entityTemplate")]
        public Guid EntityTemplate { get; set; }

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
                result = entity.Template?.Id == EntityTemplate;
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope}:Is an instance of {Name} with ID '{EntityTemplate:D}";
        }
    }
}
