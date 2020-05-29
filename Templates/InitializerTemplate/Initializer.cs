using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace $rootnamespace$
{
    [Extension(typeof(IInitializer), "$guid1$", 
        "$itemname$ $safeitemname$", 100, ExecutionMode.Simplified)]
    public class $safeitemname$ : IInitializer 
    {
        public void Initialize(IThreatModel model)
        {
            throw new NotImplementedException();
        }
    }
}
