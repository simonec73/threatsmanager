namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Interface used to describe objects that should be initialized before use.
    /// </summary>
    /// <remarks>It is typically used with Aspect InitializationRequired.
    /// <para>It requires PostSharp to be used.</para></remarks>
    public interface IInitializableObject
    {
        /// <summary>
        /// Property returning true if the object has been initialized correctly.
        /// </summary>
        bool IsInitialized { get; }
    }
}
