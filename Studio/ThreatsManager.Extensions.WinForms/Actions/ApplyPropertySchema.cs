using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("EC6584B8-A520-42FC-9FEA-DC409AE79839", "Apply Property Schema Context Aware Action", 50, ExecutionMode.Expert)]
    public class ApplyPropertySchema : IIdentityContextAwareAction, IThreatTypeMitigationContextAwareAction, 
        IThreatEventMitigationContextAwareAction, IWeaknessMitigationContextAwareAction, IVulnerabilityMitigationContextAwareAction
    {
        public bool Execute(object item)
        {
            bool result = false;

            if (item is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public Scope Scope => Scope.PropertyContainers;
        public string Label => "Apply Property Schema";
        public string Group => "ItemActions";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(IVulnerabilityMitigation mitigation)
        {
            bool result = false;

            if (mitigation is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool Execute(IWeaknessMitigation mitigation)
        {
            bool result = false;

            if (mitigation is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool Execute(IThreatEventMitigation mitigation)
        {
            bool result = false;

            if (mitigation is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            bool result = false;

            if (mitigation is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IPropertiesContainer container)
                result = Execute(container);

            return result;
        }

        public bool Execute(IPropertiesContainer container)
        {
            bool result = false;

            if (container != null)
            {
                var dialog = new ApplySchemaDialog();
                dialog.Initialize(container);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                {
                    var schema = dialog.SelectedSchema;
                    if (schema != null)
                    {
                        container.Apply(schema);
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
