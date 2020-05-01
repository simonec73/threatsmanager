using System;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface representing an Identity.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Identifier of the Identity.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of the Identity.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the Identity.
        /// </summary>
        string Description { get; set; }
    }
}