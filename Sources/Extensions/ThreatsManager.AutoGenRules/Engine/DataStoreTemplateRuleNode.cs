using System;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DataStoreTemplateRuleNode : EntityTemplateRuleNode
    {
        public DataStoreTemplateRuleNode()
        {
        }

        public DataStoreTemplateRuleNode(IEntityTemplate entityTemplate) : base(entityTemplate)
        {
            if (entityTemplate.EntityType != EntityType.DataStore)
                throw new ArgumentException();

            this.Name = "Data Store Template";
        }
    }
}
