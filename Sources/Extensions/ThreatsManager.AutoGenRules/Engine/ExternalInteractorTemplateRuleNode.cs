using System;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExternalInteractorTemplateRuleNode : EntityTemplateRuleNode
    {
        public ExternalInteractorTemplateRuleNode()
        {
        }

        public ExternalInteractorTemplateRuleNode(IEntityTemplate entityTemplate) : base(entityTemplate)
        {
            if (entityTemplate.EntityType != EntityType.ExternalInteractor)
                throw new ArgumentException();

            Name = "External Interactor Template";
        }
    }
}
