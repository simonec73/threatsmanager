using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("B1EB3B54-0BFB-473A-A86D-FF35CF9D455A", "Copy a Threat Event Context Aware Action", 10, ExecutionMode.Simplified)]
    public class CopyThreatEvent : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Copy a Threat Event";
        public string Group => "Copy&Paste";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            return (item is IThreatEvent threatEvent) && Execute(threatEvent);
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            if (identity is IThreatEvent threatEvent)
            {
                ThreatEventsInfo info = new ThreatEventsInfo
                {
                    ThreatEvents = new List<IThreatEvent>() {threatEvent},
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

            return true;
        }
    }
}