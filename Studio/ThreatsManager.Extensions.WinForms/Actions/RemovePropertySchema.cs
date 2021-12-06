using System;
using System.Drawing;
using System.Linq;
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
    [Extension("1A089EB7-6E69-44F7-8501-21780094F8C6", "Remove Property Schema Context Aware Action", 51, ExecutionMode.Expert)]
    public class RemovePropertySchema : IIdentityContextAwareAction, IThreatTypeMitigationContextAwareAction,
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
        public string Label => "Remove Property Schema";
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
                var dialog = new RemoveSchemaDialog();
                dialog.Initialize(container);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                {
                    var schema = dialog.SelectedSchema;
                    if (schema != null && MessageBox.Show($"You are about to remove Schema '{schema.ToString()}' from the selected object. Do you confirm?", 
                        "Remove Schema", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var properties = container.Properties?
                            .Where(x => (x.PropertyType?.SchemaId ?? Guid.Empty) == schema.Id)
                            .ToArray();

                        if (properties?.Any() ?? false)
                        {
                            foreach (var property in properties)
                            {
                                container.RemoveProperty(property.PropertyTypeId);
                            }

                            result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}
