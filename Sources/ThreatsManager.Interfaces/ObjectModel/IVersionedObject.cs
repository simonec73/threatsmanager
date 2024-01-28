using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface implemented by versioned objects
    /// </summary>
    public interface IVersionedObject
    {
        /// <summary>
        /// Identified of the last saved version.
        /// </summary>
        IObjectVersion CurrentVersion { get; }

        /// <summary>
        /// Returns the list of versions, sorted from the most recent to the oldest.
        /// </summary>
        IEnumerable<IObjectVersion> Versions { get; }

        /// <summary>
        /// Adds a version to the object.
        /// </summary>
        void AddVersion();
    }
}
