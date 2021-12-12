using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("98D8C378-E73E-4B40-B558-8CA63A887607", "Copy Auto Gen Rules Context Aware Action", 100, ExecutionMode.Expert)]
    public class CopyAutoGenRules : IIdentityContextAwareAction, IThreatTypeMitigationContextAwareAction, IWeaknessMitigationContextAwareAction
    {
        public Scope Scope => Scope.ThreatType | Scope.ThreatTypeMitigation | Scope.Weakness | Scope.WeaknessMitigation;
        public string Label => "Copy Auto Gen Rules";
        public string Group => "AutoGen";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            bool result = false;

            if (item is IIdentity identity)
            {
                result = Execute(identity);
            } else if (item is IThreatTypeMitigation ttm)
            {
                result = Execute(ttm);
            } else if (item is IWeaknessMitigation wm)
            {
                result = Execute(wm);
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

            if (identity is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            return mitigation != null && Execute(mitigation as IPropertiesContainer);
        }

        public bool Execute(IWeaknessMitigation mitigation)
        {
            return mitigation != null && Execute(mitigation as IPropertiesContainer);
        }

        private bool Execute([NotNull] IPropertiesContainer container)
        {
            var result = false;

            if (container is IThreatModelChild child)
            {
                var propertyType = new AutoGenRulesPropertySchemaManager(child.Model).GetPropertyType();
                if (propertyType != null)
                {
                    DataObject dataObject = new DataObject();
                    dataObject.SetData("AutoGenRule", container.GetProperty(propertyType)?.StringValue ?? string.Empty);
                    Clipboard.SetDataObject(dataObject);
                    result = true;
                }
            }

            return result;
        }
}
}