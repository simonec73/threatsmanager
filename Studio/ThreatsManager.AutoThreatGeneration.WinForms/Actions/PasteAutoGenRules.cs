using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("67292F9F-30A1-448B-B270-9733D11DA2F3", "Paste Auto Gen Rules Context Aware Action", 101, ExecutionMode.Expert)]
    public class PasteThreatEvents : IIdentityContextAwareAction, IThreatTypeMitigationContextAwareAction, IWeaknessMitigationContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.ThreatType | Scope.ThreatTypeMitigation | Scope.Weakness | Scope.WeaknessMitigation;
        public string Label => "Paste Auto Gen Rules";
        public string Group => "AutoGen";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(IWeaknessMitigation mitigation)
        {
            var result = false;

            if (Clipboard.GetDataObject() is DataObject dataObject &&
                dataObject.GetDataPresent("AutoGenRule") &&
                dataObject.GetData("AutoGenRule") is string autoGenRule)
            {
                result = ApplyGenRule(mitigation, autoGenRule);
            }

            return result;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            var result = false;

            if (Clipboard.GetDataObject() is DataObject dataObject &&
                dataObject.GetDataPresent("AutoGenRule") &&
                dataObject.GetData("AutoGenRule") is string autoGenRule)
            {
                result = ApplyGenRule(mitigation, autoGenRule);
            }

            return result;
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            var result = false;

            if (item is IPropertiesContainer container && 
                Clipboard.GetDataObject() is DataObject dataObject &&
                dataObject.GetDataPresent("AutoGenRule") &&
                dataObject.GetData("AutoGenRule") is string autoGenRule)
            {
                result = ApplyGenRule(container, autoGenRule);
            }

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            var result = false;

            if (identity is IPropertiesContainer container && 
                Clipboard.GetDataObject() is DataObject dataObject &&
                dataObject.GetDataPresent("AutoGenRule") &&
                dataObject.GetData("AutoGenRule") is string autoGenRule)
            {
                result = ApplyGenRule(container, autoGenRule);
            }

            return result;
        }

        private bool ApplyGenRule([NotNull] IPropertiesContainer container, string value)
        {
            var result = false;

            if (container is IThreatModelChild child)
            {
                var propertyType = new AutoGenRulesPropertySchemaManager(child.Model).GetPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType);
                    if (property != null)
                    {
                        property.StringValue = value;
                    }
                    else
                    {
                        container.AddProperty(propertyType, value);
                    }

                    result = true;
                }
            }

            return result;
        }
    }
}