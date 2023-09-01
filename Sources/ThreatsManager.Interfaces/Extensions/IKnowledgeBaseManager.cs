using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Knowledge Base Manager Extensions.
    /// </summary>
    /// <remarks>Knowledge Base Manager extensions implement ways to save and load Knowledge Bases.</remarks>
    [ExtensionDescription("Knowledge Base Manager")]
    public interface IKnowledgeBaseManager : IFileManager, IExtension
    {
        /// <summary>
        /// Load the Knowledge Base from the specified location.
        /// </summary>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Location where the Knowledge Base is to be found.</param>
        /// <param name="strict">If true, the Knowledge Base is opened requiring that content is completely understood.
        /// <para>If false, importing a Knowledge Base with unknown objects will succeed, but the unknown parts will not be imported.</para></param>
        /// <param name="newThreatModelId">Optional identifier to be used for the Threat Model replacing its configured one.</param>
        /// <returns>Threat Model containing the content of the Knowledge Base.</returns>
        /// <remarks>The returned Threat Model must be released after its use, by using ThreatsManager.Utilities.ThreatModelManager.Remove().</remarks>
        IThreatModel Load(LocationType locationType, string location, bool strict = true,
            Guid? newThreatModelId = null);

        /// <summary>
        /// Import the Knowledge Base from the specified location.
        /// </summary>
        /// <param name="target">Target Threat Model to receive the Knowledge Base.</param>
        /// <param name="definition">Definition of the information to import.</param>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Location where the Knowledge Base is to be found.</param>
        /// <param name="strict">If true, the Knowledge Base is opened requiring that content is completely understood.
        /// <para>If false, importing a Knowledge Base with unknown objects will succeed, but the unknown parts will not be imported.</para></param>
        /// <param name="newThreatModelId">Optional identifier to be used for the Threat Model replacing its configured one.</param>
        /// <returns>True if the import succeds, false otherwise.</returns>
        bool Import(IThreatModel target, DuplicationDefinition definition, 
            LocationType locationType, string location, bool strict = true,
            Guid? newThreatModelId = null);

        /// <summary>
        /// Exports the Knowledge Base to the specified location.
        /// </summary>
        /// <param name="model">Source Threat Model.</param>
        /// <param name="definition">Definition of the information to export.</param>
        /// <param name="name">Name to use for the Knowledge Base. 
        /// If missing, the name of the source Threat Model will be used.</param>
        /// <param name="description">Description to use for the Knowledge Base. 
        /// If missing, the description of the source Threat Model will be used.</param>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Chosen Location.</param>
        /// <returns>True if the file has been saved successfully, false otherwise.</returns>
        bool Export(IThreatModel model, DuplicationDefinition definition,
            string name, string description, LocationType locationType, string location);
    }
}