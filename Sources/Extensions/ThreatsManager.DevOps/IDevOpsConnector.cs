using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Interfaces implemented by the connectors to DevOps systems.
    /// </summary>
    public interface IDevOpsConnector
    {
        #region Relationship with Factory.
        /// <summary>
        /// Identifier of the associated Factory.
        /// </summary>
        string FactoryId { get; }
        #endregion

        #region Connection management.
        /// <summary>
        /// Event raised when the DevOps Connector is connected.
        /// </summary>
        /// <remarks>The parameters of the connection are the DevOps Connector and the Url of the DevOps Service.</remarks>
        event Action<IDevOpsConnector, string> Connected;

        /// <summary>
        /// Event raised when the DevOps Connector is disconnected.
        /// </summary>
        /// <remarks>The parameter of the connection is the DevOps Connector.</remarks>
        event Action<IDevOpsConnector> Disconnected;

        /// <summary>
        /// Event raised when a Project has been opened.
        /// </summary>
        /// <remarks>The parameters of the connection are the DevOps Connector and the name of the project.</remarks>
        event Action<IDevOpsConnector, string> ProjectOpened;

        /// <summary>
        /// Initiates connection to a DevOps server.
        /// </summary>
        /// <param name="url">Main URL for the server.</param>
        /// <param name="personalAccessToken">Token used to access the DevOps software.</param>
        void Connect(string url, string personalAccessToken);

        /// <summary>
        /// Gets the projects associated to the current connection.
        /// </summary>
        /// <returns></returns>
        /// <returns>List of Projects that the user can connect to.</returns>
        Task<IEnumerable<string>> GetProjectsAsync();

        /// <summary>
        /// Connect to a specific Project.
        /// </summary>
        /// <param name="project">Project name.</param>
        /// <returns>True if the connection succeeds, false otherwise.</returns>
        bool OpenProject(string project);

        /// <summary>
        /// Disconnect from the DevOps server.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Check if the Connector is connected to the DevOps server.
        /// </summary>
        /// <returns>True if the connector is connected, otherwise false.</returns>
        bool IsConnected();

        /// <summary>
        /// Check if the Connector is fully configured and operational.
        /// </summary>
        /// <returns>True if the connector is fully operational, otherwise false.</returns>
        bool IsConfigured();

        /// <summary>
        /// Url of the Server to which the Connector is connected.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Project to which the Connector is connected.
        /// </summary>
        string Project { get; }
        #endregion

        #region Parent management.
        /// <summary>
        /// Information about the Parent to be used for all the objects managed by the Connector.
        /// </summary>
        IDevOpsItemInfo MasterParent { get; set; }

        /// <summary>
        /// Search for items to be shown and selected to be used as Master Parent.
        /// </summary>
        /// <param name="filter">Filter to be used for the search. It must contain at least 3 characters.</param>
        /// <returns>Enumeration of the items whose name includes the three characters.</returns>
        Task<IEnumerable<IDevOpsItemInfo>> GetItemsAsync(string filter);
        #endregion

        #region Tag management.
        /// <summary>
        /// Tag used to recognize the items managed by the Platform.
        /// </summary>
        string Tag { get; set; }
        #endregion

        #region Iterations Management.
        /// <summary>
        /// Get the Iterations defined for the Project.
        /// </summary>
        /// <returns>Enumeration with all the Iterations defined for the Project.</returns>
        Task<IEnumerable<Iteration>> GetIterationsAsync();
        #endregion

        #region Work Item Types management.
        /// <summary>
        /// Get the list of supported Work Item Types.
        /// </summary>
        /// <returns>List of supported Work Item Types.</returns>
        Task<IEnumerable<string>> GetWorkItemTypesAsync();

        /// <summary>
        /// Property to get and set the Work Item Type for new Work Items.
        /// </summary>
        string WorkItemType { get; set; }
        #endregion

        #region Work Item States management.
        /// <summary>
        /// Get the current mapping between the Work Item states and the states supported by the DevOps system.
        /// </summary>
        IEnumerable<KeyValuePair<string, WorkItemStatus>> WorkItemStateMappings { get; }

        /// <summary>
        /// Set a mapping between state supported by the DevOps system and Work Item States.
        /// </summary>
        /// <param name="devOpsState">State supported by the DevOps system.</param>
        /// <param name="status">Associated Work Item Status.</param>
        /// <remarks>To remove an association, map it to WorkItemStatus.Unknown.</remarks>
        void SetWorkItemStateMapping(string devOpsState, WorkItemStatus status);

        /// <summary>
        /// Get Work Item States from the DevOps system.
        /// </summary>
        /// <returns>List of States supported by the DevOps system.</returns>
        Task<IEnumerable<string>> GetWorkItemStatesAsync();
        #endregion

        #region Work Item Fields management.
        /// <summary>
        /// Get the current mapping between the Fields supported by the DevOps system for the Work Item
        /// and the fields defined in the Threat Model for the Mitigations.
        /// </summary>
        IEnumerable<KeyValuePair<string, IdentityField>> WorkItemFieldMappings { get; }

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Work Items.
        /// </summary>
        /// <param name="devOpsField">Definition of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetWorkItemFieldMapping(IDevOpsField devOpsField, IdentityField modelField);

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Work Items.
        /// </summary>
        /// <param name="devOpsFieldId">Identifier of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetWorkItemFieldMapping(string devOpsFieldId, IdentityField modelField);

        /// <summary>
        /// Get the list of fields supported by the DevOps system for the Work Item.
        /// </summary>
        /// <returns>List of known DevOps fields.</returns>
        Task<IEnumerable<IDevOpsField>> GetWorkItemDevOpsFieldsAsync();
        #endregion

        #region Work Items management.
        /// <summary>
        /// Event raised when a Work Item changes status.
        /// </summary>
        event Action<WorkItemInfo> WorkItemStatusChanged;

        /// <summary>
        /// Create a mitigation associated to a Work Item.
        /// </summary>
        /// <param name="mitigation">Mitigation to be associated to a Work Item.</param>
        /// <returns>Identifier of the Work Item. It may be -1 if the Task creation has failed.</returns>
        Task<int> CreateWorkItemAsync(IMitigation mitigation);

        /// <summary>
        /// Get updated information about a Work Item.
        /// </summary>
        /// <param name="mitigation">Mitigation associated to the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        Task<WorkItemInfo> GetWorkItemInfoAsync(IMitigation mitigation);

        /// <summary>
        /// Get updated information about a Work Item.
        /// </summary>
        /// <param name="id">Identifier of the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        Task<WorkItemInfo> GetWorkItemInfoAsync(int id);

        /// <summary>
        /// Get information about multiple Work Items.
        /// </summary>
        /// <param name="ids">Enumeration of the Work Items identifiers.</param>
        /// <returns>Enumeration with the information for each Work Items.</returns>
        Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(IEnumerable<int> ids);

        /// <summary>
        /// Get information about the Work Items that are in a specific status.
        /// </summary>
        /// <param name="status">Desired status.</param>
        /// <returns>Information about the Work Items in the desired status.</returns>
        Task<IEnumerable<WorkItemInfo>> GetWorkItemsInfoAsync(WorkItemStatus status);

        /// <summary>
        /// Set the state of a Work Item.
        /// </summary>
        /// <param name="mitigation">Mitigation associated to the Work Item.</param>
        /// <param name="newStatus">New status for the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        Task<bool> SetWorkItemStateAsync(IMitigation mitigation, WorkItemStatus newStatus);

        /// <summary>
        /// Set the status of a Work Item.
        /// </summary>
        /// <param name="id">Identifier of the Work Item.</param>
        /// <param name="newStatus">New status for the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        Task<bool> SetWorkItemStateAsync(int id, WorkItemStatus newStatus);

        /// <summary>
        /// Get the list of comments associated with the Mitigation.
        /// </summary>
        /// <param name="mitigation">Mitigation whose comments are to be retrieved.</param>
        /// <returns>Enumeration of the comments.</returns>
        Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(IMitigation mitigation);

        /// <summary>
        /// Get the list of comments associated with a Work Item.
        /// </summary>
        /// <param name="id">Identified of the Work Item.</param>
        /// <returns>Enumeration of the available comments.</returns>
        Task<IEnumerable<Comment>> GetWorkItemCommentsAsync(int id);
        #endregion
    }
}