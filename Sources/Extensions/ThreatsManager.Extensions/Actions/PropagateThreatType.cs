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
    [ExportMetadata("Id", "FA566F92-541F-4817-B53B-199C4BA098D6")]
    [ExportMetadata("Label", "Propagate Threat Type Name and Description Context Aware Action")]
    [ExportMetadata("Priority", 30)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class PropagateThreatType : IIdentityContextAwareAction, IAsker
    {
        public Scope Scope => Scope.ThreatType;
        public string Label => "Propagate Threat Type Info";
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
            if (identity is IThreatType threatType)
            {
                Ask?.Invoke(this, threatType, "Copy Threat Type Info", 
                    $"You are about to copy Name and Description of Threat Type '{threatType.Name}' to the associated Threat Events. Are you sure?",
                    false, RequestOptions.YesNo);
            }

            return true;
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IThreatType threatType &&
                threatType.Model is IThreatModel model)
            {
                var threatEvents = model.GetThreatEvents(threatType)?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var t in threatEvents)
                    {
                        t.Name = threatType.Name;
                        t.Description = threatType.Description;
                    }
                }
            }
        }
    }
}
