using System;
using System.Text;
using DevComponents.DotNetBar;

namespace ThreatsManager
{
    class ThreatModelInUseNotification
    {
        private ThreatModelInUseAction _action = ThreatModelInUseAction.Nothing;

        public ThreatModelInUseAction Show(string owner, string machine, DateTime timestamp, int count)
        {
            var builder = new StringBuilder();
            builder.Append(
                $"The selected Threat Model is already used by <b>{owner}</b> on {machine} and therefore it cannot be opened.");
            builder.Append("<br/>");
            builder.Append($"{count} other users are queued to work on it, after its release.");
            builder.Append("<br/>");
            var interval = DateTime.Now - timestamp;
            if (interval > TimeSpan.FromHours(4))
                builder.Append(
                    $"<b><br/>The lock may be stale</b>, because it has been established more than {(int) interval.TotalHours} hours ago. You may want to contact <b>{owner}</b>.");
            builder.Append("<br/><br/>Please choose how to continue.<br/><br/>");

            var taskDialogInfo = new TaskDialogInfo("Threat Model already in use",
                eTaskDialogIcon.Stop2, "The Threat Model cannot be opened", builder.ToString(),
                eTaskDialogButton.Close, eTaskDialogBackgroundColor.Red);
            var workWithCopyNotifyCommand = new Command()
            {
                Image = null,
                Name = "workWithCopyNotifyCommand",
                Text = "I'll work on a copy and I'll merge when I will notified<br/>that the document has been released."
            };
            workWithCopyNotifyCommand.Executed += Executed;
            var workWithCopyCommand = new Command()
            {
                Image = null,
                Name = "workWithCopyCommand",
                Text = "I'll work on a copy and I'll merge later.<br/>No notification is necessary."
            };
            workWithCopyCommand.Executed += Executed;
            var waitCommand = new Command()
            {
                Image = null,
                Name = "waitCommand",
                Text = "I'll wait for the document to be released.<br/>Please notify me when it occurs."
            };
            waitCommand.Executed += Executed;
            var nothingCommand = new Command()
            {
                Image = null,
                Name = "nothingCommand",
                Text = "No worries, I will try again later.",
                Checked = true
            };
            nothingCommand.Executed += Executed;
            taskDialogInfo.RadioButtons = new []
            {
                workWithCopyNotifyCommand, workWithCopyCommand, waitCommand, nothingCommand
            };

            TaskDialog.Show(taskDialogInfo);

            return _action;
        }

        private void Executed(object sender, EventArgs e)
        {
            if (sender is BaseItem control && control.Command is Command command)
            {
                switch (command.Name)
                {
                    case "workWithCopyNotifyCommand":
                        _action = ThreatModelInUseAction.WorkWithCopyNotify;
                        break;
                    case "workWithCopyCommand":
                        _action = ThreatModelInUseAction.WorkWithCopy;
                        break;
                    case "waitCommand":
                        _action = ThreatModelInUseAction.Notify;
                        break;
                    default:
                        _action = ThreatModelInUseAction.Nothing;
                        break;
                }
            }
        }
    }
}
