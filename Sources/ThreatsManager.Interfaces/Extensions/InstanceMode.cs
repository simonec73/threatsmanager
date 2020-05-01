namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Behavior of the Instances for the Panel Factory.
    /// </summary>
    public enum InstanceMode
    {
        /// <summary>
        /// The Panel Factory creates multiple instances.
        /// </summary>
        Multiple,

        /// <summary>
        /// The Panel Factory should be used to create a single instance.
        /// </summary>
        Single,

        /// <summary>
        /// The Panel Factory should be used to create a single instance specialized per the object it is working on.
        /// </summary>
        SinglePerObject
    }
}