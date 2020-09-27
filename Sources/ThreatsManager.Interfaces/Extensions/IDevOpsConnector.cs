using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interfaces implemented by the connectors to DevOps systems.
    /// </summary>
    public interface IDevOpsConnector
    {
        #region Connection management.
        /// <summary>
        /// Initiates connection to a DevOps server.
        /// </summary>
        /// <param name="url">Main URL for the server.</param>
        /// <returns>List of Projects that the user can connect to.</returns>
        IEnumerable<string> Connect(string url);

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
        IEnumerable<IDevOpsItemInfo> GetItems(string filter);
        #endregion

        #region Tag management.
        /// <summary>
        /// Tag used to recognize the items managed by the Platform.
        /// </summary>
        string Tag { get; set; }
        #endregion

        #region Work Item Types management.
        /// <summary>
        /// Get the list of supported Work Item Types.
        /// </summary>
        /// <returns>List of supported Work Item Types.</returns>
        IEnumerable<string> GetWorkItemTypes();

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
        IEnumerable<IDevOpsField> GetWorkItemDevOpsFields();
        #endregion

        #region Work Items management.
        /// <summary>
        /// Create a mitigation associated to a Work Item.
        /// </summary>
        /// <param name="mitigation">Mitigation to be associated to a Work Item.</param>
        /// <returns>Identifier of the Work Item. It may be -1 if the Task creation has failed.</returns>
        int CreateWorkItem(IMitigation mitigation);

        /// <summary>
        /// Get updated information about a Work Item.
        /// </summary>
        /// <param name="mitigation">Mitigation associated to the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        WorkItemInfo GetWorkItemInfo(IMitigation mitigation);

        /// <summary>
        /// Get updated information about a Work Item.
        /// </summary>
        /// <param name="id">Identifier of the Work Item.</param>
        /// <returns>Information about the Work Item.</returns>
        WorkItemInfo GetWorkItemInfo(int id);

        /// <summary>
        /// Get information about multiple Work Items.
        /// </summary>
        /// <param name="ids">Enumeration of the Work Items identifiers.</param>
        /// <returns>Dictionary containing the information for each Work Items.</returns>
        IDictionary<int, WorkItemInfo> GetWorkItemsInfo(IEnumerable<int> ids);

        /// <summary>
        /// Get information about the Work Items that are in a specific status.
        /// </summary>
        /// <param name="status">Desired status.</param>
        /// <returns>Information about the Work Items in the desired status.</returns>
        IEnumerable<WorkItemInfo> GetWorkItemsInfo(WorkItemStatus status);
        #endregion
    }
}