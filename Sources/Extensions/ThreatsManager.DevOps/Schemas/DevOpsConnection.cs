using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsConnection
    {
        public DevOpsConnection()
        {
        }

        public DevOpsConnection([NotNull] IDevOpsConnector connector)
        {
            ExtensionId = connector.FactoryId;
            Url = connector.Url;
            Project = connector.Project;
            MasterParent = connector.MasterParent;
            Tag = connector.Tag;
            WorkItemType = connector.WorkItemType;

            var workItemStateMappings = connector.WorkItemStateMappings?.ToArray();
            if (workItemStateMappings?.Any() ?? false)
            {
                WorkItemStateMappings = new List<DevOpsWorkItemStateMapping>();
                foreach (var mapping in workItemStateMappings)
                {
                    WorkItemStateMappings.Add(new DevOpsWorkItemStateMapping(mapping.Key, mapping.Value));
                }
            }

            var workItemFieldMappings = connector.WorkItemFieldMappings?.ToArray();
            if (workItemFieldMappings?.Any() ?? false)
            {
                WorkItemFieldMappings = new List<DevOpsFieldMapping>();
                foreach (var mapping in workItemFieldMappings)
                {
                    WorkItemFieldMappings.Add(new DevOpsFieldMapping(mapping.Key, mapping.Value));
                }
            }
        }

        [JsonProperty("extensionId")]
        public string ExtensionId { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("project")]
        public string Project { get; private set; }
        [JsonProperty("masterParent")]
        public IDevOpsItemInfo MasterParent { get; private set; }
        [JsonProperty("tag")]
        public string Tag { get; private set; }
        [JsonProperty("workItemType")]
        public string WorkItemType { get; private set; }
        [JsonProperty("workItemStates")]
        public List<DevOpsWorkItemStateMapping> WorkItemStateMappings { get; private set; }
        [JsonProperty("workItemFields")]
        public List<DevOpsFieldMapping> WorkItemFieldMappings { get; private set; }
    }
}