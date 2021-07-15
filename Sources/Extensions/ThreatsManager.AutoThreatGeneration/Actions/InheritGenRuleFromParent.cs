using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using Scope = ThreatsManager.Interfaces.Scope;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    [Extension("B6851BFE-A924-475E-B0B5-B24DD3F78F29", "Inherit Auto Gen Rule from parent Context Aware Action", 55, ExecutionMode.Expert)]
    public class InheritGenRuleFromParent : IThreatTypeMitigationContextAwareAction, IWeaknessMitigationContextAwareAction, 
        IDesktopAlertAwareExtension, IAsker
    {
        private class Context
        {
            public IPropertiesContainer Container { get; set; }
            public SelectionRule Rule { get; set; }
        }

        public Scope Scope => Scope.ThreatTypeMitigation | Scope.WeaknessMitigation;
        public string Label => "Inherit Auto Gen Rule from parent";
        public string Group => "AutoGen";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            var result = false;

            if (item is IThreatTypeMitigation ttm)
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

        public bool Execute(IWeaknessMitigation mitigation)
        {
            var result = false;

            var model = mitigation.Model;
            if (model != null)
            {
                var rule = mitigation.GetRule(model);
                var parent = mitigation.Weakness;

                if (parent != null)
                {
                    var parentRule = parent.GetRule(model);

                    if (string.CompareOrdinal(rule?.ToString(), parentRule?.ToString()) != 0)
                    {
                        Ask?.Invoke(this, new Context() {Container = mitigation, Rule = parentRule},
                            "Inherit Auto Gen Rule from Weakness", $"Are you sure you want to replace the existing rule for '{mitigation.Mitigation.Name}' with the one assigned to Weakness '{parent.Name}'?",
                            true, RequestOptions.YesNo);
                    }
                    else
                    {
                        ShowMessage?.Invoke("Nothing to do, because the Auto Gen Rules are equivalent.");
                    }
                }
            }

            return result;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            var result = false;

            var model = mitigation.Model;
            if (model != null)
            {
                var rule = mitigation.GetRule(model);
                var parent = mitigation.ThreatType;

                if (parent != null)
                {
                    var parentRule = parent.GetRule(model);

                    if (string.CompareOrdinal(rule?.ToString(), parentRule?.ToString()) != 0)
                    {
                        Ask?.Invoke(this, new Context() { Container = mitigation, Rule = parentRule },
                            "Inherit Auto Gen Rule from Threat Type", $"Are you sure you want to replace the existing rule for '{mitigation.Mitigation.Name}' with the one assigned to Threat Type '{parent.Name}'?",
                            true, RequestOptions.YesNo);
                    }
                    else
                    {
                        ShowMessage?.Invoke("Nothing to do, because the Auto Gen Rules are equivalent.");
                    }
                    result = true;
                }
            }

            return result;
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;
        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is Context c)
            {
                c.Container?.SetRule(c.Rule);
                ShowMessage?.Invoke("Auto Gen Rule copied successfully.");
            }
        }
    }
}
