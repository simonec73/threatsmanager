namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface defining a Property Schema, that is a container of Properties, associated to a Namespace.
    /// </summary>
    public interface IPropertySchema : IIdentity, IPropertyTypesContainer, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Namespace of the Property Schema.
        /// </summary>
        string Namespace { get; set; }

        /// <summary>
        /// Scope of the Property Schema.
        /// </summary>
        Scope AppliesTo { get; set; }

        /// <summary>
        /// Flag that allow to specify that the Property Schema must be applied automatically.
        /// </summary>
        bool AutoApply { get; set; }

        /// <summary>
        /// Priority of the Property Schema.
        /// </summary>
        /// <remarks>Priority defines the order of processing of the Properties.
        /// <para>The higher the value the lower the priority.</para>
        /// <para>If two Property Schemas have the Name and Namespace, the one with higher priority hides the other.</para></remarks>
        int Priority { get; set; }

        /// <summary>
        /// Flag that allows to hide the entire Property Schema from the UI.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Flag that allows to mark a Property Schema as being managed by the System.
        /// </summary>
        /// <remarks>System Property Schemas cannot be modified by using any interface provided with Threats Manager Studio, but only by using the code.</remarks>
        bool System { get; set; }

        /// <summary>
        /// Flag that allows to mark a Property Schema as not being exportable as a Template. 
        /// </summary>
        /// <remarks>By default all Property Schemas are exportable.</remarks>
        bool NotExportable { get; set; }

        /// <summary>
        /// Execution Mode required to show the Property Schema.
        /// </summary>
        ExecutionMode RequiredExecutionMode { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Property Schema and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Property Schema.</returns>
        IPropertySchema Clone(IPropertySchemasContainer container);
    }
}