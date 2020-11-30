using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Initializers
{
    [Extension("30530FE5-9EC2-40DD-9DFF-9482096E51C6", "Standard Actors Initializer", 10, ExecutionMode.Simplified)]
    public class ActorsInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var values = EnumExtensions.GetUIVisible<DefaultActor>()?.ToArray();
            if (values?.Any() ?? false)
            {
                foreach (var value in values)
                {
                    model.AddThreatActor(value);
                }
            }
        }
    }
}