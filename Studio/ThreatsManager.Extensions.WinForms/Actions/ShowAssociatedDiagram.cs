using System;
using System.Drawing;
using PostSharp.Patterns.Contracts;
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
    [Extension("65F44A64-0118-4BFA-874C-2C58CAE1896B", "Show associated Diagram Context Aware Action", 41, ExecutionMode.Business)]
    public class ShowAssociatedDiagram : IShapeContextAwareAction, IIdentityContextAwareAction, 
        IPanelOpenerExtension, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Show Associated Diagram";
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

            if (identity is IEntity entity && entity.Model is IThreatModel model)
            {
                var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                if (propertyType != null)
                {
                    var property = entity.GetProperty(propertyType);
                    if (property is IPropertyIdentityReference identityReference &&
                        identityReference.Value is IDiagram diagram)
                    {
                        result = true;
                        var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
                        if (factory != null)
                            OpenPanel?.Invoke(factory, diagram);
                    }
                }
            }

            if (!result)
                ShowWarning?.Invoke("The Entity is not associated to any Diagram.");

            return result;
        }
    }
}