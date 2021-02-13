using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("B1E5C7AA-8E8F-4150-BE33-ECBF5F28201D", "Disassociate Diagram Context Aware Action", 42, ExecutionMode.Simplified)]
    public class DisassociateDiagram : IShapeContextAwareAction, IIdentityContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Disassociate Diagram";
        public string Group => "Associate";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute([NotNull] object item)
        {
            return (item is IIdentity identity) && Execute(identity);
        }
        
        public bool IsVisible(object item)
        {
            bool result = false;

            IThreatModel model = null;
            if (item is IThreatModelChild child)
                model = child.Model;
            else if (item is IThreatModel threatModel)
                model = threatModel;

            if (model != null && item is IPropertiesContainer container)
            {
                var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                if (propertyType != null)
                {
                    result = !string.IsNullOrWhiteSpace(container.GetProperty(propertyType)?.StringValue?.Trim('0'));
                }
            }

            return result;
        }

        public bool Execute([NotNull] IShape shape)
        {
            return Execute(shape.Identity);
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IEntity entity && entity.Model is IThreatModel model &&
                MessageBox.Show(Form.ActiveForm, 
                $"You are about to disassociate the associated diagram from {model.GetIdentityTypeName(entity)} '{entity.Name}'. Are you sure?",
                "Disassociate Diagram", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                if (propertyType != null)
                {
                    if (entity.GetProperty(propertyType) is IPropertyIdentityReference property && 
                        property.ValueId != Guid.Empty)
                    {
                        result = true;
                        property.Value = null;
                        DiagramAssociationHelper.NotifyDiagramDisassociation(entity);
                        ShowMessage?.Invoke("Diagram has been disassociated successfully.");
                    }
                    else
                    {
                        ShowWarning?.Invoke("The Entity is not associated to any Diagram.");
                    }
                }
            }

            return result;
        }
    }
}