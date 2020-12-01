using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using PostSharp.Patterns.Contracts;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.DevOps.Engines
{
    public class AzureDevOps : IDevOpsConnector, IInitializableObject
    {
        #region Private member variables.
        private string _url;
        private VssConnection _connection;
        private WorkItemTrackingHttpClient _client;
        private const string DefaultTag = "ThreatModeling";
        private string _workItemType;
        private IEnumerable<string> _availableProjects;
        private IEnumerable<string> _itemTypes;
        private IEnumerable<string> _itemStates;
        private IEnumerable<IDevOpsField> _fields;
        private readonly Mapper<WorkItemStatus> _workItemMapping = new Mapper<WorkItemStatus>();
        private readonly Mapper<IdentityField> _workItemFieldMapping = new Mapper<IdentityField>();
        #endregion

        static AzureDevOps()
        {
            TypeDescriptor.AddAttributes(typeof(IdentityDescriptor),
                new TypeConverterAttribute(typeof(IdentityDescriptorConverter).FullName));
            TypeDescriptor.AddAttributes(typeof(SubjectDescriptor),
                new TypeConverterAttribute(typeof(SubjectDescriptorConverter).FullName));
        }

        public AzureDevOps()
        {
            Tag = DefaultTag;

            _workItemMapping.SetStandardMapping("To Do", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("New", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("Open", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("Active", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("Requested", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("Design", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("In Planning", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Approved", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Ready", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Accepted", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("In Progress", WorkItemStatus.InProgress);
            _workItemMapping.SetStandardMapping("Committed", WorkItemStatus.InProgress);
            _workItemMapping.SetStandardMapping("Done", WorkItemStatus.Done);
            _workItemMapping.SetStandardMapping("Closed", WorkItemStatus.Done);
            _workItemMapping.SetStandardMapping("Inactive", WorkItemStatus.Done);
            _workItemMapping.SetStandardMapping("Completed", WorkItemStatus.Done);
            _workItemMapping.SetStandardMapping("Removed", WorkItemStatus.Removed);

            _workItemFieldMapping.SetStandardMapping("System.Title", new IdentityField(IdentityFieldType.Name));
            _workItemFieldMapping.SetStandardMapping("System.Description", new IdentityField(IdentityFieldType.Description));
            _workItemFieldMapping.SetStandardMapping("Microsoft.VSTS.Common.Priority", new IdentityField(IdentityFieldType.Priority));
        }

        public bool IsInitialized => IsConnected();

        public override string ToString()
        {
            return "Microsoft Azure DevOps";
        }

        #region Relationship with Factory.
        public string FactoryId => "0EB212AB-EBA8-483D-A76C-E2D31CEFFCE1";
        #endregion

        #region Connection management.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<string> Connect([Required] string url)
        {
            _url = url;

            _connection = new VssConnection(new Uri(_url), new VssClientCredentials());
            using (var projectClient = _connection.GetClient<ProjectHttpClient>())
            {
                _availableProjects = projectClient.GetProjects().Result.Select(x => x.Name).ToArray();
            }


            return _availableProjects;
        }

        public bool OpenProject([Required] string project)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(_url) &&
                (_availableProjects?.Any(x => string.CompareOrdinal(project, x) == 0) ?? false))
            {
                Project = project;
                result = true;
            }

            return result;
        }

        [InitializationRequired]
        public void Disconnect()
        {
            _url = null;
            _availableProjects = null;

            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            if (_connection != null)
            {
                _connection.Disconnect();
                _connection.Dispose();
                _connection = null;
            }
        }

        public bool IsConnected()
        {
            return !string.IsNullOrWhiteSpace(Project) && !string.IsNullOrEmpty(_url) && _connection != null;
        }

        public string Url => _url;

        public string Project {get; private set;}
        #endregion

        #region Parent management.
        public IDevOpsItemInfo MasterParent { get; set; }

        [InitializationRequired]
        public async Task<IEnumerable<IDevOpsItemInfo>> GetItemsAsync(string filter)
        {
            IEnumerable<IDevOpsItemInfo> result = null;

            if (!string.IsNullOrWhiteSpace(filter) && filter.Length > 2)
            {
                var query = new Wiql()
                {
                    Query = "Select [Id] " +
                            "From WorkItems " +
                            "Where [System.TeamProject] = '" + Project + "' " +
                            "And [System.Title] Contains '" + filter + "' "
                };

                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var queryResult = _client.QueryByWiqlAsync(query);
                var ids = (await queryResult)?.WorkItems.Select(item => item.Id).ToArray();

                if (ids?.Any() ?? false)
                {
                    result = await GetDevOpsItemsInfoAsync(ids);
                }
            }

            return result;
        }
        #endregion

        #region Tag management.
        public string Tag { get; set; }
        #endregion

        #region Work Item Types management.
        [InitializationRequired]
        public async Task<IEnumerable<string>> GetWorkItemTypesAsync()
        {
            if (_itemTypes == null)
            {
                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var queryResultAsync = _client.GetWorkItemTypesAsync(Project);
                var queryResult = await queryResultAsync;

                _itemTypes = queryResult.Select(x => x.Name).ToArray();
            }


            return _itemTypes;
        }

        public string WorkItemType
        {
            get => _workItemType;
            set
            {
                if (string.CompareOrdinal(_workItemType, value) != 0)
                {
                    _workItemType = value;
                    _itemStates = null;
                }
            }
        }
        #endregion

        #region Work Item States management.
        public IEnumerable<KeyValuePair<string, WorkItemStatus>> WorkItemStateMappings => _workItemMapping.ToArray();

        public void SetWorkItemStateMapping(string devOpsState, WorkItemStatus status)
        {
            if (status == WorkItemStatus.Unknown)
            {
                if (_workItemMapping.ContainsKey(devOpsState))
                {
                    _workItemMapping.Remove(devOpsState);
                }
            }
            else
            {
                _workItemMapping[devOpsState] = status;
            }
        }

        [InitializationRequired]
        public async Task<IEnumerable<string>> GetWorkItemStatesAsync()
        {
            if (_itemStates == null && !string.IsNullOrEmpty(_workItemType))
            {
                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var queryResultAsync = _client.GetWorkItemTypeStatesAsync(Project, _workItemType);
                var queryResult = await queryResultAsync;

                _itemStates = queryResult.Select(x => x.Name).ToArray();
            }
            
            return _itemStates;
        }
        #endregion

        #region Work Item Fields management.
        public IEnumerable<KeyValuePair<string, IdentityField>> WorkItemFieldMappings => _workItemFieldMapping.ToArray();

        public void SetWorkItemFieldMapping(IDevOpsField devOpsField, IdentityField modelField)
        {
            SetWorkItemFieldMapping(devOpsField.Id, modelField);
        }

        public void SetWorkItemFieldMapping(string devOpsFieldId, IdentityField modelField)
        {
            if (modelField == null)
            {
                if (_workItemFieldMapping.ContainsKey(devOpsFieldId))
                {
                    _workItemFieldMapping.Remove(devOpsFieldId);
                }
            }
            else
            {
                _workItemFieldMapping[devOpsFieldId] = modelField;
            }
        }

        [InitializationRequired]
        public async Task<IEnumerable<IDevOpsField>> GetWorkItemDevOpsFieldsAsync()
        {
            if (_fields == null)
            {
                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var fieldsAsync = _client.GetFieldsAsync(Project);
                var fields = await fieldsAsync;
                if (fields.Any())
                {
                    var list = new List<DevOpsField>();
                    foreach (var field in fields)
                    {
                        if (field.Usage == FieldUsage.WorkItem)
                            list.Add(new DevOpsField(field.ReferenceName, field.Name));
                    }

                    _fields = list;
                }
            }

            return _fields;
        }
        #endregion

        #region Work Items management.
        [InitializationRequired(-1)]
        public async Task<int> CreateWorkItemAsync([PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation)
        {
            var result = -1;

            if (!string.IsNullOrWhiteSpace(WorkItemType))
            {

                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var workItemInfoAsync = GetWorkItemInfoAsync(mitigation);
                var workItemInfo = await workItemInfoAsync;
                if (workItemInfo != null)
                {
                    result = workItemInfo.Id;
                }
                else
                {
                    var request = CreateRequest(mitigation);

                    if (request != null)
                    {
                        var workItem = _client
                            .CreateWorkItemAsync(request, Project, WorkItemType);
                        result = (await workItem)?.Id ?? -1;
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation)
        {
            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            WorkItemInfo result = await GetWorkItemInfoAsync(mitigation,
                $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.WorkItemType] = '{WorkItemType}' And [System.Title] = '{mitigation.Name}'");

            if (result == null) 
                result = await GetWorkItemInfoAsync(mitigation,
                 $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.Title] = '{mitigation.Name.Replace("'", "''")}'");

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([Positive] int id)
        {
            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            return (await GetWorkItemsInfoAsync(new[] {id}))?.FirstOrDefault();
        }

        [InitializationRequired]
        public async Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(IEnumerable<int> ids)
        {
            IEnumerable<WorkItemInfo> result = null;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            var fields = new[] {"System.Id", "System.State"};

            var itemsAsync = _client.GetWorkItemsAsync(Project, ids, fields, expand: WorkItemExpand.Relations);

            var items = await itemsAsync;

            if (items?.Any() ?? false)
            {
                var list = new List<WorkItemInfo>();

                foreach (var item in items)
                {
                    if (item.Id.HasValue)
                    {
                        var status = WorkItemStatus.Unknown;
                        if (item.Fields.TryGetValue("System.State", out string systemState) &&
                            _workItemMapping.TryGetValue(systemState, out var mappedState))
                        {
                            status = mappedState;
                        }

                        list.Add(new WorkItemInfo(item.Id.Value, status));
                    }
                }

                result = list;
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(WorkItemStatus status)
        {
            IEnumerable<WorkItemInfo> result = null;

            var states = _workItemMapping.Get(status)?.ToArray();

            if ((states?.Any() ?? false) && !string.IsNullOrWhiteSpace(WorkItemType))
            {
                var builder = new StringBuilder();
                builder.Append("Select [Id] From WorkItems Where [Status.WorkItemType] = '" + WorkItemType + "' ");
                builder.Append("And [System.TeamProject] = '");
                builder.Append(Project);
                if (!string.IsNullOrWhiteSpace(Tag))
                {
                    builder.Append("' And [System.Tags] CONTAINS '");
                    builder.Append(Tag.Replace("'", "''"));
                }
                builder.Append("' And (");
                bool first = true;
                foreach (var state in states)
                {
                    if (first)
                        first = false;
                    else
                    {
                        builder.Append(" Or ");
                    }

                    builder.Append("[System.State] = '");
                    builder.Append(state);
                    builder.Append("'");
                }

                builder.Append(")");

                var query = new Wiql()
                {
                    Query = builder.ToString()
                };

                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                var queryResult = _client.QueryByWiqlAsync(query);
                var ids = queryResult.Result?.WorkItems?.Select(item => item.Id).ToArray();

                if (ids?.Any() ?? false)
                {
                    result = await GetWorkItemsInfoAsync(ids);
                }
            }

            return result;
        }
        #endregion

        #region Auxiliary methods.
        private JsonPatchDocument CreateRequest([PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation)
        {
            JsonPatchDocument result = null;

            var priority = CalculatePriority(mitigation);

            if (priority != null)
            {
                result = new JsonPatchDocument();

                AddFields(result, _workItemFieldMapping, mitigation);

                if (!string.IsNullOrWhiteSpace(Tag))
                {
                    result.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/System.Tags",
                            Value = Tag
                        }
                    );
                }

                if (MasterParent != null)
                {
                    result.Add(new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = "/relations/-",
                        Value = new
                        {
                            rel = "System.LinkTypes.Hierarchy-Reverse",
                            url = MasterParent.Url,
                            attributes = new {name = "Parent"}
                        }
                    });
                }
            }

            return result;
        }

        private void AddFields([PostSharp.Patterns.Contracts.NotNull] JsonPatchDocument doc, 
            [PostSharp.Patterns.Contracts.NotNull] Mapper<IdentityField> mapper,
            [PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation)
        {
            var keys = mapper.Keys.ToArray();

            foreach (var key in keys)
            {
                var field = mapper[key];
                object value = null;
                switch (field.FieldType)
                {
                    case IdentityFieldType.Id:
                        value = mitigation.Id.ToString("D");
                        break;
                    case IdentityFieldType.Name:
                        value = mitigation.Name;
                        break;
                    case IdentityFieldType.Description:
                        value = mitigation.Description;
                        break;
                    case IdentityFieldType.Priority:
                        value = CalculatePriority(mitigation);
                        break;
                    case IdentityFieldType.Property:
                        if (field.PropertyType != null)
                        {
                            var property = mitigation.GetProperty(field.PropertyType);
                            if (property != null)
                                value = property.StringValue;
                        }
                        break;
                }

                if (value != null)
                {
                    doc.Add(new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = $"/fields/{key}",
                        Value = value
                    });
                }
            }
        }

        private string CalculatePriority([PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation)
        {
            var result = "2";

            if (mitigation.Model is IThreatModel model)
            {
                var roadmapSchemaManager = new SimplifiedRoadmapPropertySchemaManager(model);
                var roadmapStatus = roadmapSchemaManager.GetStatus(mitigation);
                switch (roadmapStatus)
                {
                    case RoadmapStatus.NotAssessed:
                        result = "4";
                        break;
                    case RoadmapStatus.ShortTerm:
                        result = "1";
                        break;
                    case RoadmapStatus.MidTerm:
                        result = "2";
                        break;
                    case RoadmapStatus.LongTerm:
                        result = "3";
                        break;
                    case RoadmapStatus.NoActionRequired:
                        result = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }

        private async Task<WorkItemInfo> GetWorkItemInfoAsync([PostSharp.Patterns.Contracts.NotNull] IMitigation mitigation, string queryText)
        {
            WorkItemInfo result = null;

            if (!string.IsNullOrWhiteSpace(WorkItemType))
            {
                var query = new Wiql()
                {
                    Query = queryText
                };

                var queryResult = _client.QueryByWiqlAsync(query);
                var ids = queryResult.Result.WorkItems.Select(item => item.Id).ToArray();

                if (ids.Any() && ids.FirstOrDefault() is int id)
                {
                    result = (await GetWorkItemsInfoAsync(new[] {id}))?.FirstOrDefault();
                }
            }

            return result;
        }

        private async Task<IEnumerable<DevOpsItemInfo>> GetDevOpsItemsInfoAsync([PostSharp.Patterns.Contracts.NotNull] IEnumerable<int> ids)
        {
            IEnumerable<DevOpsItemInfo> result = null;
            var fields = new[] { "System.Id", "System.Title" };

            var itemsAsync = _client.GetWorkItemsAsync(Project, ids, fields);

            var items = await itemsAsync;

            if (items?.Any() ?? false)
            {
                var list = new List<DevOpsItemInfo>();

                foreach (var item in items)
                {
                    if (item.Id.HasValue)
                    {
                        if (item.Fields.TryGetValue("System.Title", out string title))
                        {
                            list.Add(new DevOpsItemInfo(item.Id.Value, title, item.Url));
                        }
                    }
                }

                result = list;
            }

            return result;
        }
        #endregion
    }
}
