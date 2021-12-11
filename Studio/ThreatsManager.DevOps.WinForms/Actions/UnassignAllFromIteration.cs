using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.DevOps.Actions
{
    [Extension("A14CDCC0-0B32-401B-B075-7CF6E2A8D326", "Unassign all Mitigations from Iterations Context Aware Action", 30, ExecutionMode.Management)]
    public class UnassignAllFromIteration : IIdentityContextAwareAction, ICommandsBarContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatModel;
        public string Label => "Unassign all Mitigations from Iterations";
        public string Group => "Iterations";
        public Bitmap Icon => Properties.Resources.iteration_big_delete;
        public Bitmap SmallIcon => Properties.Resources.iteration_delete;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Group, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, true, Shortcut)
            {
                Tag = this
            }
        });

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IThreatModel model)
                result = Execute(model);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IThreatModel model)
            {
                var schemaManager = new DevOpsPropertySchemaManager(model);
                var mitigations = model.Mitigations?
                    .Where(x => schemaManager.GetFirstSeenOn(x) != null)
                    .ToArray();

                if (mitigations?.Any() ?? false)
                {
                    var configSchemaManager = new DevOpsConfigPropertySchemaManager(model);
                    if (MessageBox.Show(
                        $"You are about to unassign {mitigations.Length} mitigations from the assigned iterations." +
                        $"\nDo you confirm?", "Bulk unassignment from Iterations", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            schemaManager.SetFirstSeenOn(mitigation, null);
                        }

                        result = true;
                        ShowMessage?.Invoke("Unassignment from Iterations succeeded.");
                    }
                }
                else
                {
                    ShowMessage?.Invoke(
                        "Nothing to do, because no Mitigation has already been assigned to an Iteration.");
                }
            }

            return result;
        }
    }
}
