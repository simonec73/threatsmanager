using System;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
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
