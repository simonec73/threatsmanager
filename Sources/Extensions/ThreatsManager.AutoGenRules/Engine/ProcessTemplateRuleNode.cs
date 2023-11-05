using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class ProcessTemplateRuleNode : EntityTemplateRuleNode
    {
        public ProcessTemplateRuleNode()
        {
        }

        public ProcessTemplateRuleNode(IEntityTemplate entityTemplate) : base(entityTemplate)
        {
            if (entityTemplate.EntityType != EntityType.Process)
                throw new ArgumentException();

            this.Name = "Process Template";
        }
    }
}
