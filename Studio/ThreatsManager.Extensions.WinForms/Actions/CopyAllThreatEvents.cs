using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("6E33D6FF-0381-4F03-8B67-EFC99EC7845B", "Copy All Threat Events Context Aware Action", 20, ExecutionMode.Simplified)]
    public class CopyAllThreatEvents : IShapeContextAwareAction, IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Entity | Scope.Group | Scope.DataFlow;
        public string Label => "Copy All Threat Events";
        public string Group => "Copy&Paste";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            return (item is IIdentity identity) && Execute(identity);
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IShape shape)
        {
            return Execute(shape.Identity);
        }

        public bool Execute(IIdentity identity)
        {
            if (identity is IThreatEventsContainer container)
            {
                var threatEvents = container.ThreatEvents?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    ThreatEventsInfo info = new ThreatEventsInfo
                    {
                        ThreatEvents = new List<IThreatEvent>(threatEvents),
                    };
                    var serialized = JsonConvert.SerializeObject(info, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                            TypeNameHandling = TypeNameHandling.All
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                        });

                    DataObject dataObject = new DataObject();
                    dataObject.SetData("ThreatEventsInfo", serialized);
                    Clipboard.SetDataObject(dataObject);
                }
            }

            return true;
        }
    }
}