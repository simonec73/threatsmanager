using System.Drawing;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("F6A8BC36-2321-4286-BEAE-8FE6FD333046", "Toggle Text Location for Links Context Aware Action", 
        30, ExecutionMode.Simplified)]
    public class ToggleTextLocation : ILinkContextAwareAction
    {
        public Scope Scope => Scope.Link;
        public string Label => "Toggle Text Location";
        public string Group => "Text";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is ILink link)
                result = Execute(link);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(ILink link)
        {
            bool result = false;

            if (link is IThreatModelChild child && child.Model != null)
            {
                var schemaManager = new DiagramPropertySchemaManager(child.Model);
                var propertyType = schemaManager.GetTextLocationPropertyType();
                if (propertyType != null)
                {
                    if ((link.GetProperty(propertyType) ?? link.AddProperty(propertyType, null)) is IPropertyBool
                        propertyBool)
                    {
                        propertyBool.Value = !propertyBool.Value;
                    }
                }
            }

            return result;
        }
    }
}
