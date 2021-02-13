using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to Initialize the Extension when it is loaded.
    /// </summary>
    /// <remarks>It is typically used to initialize Json deserialization for some object defined in the library.</remarks>
    [Extension("$guid1$", "$itemname$ Extension Initializer", 100, ExecutionMode.Business)]
    public class $safeitemname$ : IExtensionInitializer 
    {
        public void Initialize()
        {
            //KnownTypesBinder.AddKnownType(typeof(SomeJsonSerializableObject));

            throw new NotImplementedException();
        }
    }
}
