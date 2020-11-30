using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Interface implemented by factories used to create IDevOpsConnector objects.
    /// </summary>
    [ExtensionDescription("DevOps Connector Factory")]
    public interface IDevOpsConnectorFactory : IExtension
    {
        /// <summary>
        /// Create an instance of IDevOpsConnector.
        /// </summary>
        /// <returns>New instance of IDevOpsConnector.</returns>
        IDevOpsConnector Create();
    }
}