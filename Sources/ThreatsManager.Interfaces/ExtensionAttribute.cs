using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute used to identify Extensions and provide relevant information.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExtensionAttribute : ExportAttribute, IExtensionMetadata
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier of the Extension. It is a Guid serialized as string.</param>
        /// <param name="label">Label characterizing the Extension.</param>
        /// <param name="priority">Priority of the Extension. The lower it is, the higher the Priority.</param>
        /// <param name="executionMode">Required Execution Mode, to determine the visibility.</param>
        /// <param name="parameters">Optional list of parameters for the Extension configuration.</param>
        public ExtensionAttribute(string id, string label, 
            int priority, ExecutionMode executionMode, string[] parameters = null) : base(typeof(IExtension))
        {
            Id = id;
            Label = label;
            Priority = priority;
            Mode = executionMode;
            Parameters = parameters;
        }

        /// <summary>
        /// Identifier of the Extension.
        /// </summary>
        /// <remarks>It is a Guid serialized as string.</remarks>
        public string Id { get; private set; }

        /// <summary>
        /// Label characterizing the Extension.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Priority of the Extension.
        /// </summary>
        /// <remarks>The lower it is, the higher the Priority.</remarks>
        public int Priority { get; private set; }

        /// <summary>
        /// Optional list of parameters for the Extension configuration.
        /// </summary>
        public IEnumerable<string> Parameters { get; private set; }

        /// <summary>
        /// Required Execution Mode, to determine the visibility.
        /// </summary>
        public ExecutionMode Mode { get; private set; }
    }
}