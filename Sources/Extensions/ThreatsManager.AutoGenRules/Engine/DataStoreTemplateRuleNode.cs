using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
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
