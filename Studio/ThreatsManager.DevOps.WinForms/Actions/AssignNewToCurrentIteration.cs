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
    [Extension("410C2F55-D3C8-4CCA-8BB5-CF1DB9115FDB", "Assign new Mitigations to the current Iteration Context Aware Action", 20, ExecutionMode.Management)]
    public class AssignNewToCurrentIteration : IIdentityContextAwareAction, ICommandsBarContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatModel;
        public string Label => "Assign new Mitigations to current Iteration";
        public string Group => "Iterations";
        public Bitmap Icon => Properties.Resources.iteration_big;
        public Bitmap SmallIcon => Properties.Resources.iteration;
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
                    .Where(x => schemaManager.GetFirstSeenOn(x) == null)
                    .ToArray();

                if (mitigations?.Any() ?? false)
                {
                    var configSchemaManager = new DevOpsConfigPropertySchemaManager(model);
                    var iteration = configSchemaManager.CurrentIteration;
                    if (iteration != null)
                    {
                        if (MessageBox.Show(
                            $"You are about to assign {mitigations.Length} mitigations to the current iteration ('{iteration.Name}')." +
                            $"\nDo you confirm?", "Bulk assignment to Iteration", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                schemaManager.SetFirstSeenOn(mitigation, iteration);
                            }

                            result = true;
                            ShowMessage?.Invoke("Assignment to Iteration succeeded.");
                        }
                    }
                    else
                    {
                        ShowWarning?.Invoke("No current Iteration is defined.");
                    }
                }
                else
                {
                    ShowMessage?.Invoke(
                        "Nothing to do, because all Mitigations have already been assigned to an Iteration.");
                }
            }

            return result;
        }
    }
}
