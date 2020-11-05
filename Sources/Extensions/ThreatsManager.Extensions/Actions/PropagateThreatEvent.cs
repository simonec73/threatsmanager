using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Export(typeof(IContextAwareAction))]
    [ExportMetadata("Id", "606CDD2C-8811-47D2-87DE-7BAE808146B3")]
    [ExportMetadata("Label", "Propagate Threat Event Name and Description Context Aware Action")]
    [ExportMetadata("Priority", 30)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class PropagateThreatEvent : IIdentityContextAwareAction, IAsker
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Propagate Threat Event Info";
        public string Group => "Propagate";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        public bool Execute([NotNull] object item)
        {
            bool result = false;

            if (item is IIdentity identity)
            {
                result = Execute(identity);
            }

            return result;
        }

        public bool Execute([NotNull] IIdentity identity)
        {
            if (identity is IThreatEvent threatEvent)
            {
                Ask?.Invoke(this, threatEvent, "Copy Threat Event Info",
                    $"You are about to copy Name and Description of Threat Event '{threatEvent.Name}' to the associated Threat Type and to its sibling Threat Events. Are you sure?",
                    false, RequestOptions.YesNo);
            }

            return true;
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IThreatEvent threatEvent &&
                threatEvent.Model is IThreatModel model)
            {
                var threatType = threatEvent.ThreatType;
                if (threatType != null)
                {
                    threatType.Name = threatEvent.Name;
                    threatType.Description = threatEvent.Description;
                    var threatEvents = model.GetThreatEvents(threatType)?.ToArray();
                    if (threatEvents?.Any() ?? false)
                    {
                        foreach (var t in threatEvents)
                        {
                            if (t != threatEvent)
                            {
                                t.Name = threatEvent.Name;
                                t.Description = threatEvent.Description;
                            }
                        }
                    }
                }
            }
        }
    }
}
