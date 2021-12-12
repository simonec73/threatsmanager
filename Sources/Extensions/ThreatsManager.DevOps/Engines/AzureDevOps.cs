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
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.DevOps.Engines
{
    public class AzureDevOps : IDevOpsConnector, IInitializableObject
    {
        #region Private member variables.
        private Uri _uri;
        private bool _connected;
        private string _accessToken;
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
            _workItemMapping.SetStandardMapping("Requested", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("Design", WorkItemStatus.Created);
            _workItemMapping.SetStandardMapping("In Planning", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Approved", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Ready", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Accepted", WorkItemStatus.Planned);
            _workItemMapping.SetStandardMapping("Active", WorkItemStatus.InProgress);
            _workItemMapping.SetStandardMapping("In Progress", WorkItemStatus.InProgress);
            _workItemMapping.SetStandardMapping("Committed", WorkItemStatus.InProgress);
            _workItemMapping.SetStandardMapping("Resolved", WorkItemStatus.InProgress);            
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

        public void Connect([Required] string url, [Required] string personalAccessToken)
        {
            _uri = new Uri(url);
            _accessToken = personalAccessToken;
        }

        private VssConnection GetConnection()
        {
            VssConnection result = null;

            if (_accessToken != null)
            {
                var creds = new VssBasicCredential(string.Empty, _accessToken);
                result = new VssConnection(_uri, creds);

                if (!_connected)
                {
                    _connected = true;
                    Connected?.Invoke(this, _uri.AbsoluteUri);
                }
            }

            return result;
        }

        public async Task<IEnumerable<string>> GetProjectsAsync()
        {
            IEnumerable<string> result = null;

            if (_availableProjects == null)
            {
                try
                {
                    using (var connection = GetConnection())
                    using (var projectClient = connection.GetClient<ProjectHttpClient>())
                    {
                        var projects = await projectClient.GetProjects();
                        _availableProjects = projects?.ToDictionary(x => x.Name, y => y.Id);
                    }
                }
                catch
                {
                    _availableProjects = null;
                }
            }

            result = _availableProjects?.Keys;

            return result;
        }

        public bool OpenProject([Required] string projectName)
        {
            bool result = false;

            try
            {
                if (_uri != null)
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
            _uri = null;
            _availableProjects = null;
            _accessToken = null;
            _connected = false;

            Disconnected?.Invoke(this);
        }

        public bool IsConnected()
        {
            return !string.IsNullOrWhiteSpace(Project) && _uri != null && _connected && _accessToken != null;
        }

        public bool IsConfigured()
        {
            return MasterParent != null && !string.IsNullOrWhiteSpace(WorkItemType) && WorkItemStateMappings.Any() && WorkItemFieldMappings.Any();
        }

        public string Url => _uri.AbsoluteUri;

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

                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    try
                    {
                        var ids = (await client.QueryByWiqlAsync(query).ConfigureAwait(false))?
                            .WorkItems.Select(item => item.Id).ToArray();

                        if (ids?.Any() ?? false)
                        {
                            result = await InternalGetDevOpsItemsInfoAsync(ids, client).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        result = null;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Tag management.
        public string Tag { get; set; }
        #endregion

        #region Iterations Management.
        public async Task<IEnumerable<Iteration>> GetIterationsAsync()
        {
            IEnumerable<Iteration> result = null;

            using (var connection = GetConnection())
            using (var workHttpClient = connection.GetClient<WorkHttpClient>())
            {
                try
                {
                    var queryResult = await workHttpClient.GetTeamIterationsAsync(new TeamContext(_projectId))
                        .ConfigureAwait(false);
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
                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    try
                    {
                        var queryResult = await client.GetWorkItemTypesAsync(Project).ConfigureAwait(false);
                        _itemTypes = queryResult.Select(x => x.Name).ToArray();
                    }
                    catch
                    {
                        // Ok to suppress errors here.
                    }
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
                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    try
                    {
                        var queryResult = await client.GetWorkItemTypeStatesAsync(Project, _workItemType)
                            .ConfigureAwait(false);
                        _itemStates = queryResult.Select(x => x.Name).ToArray();

                    }
                    catch
                    {
                        // Ok to suppress error here.
                    }
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
                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    try
                    {
                        var fields = (await client.GetFieldsAsync(Project).ConfigureAwait(false))?.ToArray();
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
                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    try
                    {
                        var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation, client).ConfigureAwait(false);
                        if (workItemInfo != null)
                        {
                            result = workItemInfo.Id;
                        }
                        else
                        {
                            var request = CreateRequest(mitigation);

                            if (request != null)
                            {
                                var workItem = client.CreateWorkItemAsync(request, Project, WorkItemType);
                                result = (await workItem.ConfigureAwait(false))?.Id ?? -1;
                            }
                        }
                    }
                    catch
                    {
                        result = -1;
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([NotNull] IMitigation mitigation)
        {
            WorkItemInfo result = null;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                try
                {
                    result = await InternalGetWorkItemInfoAsync(mitigation, client).ConfigureAwait(false);
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<WorkItemInfo> GetWorkItemInfoAsync([Positive] int id)
        {
            WorkItemInfo result = null;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                try
                {
                    result = (await InternalGetWorkItemsInfoAsync(new[] { id }, client).ConfigureAwait(false))
                        ?.FirstOrDefault();
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(IEnumerable<int> ids)
        {
            IEnumerable<WorkItemInfo> result = null;

            var items = ids?.Where(x => x >= 0).ToArray();

            if (items?.Any() ?? false)
            {
                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    result = await InternalGetWorkItemsInfoAsync(items, client).ConfigureAwait(false);
                }
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

                using (var connection = GetConnection())
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    var queryResult = await client.QueryByWiqlAsync(query).ConfigureAwait(false);
                    var ids = queryResult?.WorkItems?.Select(item => item.Id).ToArray();

                    if (ids?.Any() ?? false)
                    {
                        result = await InternalGetWorkItemsInfoAsync(ids, client).ConfigureAwait(false);
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<bool> SetWorkItemStateAsync(IMitigation mitigation, WorkItemStatus newStatus)
        {
            bool result = false;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation, client).ConfigureAwait(false);
                if (workItemInfo != null)
                {
                    if (newStatus == WorkItemStatus.Unknown)
                        result = await InternalRemoveWorkItemAsync(workItemInfo.Id, client).ConfigureAwait(false);
                    else    
                        result = await InternalSetWorkItemStateAsync(workItemInfo.Id, newStatus, client).ConfigureAwait(false);
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<bool> SetWorkItemStateAsync(int id, WorkItemStatus newStatus)
        {
            bool result = false;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                if (newStatus == WorkItemStatus.Unknown)
                    result = await InternalRemoveWorkItemAsync(id, client).ConfigureAwait(false);
                else    
                    result = await InternalSetWorkItemStateAsync(id, newStatus, client).ConfigureAwait(false);
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(IMitigation mitigation)
        {
            IEnumerable<Comment> result;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                try
                {
                    result = await InternalGetWorkItemCommentsAsync(mitigation, client).ConfigureAwait(false);
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        [InitializationRequired]
        public async Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(int id)
        {
            IEnumerable<Comment> result;

            using (var connection = GetConnection())
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                try
                {
                    result = await InternalGetWorkItemCommentsAsync(id, client).ConfigureAwait(false);
                }
                catch
                {
                    result = null;
                }
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
                                value = property.StringValue?.Replace("\n", "<br/>");
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

        private async Task<WorkItemInfo> InternalGetWorkItemInfoAsync([NotNull] IMitigation mitigation, 
            [NotNull] WorkItemTrackingHttpClient client)
        {
            return await InternalGetWorkItemInfoAsync(mitigation, client,
                $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.WorkItemType] = '{WorkItemType}' And [System.Title] = '{mitigation.Name}'").ConfigureAwait(false) ??
                                  await InternalGetWorkItemInfoAsync(mitigation, client,
                $"Select [Id] From WorkItems Where [System.TeamProject] = '{Project}' And [System.Title] = '{mitigation.Name.Replace("'", "''")}'").ConfigureAwait(false);
        }

        private async Task<WorkItemInfo> InternalGetWorkItemInfoAsync([NotNull] IMitigation mitigation, 
            [NotNull] WorkItemTrackingHttpClient client, string queryText)
        {
            WorkItemInfo result = null;

            if (!string.IsNullOrWhiteSpace(WorkItemType))
            {
                try
                {
                    var query = new Wiql()
                    {
                        Query = queryText
                    };

                    var queryResult = await client.QueryByWiqlAsync(query).ConfigureAwait(false);
                    var ids = queryResult?.WorkItems.Select(item => item.Id).ToArray();

                    if ((ids?.Any() ?? false) && ids.FirstOrDefault() is int id)
                    {
                        result = (await GetWorkItemsInfoAsync(new[] { id }).ConfigureAwait(false))?.FirstOrDefault();
                    }
                }
                catch (VssServiceException)
                {
                    result = null;
                }
            }

            return result;
        }

        private async Task<IEnumerable<WorkItemInfo>> InternalGetWorkItemsInfoAsync(IEnumerable<int> ids, [NotNull] WorkItemTrackingHttpClient client)
        {
            IEnumerable<WorkItemInfo> result = null;

            var fields = new[] {"System.Id", "System.State", "System.AssignedTo"};

            var items = await client.GetWorkItemsAsync(Project, ids, fields, null, null, WorkItemErrorPolicy.Omit).ConfigureAwait(false);
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

                        string name = null;
                        if (item.Fields.TryGetValue("System.AssignedTo", out IdentityRef assignedTo))
                        {
                            name = assignedTo.DisplayName;
                        }

                        list.Add(new WorkItemInfo(item.Id.Value, item.Url, name, status));
                    }
                }

                result = list;
            }

            return result;
        }

        private async Task<bool> InternalSetWorkItemStateAsync(int id, WorkItemStatus newStatus, [NotNull] WorkItemTrackingHttpClient client)
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

                var updatedWorkItem = await client.UpdateWorkItemAsync(patchDocument, id).ConfigureAwait(false);
                if (updatedWorkItem != null)
                    result = true;
            }

            return result;
        }

        private async Task<IEnumerable<DevOpsItemInfo>> InternalGetDevOpsItemsInfoAsync([NotNull] IEnumerable<int> ids, 
            [NotNull] WorkItemTrackingHttpClient client)
        {
            IEnumerable<DevOpsItemInfo> result = null;
            var fields = new[] { "System.Id", "System.Title", "System.WorkItemType", "System.AssignedTo" };

            var items = await client.GetWorkItemsAsync(Project, ids, fields).ConfigureAwait(false);

            if (items?.Any() ?? false)
            {
                var list = new List<DevOpsItemInfo>();

                foreach (var item in items)
                {
                    if (item.Id.HasValue)
                    {
                        if (item.Fields.TryGetValue("System.Title", out string title) && 
                            item.Fields.TryGetValue("System.WorkItemType", out string workItemType))
                        {
                            string name = null;
                            if (item.Fields.TryGetValue("System.AssignedTo", out IdentityRef assignedTo))
                                name = assignedTo.DisplayName;

                            list.Add(new DevOpsItemInfo(item.Id.Value, title, item.Url, workItemType, name));
                        }
                    }
                }

                result = list;
            }

            return result;
        }

        private async Task<IEnumerable<Comment>> InternalGetWorkItemCommentsAsync(IMitigation mitigation, [NotNull] WorkItemTrackingHttpClient client)
        {
            IEnumerable<Comment> result = null;

            var workItemInfo = await InternalGetWorkItemInfoAsync(mitigation, client);

            if (workItemInfo != null)
            {
                result = await InternalGetWorkItemCommentsAsync(workItemInfo.Id, client);
            }

            return result;
        }

        private async Task<IEnumerable<Comment>> InternalGetWorkItemCommentsAsync(int id, [NotNull] WorkItemTrackingHttpClient client)
        {
            IEnumerable<Comment> result = null;

            var items = await client.GetCommentsAsync(Project, id).ConfigureAwait(false);
            if ((items?.Count ?? 0) > 0)
            {
                result = items.Comments.Select(x => new Comment(x.Text, x.CreatedBy.DisplayName, x.CreatedDate.Date));
            }

            return result;
        }
        
        private async Task<bool> InternalRemoveWorkItemAsync(int id, [NotNull] WorkItemTrackingHttpClient client)
        {
            bool result = false;

            var deletedWorkItem = await client.DeleteWorkItemAsync(id, false).ConfigureAwait(false);
            if (deletedWorkItem != null)
                result = true;

            return result;
        }

        #endregion
    }
}
