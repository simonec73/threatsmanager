using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to Initialize a newly created Threat Model.
    /// </summary>
    [Extension("$guid1$", "$itemname$ Initializer", 100, ExecutionMode.Simplified)]
    public class $safeitemname$ : IInitializer 
    {
        public void Initialize(IThreatModel model)
        {
            throw new NotImplementedException();
        }
    }
}
