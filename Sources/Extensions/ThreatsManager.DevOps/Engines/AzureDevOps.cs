using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using PostSharp.Patterns.Contracts;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using ThreatsManager.Extensions.Client.Schemas;
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
        private Guid _projectId;
        private IDictionary<string, Guid> _availableProjects;
        private IEnumerable<string> _itemTypes;
        private IEnumerable<string> _itemStates;
        private IEnumerable<IDevOpsField> _fields;
        private readonly Mapper<WorkItemStatus> _workItemMapping = new Mapper<WorkItemStatus>();
        private readonly Mapper<IdentityField> _workItemFieldMapping = new Mapper<IdentityField>();
        #endregion

        #region Constructors.
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
        #endregion

        #region Other public methods.
        public bool IsInitialized => IsConnected();

        public override string ToString()
        {
            return "Microsoft Azure DevOps";
        }
        #endregion

        #region Relationship with Factory.
        public string FactoryId => "0EB212AB-EBA8-483D-A76C-E2D31CEFFCE1";
        #endregion

        #region Connection management.
        public event Action<IDevOpsConnector, string> Connected;
        public event Action<IDevOpsConnector> Disconnected;
        public event Action<IDevOpsConnector, string> ProjectOpened;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<string> Connect([Required] string url)
        {
            IEnumerable<string> result = null;

            try
            {
                _url = url;

                _connection = new VssConnection(new Uri(_url), new VssClientCredentials());
                using (var projectClient = _connection.GetClient<ProjectHttpClient>())
                {
                    _availableProjects = projectClient.GetProjects().Result
                        .ToDictionary(x => x.Name, y => y.Id);
                }

                Connected?.Invoke(this, url);

                result = _availableProjects?.Keys;
            }
            catch
            {
                _connection = null;
                _url = null;
                _availableProjects = null;
            }

            return result;
        }

        public bool OpenProject([Required] string projectName)
        {
            bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(_url))
                {
                    var project = _availableProjects?
                        .FirstOrDefault(x => string.CompareOrdinal(projectName, x.Key) == 0);

                    if (project.HasValue)
                    {
                        Project = project.Value.Key;
                        _projectId = project.Value.Value;

                        ProjectOpened?.Invoke(this, projectName);

                        result = true;
                    }
                }
            }
            catch
            {
                Project = null;
                _projectId = Guid.Empty;
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

            Disconnected?.Invoke(this);
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

                try
                {
                    var ids = (await _client.QueryByWiqlAsync(query).ConfigureAwait(false))?
                        .WorkItems.Select(item => item.Id).ToArray();

                    if (ids?.Any() ?? false)
                    {
                        result = await InternalGetDevOpsItemsInfoAsync(ids).ConfigureAwait(false);
                    }
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }
        #endregion

        #region Tag management.
        public string Tag { get; set; }
        #endregion

        #region Iterations Management.
        public async Task<IEnumerable<Iteration>> GetIterations()
        {
            IEnumerable<Iteration> result = null;

            var workHttpClient = _connection.GetClient<WorkHttpClient>();

            try
            {
                var queryResult = await workHttpClient.GetTeamIterationsAsync(new TeamContext(_projectId)).ConfigureAwait(false);
                result = queryResult?.Select(x => new Iteration()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Url = x.Url,
                    Start = x.Attributes?.StartDate,
                    End = x.Attributes?.FinishDate
                });
            }
            catch
            {
                result = null;
            }

            return result;
        }
        #endregion

        #region Work Item Types management.
        [InitializationRequired]
        public async Task<IEnumerable<string>> GetWorkItemTypesAsync()
        {
            if (_itemTypes == null)
            {
                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                try
                {
                    var queryResult = await _client.GetWorkItemTypesAsync(Project).ConfigureAwait(false);
                    _itemTypes = queryResult.Select(x => x.Name).ToArray();
                }
                catch
                {
                    // Ok to suppress errors here.
                }
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

                try
                {
                    var queryResult = await _client.GetWorkItemTypeStatesAsync(Project, _workItemType).ConfigureAwait(false);
                    _itemStates = queryResult.Select(x => x.Name).ToArray();

                }
                catch
                {
                    // Ok to suppress error here.
                }
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

                try
                {
                    var fields = (await _client.GetFieldsAsync(Project).ConfigureAwait(false))?.ToArray();
                    if (fields?.Any() ?? false)
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
                catch
                {
                    // Ok to suppress error here.
                }
            }

            return _fields;
        }
        #endregion

        #region Work Items management.
        public event Action<WorkItemInfo> WorkItemStatusChanged;

        [InitializationRequired(-1)]
        public async Task<int> CreateWorkItemAsync([NotNull] IMitigation mitigation)
        {
            var result = -1;

            if (!string.IsNullOrWhiteSpace(WorkItemType))
            {
                if (_client == null)
                    _client = _connection.GetClient<WorkItemTrackingHttpClient>();

                try
                {
                    var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation).ConfigureAwait(false);
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
                            result = (await workItem.ConfigureAwait(false))?.Id ?? -1;
                        }
                    }
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([NotNull] IMitigation mitigation)
        {
            WorkItemInfo result = null;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                result = await InternalGetWorkItemInfoAsync(mitigation).ConfigureAwait(false);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([Positive] int id)
        {
            WorkItemInfo result = null;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                result = (await InternalGetWorkItemsInfoAsync(new[] { id }).ConfigureAwait(false))?.FirstOrDefault();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(IEnumerable<int> ids)
        {
            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            return await InternalGetWorkItemsInfoAsync(ids).ConfigureAwait(false);
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

                var queryResult = await _client.QueryByWiqlAsync(query).ConfigureAwait(false);
                var ids = queryResult?.WorkItems?.Select(item => item.Id).ToArray();

                if (ids?.Any() ?? false)
                {
                    result = await InternalGetWorkItemsInfoAsync(ids).ConfigureAwait(false);
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<bool> SetWorkItemStateAsync(IMitigation mitigation, WorkItemStatus newStatus)
        {
            bool result = false;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation).ConfigureAwait(false);
            if (workItemInfo != null)
            {
                result = await InternalSetWorkItemStateAsync(workItemInfo.Id, newStatus).ConfigureAwait(false);
            }

            return result;
        }

        [InitializationRequired]
        public async Task<bool> SetWorkItemStateAsync(int id, WorkItemStatus newStatus)
        {
            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            return await InternalSetWorkItemStateAsync(id, newStatus).ConfigureAwait(false);
        }

        [InitializationRequired]
        public async Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(IMitigation mitigation)
        {
            IEnumerable<Comment> result;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                result = await InternalGetWorkItemCommentsAsync(mitigation).ConfigureAwait(false);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(int id)
        {
            IEnumerable<Comment> result;

            if (_client == null)
                _client = _connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                result = await InternalGetWorkItemCommentsAsync(id).ConfigureAwait(false);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        #endregion

        #region Auxiliary methods.
        private JsonPatchDocument CreateRequest([NotNull] IMitigation mitigation)
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

        private void AddFields([NotNull] JsonPatchDocument doc, 
            [NotNull] Mapper<IdentityField> mapper,
            [NotNull] IMitigation mitigation)
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
                        value = mitigation.Description.Replace("\n", "<br/>");
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
                    var name = key.StartsWith("/") ? key : $"/fields/{key}";

                    if (!doc.Any(x => string.CompareOrdinal(x.Path, name) == 0))
                    {
                        doc.Add(new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = name,
                            Value = value
                        });
                    }
                }
            }
        }

        private string CalculatePriority([NotNull] IMitigation mitigation)
        {
            var result = "2";

            if (mitigation.Model is IThreatModel model)
            {
                var roadmapSchemaManager = new RoadmapPropertySchemaManager(model);
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

        private async Task<WorkItemInfo> InternalGetWorkItemInfoAsync([NotNull] IMitigation mitigation)
        {
            return await InternalGetWorkItemInfoAsync(mitigation,
                $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.WorkItemType] = '{WorkItemType}' And [System.Title] = '{mitigation.Name}'").ConfigureAwait(false) ??
                                  await InternalGetWorkItemInfoAsync(mitigation,
                $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.Title] = '{mitigation.Name.Replace("'", "''")}'").ConfigureAwait(false);
        }

        private async Task<WorkItemInfo> InternalGetWorkItemInfoAsync([NotNull] IMitigation mitigation, string queryText)
        {
            WorkItemInfo result = null;

            if (!string.IsNullOrWhiteSpace(WorkItemType))
            {
                var query = new Wiql()
                {
                    Query = queryText
                };

                var queryResult = await _client.QueryByWiqlAsync(query).ConfigureAwait(false);
                var ids = queryResult?.WorkItems.Select(item => item.Id).ToArray();

                if ((ids?.Any() ?? false) && ids.FirstOrDefault() is int id)
                {
                    result = (await GetWorkItemsInfoAsync(new[] {id}).ConfigureAwait(false))?.FirstOrDefault();
                }
            }

            return result;
        }

        private async Task<IEnumerable<WorkItemInfo>> InternalGetWorkItemsInfoAsync(IEnumerable<int> ids)
        {
            IEnumerable<WorkItemInfo> result = null;

            var fields = new[] {"System.Id", "System.State"};

            var items = await _client.GetWorkItemsAsync(Project, ids, fields).ConfigureAwait(false);
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

                        list.Add(new WorkItemInfo(item.Id.Value, item.Url, status));
                    }
                }

                result = list;
            }

            return result;
        }

        private async Task<bool> InternalSetWorkItemStateAsync(int id, WorkItemStatus newStatus)
        {
            bool result = false;

            var supportedStates = (await GetWorkItemStatesAsync().ConfigureAwait(false))?.ToArray();
            var states = _workItemMapping.Get(newStatus)?.ToArray();
            var state = states?.FirstOrDefault(x => supportedStates?.Contains(x) ?? false);

            if (state != null)
            {
                var patchDocument = new JsonPatchDocument
                {
                    new JsonPatchOperation()
                    {
                        Operation = Operation.Add, Path = "/fields/System.State", Value = state
                    }
                };

                var updatedWorkItem = await _client.UpdateWorkItemAsync(patchDocument, id).ConfigureAwait(false);
                if (updatedWorkItem != null)
                    result = true;
            }

            return result;
        }

        private async Task<IEnumerable<DevOpsItemInfo>> InternalGetDevOpsItemsInfoAsync([NotNull] IEnumerable<int> ids)
        {
            IEnumerable<DevOpsItemInfo> result = null;
            var fields = new[] { "System.Id", "System.Title" };

            var items = await _client.GetWorkItemsAsync(Project, ids, fields).ConfigureAwait(false);

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

        private async Task<IEnumerable<Comment>> InternalGetWorkItemCommentsAsync(IMitigation mitigation)
        {
            IEnumerable<Comment> result = null;

            var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation);

            if (workItemInfo != null)
            {
                result = await InternalGetWorkItemCommentsAsync(workItemInfo.Id);
            }

            return result;
        }

        private async Task<IEnumerable<Comment>> InternalGetWorkItemCommentsAsync(int id)
        {
            IEnumerable<Comment> result = null;

            var items = await _client.GetCommentsAsync(Project, id).ConfigureAwait(false);
            if ((items?.Count ?? 0) > 0)
            {
                result = items.Comments.Select(x => new Comment(x.Text, x.CreatedBy.DisplayName, x.CreatedDate.Date));
            }

            return result;
        }
        #endregion
    }
}
