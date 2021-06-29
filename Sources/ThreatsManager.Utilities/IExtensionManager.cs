using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Interface implemented by ThreatsManager.Engine.Manager in ThreatsManager.Engine, allowing to access Extensions.
    /// </summary>
    public interface IExtensionManager
    {
        /// <summary>
        /// Get the Extensions implementing a specific interface derived from IExtension.
        /// </summary>
        /// <typeparam name="T">Interface that is sought.</typeparam>
        /// <returns>Enumeration of the available Extensions implementing the required interface.</returns>
        IEnumerable<T> GetExtensions<T>() where T : class, IExtension;

        /// <summary>
        /// Get a specific Extension, given its type and Identifier.
        /// </summary>
        /// <typeparam name="T">Interface implemented by the Extension.</typeparam>
        /// <param name="id">Identifier of the Extension.</param>
        /// <returns>Instance of the Extension.</returns>
        T GetExtension<T>(string id) where T : class, IExtension;

        /// <summary>
        /// Get a specific Extension, given its type and label.
        /// </summary>
        /// <typeparam name="T">Interface implemented by the Extension.</typeparam>
        /// <param name="label">Label of the Extension.</param>
        /// <returns>Instance of the Extension.</returns>
        T GetExtensionByLabel<T>(string label) where T : class, IExtension;
    }
}
