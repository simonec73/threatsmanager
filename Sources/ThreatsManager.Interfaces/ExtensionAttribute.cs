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
        /// <param name="type">Main Type exported by the Extension.</param>
        /// <param name="id">Identifier of the Extension. It is a Guid serialized as string.</param>
        /// <param name="label">Label characterizing the Extension.</param>
        /// <param name="priority">Priority of the Extension. The lower it is, the higher the Priority.</param>
        /// <param name="executionMode">Required Execution Mode, to determine the visibility.</param>
        /// <param name="parameters">Optional list of parameters for the Extension configuration.</param>
        public ExtensionAttribute(Type type, string id, string label, 
            int priority, ExecutionMode executionMode, string[] parameters = null) : base(type)
        {
            Id = id;
            Label = label;
            Priority = priority;
            Mode = executionMode;
            Parameters = parameters;
        }

        public string Id { get; private set; }
        public string Label { get; private set; }
        public int Priority { get; private set; }
        public IEnumerable<string> Parameters { get; private set; }
        public ExecutionMode Mode { get; private set; }
    }
}