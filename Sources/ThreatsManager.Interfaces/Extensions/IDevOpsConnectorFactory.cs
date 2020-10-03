namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by factories used to create IDevOpsConnector objects.
    /// </summary>
    public interface IDevOpsConnectorFactory
    {
        /// <summary>
        /// Create an instance of IDevOpsConnector.
        /// </summary>
        /// <returns>New instance of IDevOpsConnector.</returns>
        IDevOpsConnector Create();
    }
}