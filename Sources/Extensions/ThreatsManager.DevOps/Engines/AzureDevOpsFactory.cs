using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Engines
{
    /// <summary>
    /// Azure DevOps Factory.
    /// </summary>
    /// <remarks>Supports only</remarks>
    [Extension("0EB212AB-EBA8-483D-A76C-E2D31CEFFCE1", "Azure DevOps Connector", 10, ExecutionMode.Management)]
    public class AzureDevOpsFactory : IDevOpsConnectorFactory
    {
        public IDevOpsConnector Create()
        {
            return new AzureDevOps();
        }

        public override string ToString()
        {
            return this.GetExtensionLabel();
        }
    }
}