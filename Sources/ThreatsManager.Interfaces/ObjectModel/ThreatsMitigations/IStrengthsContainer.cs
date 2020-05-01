using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by container of Strengths.
    /// </summary>
    public interface IStrengthsContainer
    {
        /// <summary>
        /// Event raised when a Strength is created.
        /// </summary>
        /// <returns>The object which has been created as IStrength.</returns>
        event Action<IStrength> StrengthCreated;

        /// <summary>
        /// Event raised when a Strength is removed.
        /// </summary>
        /// <returns>The object which has been removed as IStrength.</returns>
        event Action<IStrength> StrengthRemoved;

        /// <summary>
        /// Enumeration of Strengths.
        /// </summary>
        IEnumerable<IStrength> Strengths { get; }

        /// <summary>
        /// Get the Strength whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Strength.</param>
        /// <returns>Strength object if found, otherwise null.</returns>
        IStrength GetStrength(int id);

        /// <summary>
        /// Get the first Strength having its id equal of greater than the one passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Strength.</param>
        /// <returns>Strength object if found, otherwise null.</returns>
        IStrength GetMappedStrength(int id);

        /// <summary>
        /// Adds the Strength passed as argument to the container.
        /// </summary>
        /// <param name="strength">Strength to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IStrength strength);

        /// <summary>
        /// Add a custom Strength to the container.
        /// </summary>
        /// <param name="id">Identifier of the Strength.</param>
        /// <param name="name">Name of the Strength.</param>
        /// <returns>New Strength if it is not already included in the system, otherwise null.</returns>
        IStrength AddStrength(int id, string name);

        /// <summary>
        /// Add a standard Strength to the container.
        /// </summary>
        /// <param name="strength">Default Strength to be created.</param>
        /// <returns>New Strength if it is not already included in the system, otherwise null.</returns>
        IStrength AddStrength(DefaultStrength strength);

        /// <summary>
        /// Remove an existing Strength from the Container.
        /// </summary>
        /// <param name="id">Identifier of the Strength to be removed.</param>
        /// <returns>True if the Strength has been found and removed, otherwise false.</returns>
        /// <remarks>If the strength is in use by any associate Mitigation, it cannot be removed.</remarks>
        bool RemoveStrength(int id);

        /// <summary>
        /// Initialization of the Standard Strength.
        /// </summary>
        void InitializeStandardStrengths();
    }
}