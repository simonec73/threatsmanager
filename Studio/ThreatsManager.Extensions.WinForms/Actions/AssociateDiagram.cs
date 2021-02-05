using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
#pragma warning disable CS0067
    [Extension("8E4A5EC1-A4D7-4BF9-A034-440A6F4717AD", "Associate Diagram Context Aware Action", 40, ExecutionMode.Simplified)]
    public class AssociateDiagram : IShapeContextAwareAction, IIdentityContextAwareAction, IPanelOpenerExtension, 
        IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Associate Diagram";
        public string Group => "Associate";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public event Action<IPanelFactory, IIdentity> OpenPanel;

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
                    result = string.IsNullOrWhiteSpace(container.GetProperty(propertyType)?.StringValue?.Trim('0'));
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

            if (identity is IEntity entity && entity.Model is IThreatModel model)
            {
                using (var dialog = new DiagramSelectionDialog(entity))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var diagram = dialog.Diagram;
                        if (diagram == null)
                        {
                            diagram = model.AddDiagram(dialog.DiagramName);
                        }

                        if (diagram != null)
                        {
                            var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                            var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                            if (propertyType != null)
                            {
                                var property = entity.GetProperty(propertyType);
                                if (property == null)
                                {
                                    property = entity.AddProperty(propertyType, diagram.Id.ToString("N"));
                                }
                                else
                                {
                                    property.StringValue = diagram.Id.ToString("N");
                                }

                                result = true;
                                DiagramAssociationHelper.NotifyDiagramAssociation(entity, diagram);
                                var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
                                if (factory != null)
                                {
                                    OpenPanel?.Invoke(factory, diagram);
                                    ShowMessage?.Invoke("Diagram has been associated successfully.");
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}