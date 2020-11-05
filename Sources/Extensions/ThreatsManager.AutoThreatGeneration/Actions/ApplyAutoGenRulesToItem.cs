using System;
using System.ComponentModel.Composition;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Export(typeof(IContextAwareAction))]
    [ExportMetadata("Id", "FC9151DF-D604-4B88-A529-6418495FA5C8")]
    [ExportMetadata("Label", "Apply Auto Gen Rules to Item Context Aware Action")]
    [ExportMetadata("Priority", 35)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class ApplyAutoGenRulesToItem : IIdentityContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity | Scope.DataFlow;
        public string Label => "Apply Auto Gen Rules";
        public string Group => "ItemActions";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool Execute(IIdentity identity)
        {
            if (identity is IEntity entity)
            {
                if (entity.GenerateThreatEvents())
                    ShowMessage?.Invoke("Threat Events generated successfully.");
                else
                {
                    ShowWarning?.Invoke("No Threat Event or Mitigation has been generated.");
                }

            }
            else if (identity is IDataFlow flow)
            {
                if (flow.GenerateThreatEvents())
                    ShowMessage?.Invoke("Threat Events generated successfully.");
                else
                {
                    ShowWarning?.Invoke("No Threat Event or Mitigation has been generated.");
                }
            }

            return true;
        }
    }
}