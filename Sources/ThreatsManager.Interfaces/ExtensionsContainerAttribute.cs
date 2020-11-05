using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute to be applied to the Assembly to characterize it is as an Extensions Container. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ExtensionsContainerAttribute : Attribute
    {
        /// <summary>
        /// Constructor to describe dependency with a specific version of the Engine.
        /// </summary>
        /// <param name="version">Version of the Engine.</param>
        public ExtensionsContainerAttribute([Required] string version) : this(version, version, 0)
        {
        }

        /// <summary>
        /// Constructor to describe dependency with a specific version of the Engine.
        /// </summary>
        /// <param name="version">Version of the Engine.</param>
        /// <param name="clientId">Client Identifier.</param>
        public ExtensionsContainerAttribute([Required] string version, uint clientId) : this(version, version, clientId)
        {
        }

        /// <summary>
        /// Constructor to describe dependency with a range of versions of the Engine.
        /// </summary>
        /// <param name="minVersion">Minimum supported versions of the Engine.</param>
        /// <param name="maxVersion">Maximum supported versions of the Engine.</param>
        public ExtensionsContainerAttribute([Required] string minVersion, [Required] string maxVersion) : this(minVersion, maxVersion, 0)
        {
        }

        /// <summary>
        /// Constructor to describe dependency with a range of versions of the Engine.
        /// </summary>
        /// <param name="minVersion">Minimum supported versions of the Engine.</param>
        /// <param name="maxVersion">Maximum supported versions of the Engine.</param>
        /// <param name="clientId">Client Identifier.</param>
        public ExtensionsContainerAttribute([Required] string minVersion, [Required] string maxVersion, uint clientId)
        {
            MinVersion = minVersion;
            MaxVersion = maxVersion;
            ClientId = clientId;
        }

        /// <summary>
        /// Minimum supported version.
        /// </summary>
        public string MinVersion { get; set; }

        /// <summary>
        /// Maximum supported version.
        /// </summary>
        public string MaxVersion { get; set; }

        /// <summary>
        /// Client Identifier.
        /// </summary>
        /// <remarks>If the Client Id is not 0, then the Extension Container requires a special client to be executed.
        /// <para><b>This is not a security feature!</b></para></remarks>
        public uint ClientId { get; set; }

        /// <summary>
        /// Checks if the Extension supports the version of the Engine passed as argument.
        /// </summary>
        /// <param name="engineVersion">Version of the engine to be checked.</param>
        /// <returns>True if the Engine version is supported, otherwise not.</returns>
        public bool IsApplicableExtensionsContainer([NotNull] Version engineVersion)
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(MinVersion) && Version.TryParse(MinVersion, out var minVersion) &&
                !string.IsNullOrWhiteSpace(MaxVersion) && Version.TryParse(MaxVersion, out var maxVersion))
            {
                if (GreaterThan(minVersion, engineVersion) || GreaterThan(engineVersion, maxVersion))
                    result = false;
            }

            return result;
        }

        private bool GreaterThan([NotNull] Version left, [NotNull] Version right)
        {
            bool result = false;

            if (left.Major > right.Major) result = true;
            else if (left.Major == right.Major)
            {
                if (left.Minor > right.Minor) result = true;
                else if (left.Minor == right.Minor)
                {
                    if (left.Build > right.Build) result = true;
                }
            }

            return result;
        }
    }
}