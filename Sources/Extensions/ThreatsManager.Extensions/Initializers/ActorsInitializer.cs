using System.ComponentModel.Composition;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Initializers
{
    [Export(typeof(IInitializer))]
    [ExportMetadata("Id", "30530FE5-9EC2-40DD-9DFF-9482096E51C6")]
    [ExportMetadata("Label", "Standard Actors Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
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