﻿using System;
using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("D252EAFC-8F07-4D35-900D-981A96D5779F", "Remove Threat Event Context Aware Action", 50, ExecutionMode.Simplified)]
    public class RemoveThreatEvent : IIdentityContextAwareAction, IDesktopAlertAwareExtension, IAsker
    {
        public Scope Scope => Scope.ThreatEvent;
        public string Label => "Remove Threat Event";
        public string Group => "Object";
        public Bitmap Icon => Resources.threat_event_big_delete;
        public Bitmap SmallIcon => Resources.threat_event_delete;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
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

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IIdentity identity)
        {
            if (identity is IThreatEvent threatEvent && threatEvent.Parent is IIdentity parent)
            {
                Ask?.Invoke(this, threatEvent, "Remove Threat Event",
                    $"You are about to remove Threat Event '{threatEvent.Name}' associated to '{parent.Name}'. Are you sure?",
                    false, RequestOptions.YesNo);
            }

            return true;
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IThreatEvent threatEvent &&
                threatEvent.Parent is IThreatEventsContainer container)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Threat Event"))
                {
                    var result = container.RemoveThreatEvent(threatEvent.Id);
                    if (result)
                    {
                        scope?.Complete();
                        ShowMessage?.Invoke("Remove Threat Event has been executed successfully.");
                    }
                    else
                        ShowWarning?.Invoke("Remove Threat Event has failed.");
                }
            }
        }
    }
}
