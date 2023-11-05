using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
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
