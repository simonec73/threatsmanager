using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interfaces implemented by the connectors to DevOps systems.
    /// </summary>
    public interface IDevOpsConnector
    {
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
        /// Url of the Server to which the Connector is connected.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Project to which the Connector is connected.
        /// </summary>
        string Project { get; }

        /// <summary>
        /// Identifier of the Parent to be used for all the objects managed by the Connector.
        /// </summary>
        int MasterParent { get; set; }

        /// <summary>
        /// Search for items to be shown and selected to be used as Master Parent.
        /// </summary>
        /// <param name="filter">Filter to be used for the search. It must contain at least 3 characters.</param>
        /// <returns>Enumeration of the items whose name includes the three characters.</returns>
        IEnumerable<DevOpsItemInfo> GetItems(string filter);

        /// <summary>
        /// Check if the Connector is connected to the DevOps server.
        /// </summary>
        /// <returns>True if the connector is connected, otherwise false.</returns>
        bool IsConnected();

        /// <summary>
        /// Tag used to recognize the items managed by the Platform.
        /// </summary>
        string Tag { get; set; }

        #region Bugs management.
        /// <summary>
        /// Get the current mapping between states supported by the DevOps system and the Bug Status.
        /// </summary>
        IEnumerable<KeyValuePair<string, BugStatus>> BugStateMappings { get; }

        /// <summary>
        /// Set a mapping between state supported by the DevOps system and Bug States.
        /// </summary>
        /// <param name="devOpsState">State supported by the DevOps system.</param>
        /// <param name="status">Associated Bug Status.</param>
        /// <remarks>To remove an association, map BugStatus.Unknown.</remarks>
        void SetBugStateMapping(string devOpsState, BugStatus status);

        /// <summary>
        /// Get the current mapping between the Fields supported by the DevOps system for the Bugs and the fields defined in the Threat Model for the Threat Events.
        /// </summary>
        IEnumerable<KeyValuePair<string, IdentityField>> BugFieldMappings { get; }

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Bugs.
        /// </summary>
        /// <param name="devOpsField">Definition of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetBugFieldMapping(DevOpsField devOpsField, IdentityField modelField);

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Bugs.
        /// </summary>
        /// <param name="devOpsFieldId">Identifier of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetBugFieldMapping(string devOpsFieldId, IdentityField modelField);

        /// <summary>
        /// Get the list of fields supported by the DevOps system for the Bugs.
        /// </summary>
        /// <returns>List of known DevOps fields.</returns>
        IEnumerable<DevOpsField> GetBugDevOpsFields();

        /// <summary>
        /// Create a Bug out of a Threat Event.
        /// </summary>
        /// <param name="threatEvent">Threat Event to be used to generate the Bug.</param>
        /// <returns>Identifier of the Bug. It may be -1 if the Bug creation has failed.</returns>
        int CreateBug(IThreatEvent threatEvent);

        /// <summary>
        /// Get updated information about a Bug.
        /// </summary>
        /// <param name="threatEvent">Threat Event associated with the Bug.</param>
        /// <returns>Information about the Bug.</returns>
        BugInfo GetBugInfo(IThreatEvent threatEvent);

        /// <summary>
        /// Get updated information about a Bug.
        /// </summary>
        /// <param name="id">Identifier of the Bug.</param>
        /// <returns>Information about the Bug.</returns>
        BugInfo GetBugInfo(int id);

        /// <summary>
        /// Get updated information about multiple Bugs.
        /// </summary>
        /// <param name="ids">Enumeration of the Bug identifiers.</param>
        /// <returns>Dictionary containing the info for each Bug.</returns>
        IDictionary<int, BugInfo> GetBugInfo(IEnumerable<int> ids);

        /// <summary>
        /// Get information about the Bugs that are in a specific status.
        /// </summary>
        /// <param name="status">Desired status.</param>
        /// <returns>Information about the Bugs in the desired status.</returns>
        IEnumerable<BugInfo> GetBugInfo(BugStatus status);
        #endregion

        #region Task management.
        /// <summary>
        /// Get the current mapping between the Task States and the states supported by the DevOps system.
        /// </summary>
        IEnumerable<KeyValuePair<string, TaskStatus>> TaskStateMappings { get; }

        /// <summary>
        /// Set a mapping between state supported by the DevOps system and Task States.
        /// </summary>
        /// <param name="devOpsState">State supported by the DevOps system.</param>
        /// <param name="status">Associated Task Status.</param>
        /// <remarks>To remove an association, map TaskStatus.Unknown.</remarks>
        void SetTaskStateMapping(string devOpsState, TaskStatus status);

        /// <summary>
        /// Get the current mapping between the Fields supported by the DevOps system for the Tasks and the fields defined in the Threat Model for the Mitigations.
        /// </summary>
        IEnumerable<KeyValuePair<string, IdentityField>> TaskFieldMappings { get; }

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Tasks.
        /// </summary>
        /// <param name="devOpsField">Definition of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetTaskFieldMapping(DevOpsField devOpsField, IdentityField modelField);

        /// <summary>
        /// Set the mapping between a field defined in the DevOps and a internal field as defined in the Model, for Tasks.
        /// </summary>
        /// <param name="devOpsFieldId">Identifier of the DevOps field.</param>
        /// <param name="modelField">Definition of the field in the Threat Model.</param>
        void SetTaskFieldMapping(string devOpsFieldId, IdentityField modelField);

        /// <summary>
        /// Get the list of fields supported by the DevOps system for the Tasks.
        /// </summary>
        /// <returns>List of known DevOps fields.</returns>
        IEnumerable<DevOpsField> GetTaskDevOpsFields();

        /// <summary>
        /// Create a mitigation associated to a Task.
        /// </summary>
        /// <param name="mitigation">Mitigation to a Task.</param>
        /// <returns>Identifier of the Task. It may be -1 if the Task creation has failed.</returns>
        int CreateTask(IThreatEventMitigation mitigation);

        /// <summary>
        /// Get updated information about a Task.
        /// </summary>
        /// <param name="mitigation">Mitigation associated to the Task.</param>
        /// <returns>Information about the Task.</returns>
        TaskInfo GetTaskInfo(IThreatEventMitigation mitigation);

        /// <summary>
        /// Get updated information about a Task.
        /// </summary>
        /// <param name="id">Identifier of the Task.</param>
        /// <returns>Information about the Task.</returns>
        TaskInfo GetTaskInfo(int id);

        /// <summary>
        /// Get information about multiple Tasks.
        /// </summary>
        /// <param name="ids">Enumeration of the Task identifiers.</param>
        /// <returns>Dictionary containing the information for each Task.</returns>
        IDictionary<int, TaskInfo> GetTaskInfo(IEnumerable<int> ids);

        /// <summary>
        /// Get information about the Tasks that are in a specific status.
        /// </summary>
        /// <param name="status">Desired status.</param>
        /// <returns>Information about the Tasks in the desired status.</returns>
        IEnumerable<TaskInfo> GetTaskInfo(TaskStatus status);
        #endregion
    }
}