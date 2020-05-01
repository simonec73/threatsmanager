using System.Collections.Generic;
using System.ComponentModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface representing the Extensions metadata.
    /// </summary>
    /// <remarks>This interface is not intended to be implemented by any class, but is used automatically by the infrastructure.</remarks>
    public interface IExtensionMetadata
    {
        /// <summary>
        /// Identifier of the Extension.
        /// </summary>
        /// <remarks>It can be any string, provided that it is unique. It is recommended to use GUIDs.</remarks>
        string Id { get; }

        /// <summary>
        /// Name to show for the Extension.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Priority of the Extension.
        /// </summary>
        /// <remarks>The lower the number, the greater the priority.
        /// <para>Standard extensions have an ID in the range 10 to 49.</para></remarks>
        int Priority { get; }

        /// <summary>
        /// Names of the configuration parameters for the Extension.
        /// </summary>
        [DefaultValue(null)]
        IEnumerable<string> Parameters { get; }

        /// <summary>
        /// Attribute used to specify the required Execution Mode for the interface.
        /// </summary>
        ExecutionMode Mode { get; }
    }
}
