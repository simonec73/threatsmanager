using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;
using ThreatsManager.Extensions.Relationships;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("485D7009-BC4D-4FF2-918C-B65E4B49661D", 
        "Manage Complementary Mitigations Context Aware Action",
        205, ExecutionMode.Simplified)]
    public class ManageComplementaryMitigations : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Mitigation;
        public string Label => "Manage Complementary Mitigations";
        public string Group => "Mitigations";
        public Bitmap Icon => Icons.Resources.mitigations;
        public Bitmap SmallIcon => Icons.Resources.mitigations_small;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
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

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IMitigation mitigation)
            {
                var dialog = new ManageMitigationsRelationshipsDialog(mitigation, AssociationType.Complementary);
                result = dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
            }

            return result;
        }
    }
}
