using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Package Manager Extensions.
    /// </summary>
    /// <remarks>Package Manager extensions implement ways to save and load Threat Models.</remarks>
    [ExtensionDescription("Package Manager")]
    public interface IPackageManager : IFileManager, IExtension
    {
        /// <summary>
        /// Get the location of the latest version available for a given Threat Model.
        /// </summary>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Location where the main Threat Model is to be found.</param>
        /// <param name="dateTime">Date and time of creation of the new version.</param>
        /// <returns>Location of the latest version. It return null if there is no alternative version.</returns>
        string GetLatest(LocationType locationType, string location, out DateTime dateTime);

        /// <summary>
        /// Load the Threat Model from the specified location.
        /// </summary>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Location where the Threat Model is to be found.</param>
        /// <param name="extensions">List of enabled extensions.
        /// <para>It is used to provide more meaningful errors when the operation fails due to a missing extension.</para></param>
        /// <param name="strict">If true, the Threat Model is opened requiring that content is completely understood.
        /// <para>If false, opening a Threat Model with unknown objects will succeed, but the unknown parts will not be imported.</para></param>
        /// <param name="newThreatModelId">Optional identifier to be used for the Threat Model replacing its configured one.</param>
        /// <returns>Threat Model loaded from the specified location.</returns>
        /// <exception cref="System.IO.FileNotFoundException">The file specified in the location cannot be found.</exception>
        /// <exception cref="Exceptions.ThreatModelOpeningFailureException">The Threat Model cannot be opened for some reason. It is most typically due to a deserialization issues.</exception>
        /// <exception cref="Exceptions.EncryptionRequiredException">The Package Manager requires encryption, which has not been configured: see property RequiredProtection for details.</exception>
        /// <exception cref="Exceptions.FileEncryptedException">The file cannot be opened because it is encrypted.</exception>
        /// <exception cref="Exceptions.FileNotEncryptedException">The file cannot be opened because it is not encrypted while it should, or it lacks the embedded configuration file necessary to unencrypt it.</exception>
        IThreatModel Load(LocationType locationType, string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true,
            Guid? newThreatModelId = null);

        /// <summary>
        /// Save the model to the specified location.
        /// </summary>
        /// <param name="model">Threat Model to be saved.</param>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Chosen Location.</param>
        /// <param name="autoAddDateTime">Add automatically the current date and time, to avoid writing over an existing Threat Model.</param>
        /// <param name="extensions">List of enabled extensions.
        /// <para>It is used to provide more meaningful errors when loading fails due to a missing extension.</para></param>
        /// <param name="newLocation">[out] Calculated location for the file, which would be different from the input Location if the autoAddDateTime is true.</param>
        /// <returns>True if the file has been saved successfully, false otherwise.</returns>
        /// <exception cref="Exceptions.EncryptionRequiredException">The Package Manager requires encryption: see property RequiredProtection for details.</exception>
        bool Save(IThreatModel model, LocationType locationType, string location, bool autoAddDateTime,
            IEnumerable<IExtensionMetadata> extensions, out string newLocation);

        /// <summary>
        /// Performs automatic cleanup of the old instances.
        /// </summary>
        /// <param name="locationType">Location Type.</param>
        /// <param name="location">Chosen Location.</param>
        /// <param name="maxInstances">Max number of instances to maintain. It must be a positive number.</param>
        void AutoCleanup(LocationType locationType, string location, int maxInstances);
    }
}