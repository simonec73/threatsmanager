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
    public interface IPackageManager : IExtension
    {
        /// <summary>
        /// Supported locations.
        /// </summary>
        LocationType SupportedLocations { get; }

        /// <summary>
        /// Get the Filter for the specified location type.
        /// </summary>
        /// <param name="locationType">Location Type.</param>
        /// <returns>Filter to be used for getting the location.</returns>
        string GetFilter(LocationType locationType);

        /// <summary>
        /// Verifies if the Package Manager can handle the location identified with the arguments.
        /// </summary>
        /// <param name="locationType">Type of location.</param>
        /// <param name="location">Location where the Threat Model is to be found.</param>
        /// <returns>True if the location can be handled by the Package Manager, false otherwise.</returns>
        bool CanHandle(LocationType locationType, string location);

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
        /// <returns>Threat Model loaded from the specified location.</returns>
        /// <exception cref="Exceptions.EncryptionRequiredException">The Package Manager requires encryption: see property RequiredProtection for details.</exception>
        IThreatModel Load(LocationType locationType, string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true);

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