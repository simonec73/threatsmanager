using System;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Enumeration of the supported locations to store Threat Models.
    /// </summary>
    [Flags]
    public enum LocationType
    {
        /// <summary>
        /// Local File System.
        /// </summary>
        [EnumLabel("Local File System")]
        FileSystem = 1,

        /// <summary>
        /// A Cloud service.
        /// </summary>
        [EnumLabel("A Cloud Service")]
        Cloud = 2
    }
}