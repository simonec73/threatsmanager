namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface implemented by properties that may override other properties.
    /// </summary>
    /// <remarks>The main example of Shadow Properties, are those implemented by Threat Events, which shadow those specified in the Threat Types.</remarks>
    public interface IShadowProperty
    {
        /// <summary>
        /// Original property which is shadowed by the current one.
        /// </summary>
        IProperty Original { get; }

        /// <summary>
        /// It is true if the original property is overridden by the current property.
        /// </summary>
        bool IsOverridden { get; }

        /// <summary>
        /// Reverts the property to the original value.
        /// </summary>
        void RevertToOriginal();
    }
}