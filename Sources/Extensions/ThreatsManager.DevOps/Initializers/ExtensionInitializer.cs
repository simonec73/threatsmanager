using ThreatsManager.DevOps.Review;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Initializers
{
    [Extension("F3380EE8-43D2-4BB4-B7E5-95CEEC105FDF", "DevOps Extension Initializer", 10, ExecutionMode.Business)]
    public class ExtensionInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(DevOpsInfo));
            KnownTypesBinder.AddKnownType(typeof(DevOpsConnectionInfo));
            KnownTypesBinder.AddKnownType(typeof(DevOpsWorkItemConnectionInfo));
            KnownTypesBinder.AddKnownType(typeof(DevOpsConnection));
            KnownTypesBinder.AddKnownType(typeof(DevOpsWorkItemStateMapping));
            KnownTypesBinder.AddKnownType(typeof(DevOpsFieldMapping));
            KnownTypesBinder.AddKnownType(typeof(DevOpsItemInfo));
            KnownTypesBinder.AddKnownType(typeof(ReviewInfo));
            KnownTypesBinder.AddKnownType(typeof(Iteration));
            KnownTypesBinder.AddKnownType(typeof(Iterations));
            KnownTypesBinder.AddKnownType(typeof(IterationRisk));
            KnownTypesBinder.AddKnownType(typeof(IterationInfo));
        }
    }
}
