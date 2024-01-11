using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface representing an object version
    /// </summary>
    public interface IObjectVersion
    {
        /// <summary>
        /// Identifier of the version.
        /// </summary>
        string VersionId { get; }

        /// <summary>
        /// Author of the version.
        /// </summary>
        string VersionAuthor { get; }
    }
}
