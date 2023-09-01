using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by File Manager extensions.
    /// </summary>
    /// <remarks>File Manager extensions implement ways to save and load Threat Models and Knowledge Bases.
    /// The actual Load, Save, Import and Export methods are defined in the specific interfaces
    /// defining Package Managers and Knowledge Base Managers.</remarks>
    public interface IFileManager
    {
        /// <summary>
        /// Supported locations.
        /// </summary>
        LocationType SupportedLocations { get; }

        /// <summary>
        /// Name to use to identify the files supported by the Manager.
        /// </summary>
        string PackageName { get; }

        /// <summary>
        /// Extensions supported by the Manager.
        /// </summary>
        /// <remarks>They include the period (".").</remarks>
        IEnumerable<string> Extensions { get; }

        /// <summary>
        /// Verifies if the File Manager can handle the location identified with the arguments.
        /// </summary>
        /// <param name="locationType">Type of location.</param>
        /// <param name="location">Location where the file is to be found.</param>
        /// <returns>True if the location can be handled by the File Manager, false otherwise.</returns>
        bool CanHandle(LocationType locationType, string location);
    }
}