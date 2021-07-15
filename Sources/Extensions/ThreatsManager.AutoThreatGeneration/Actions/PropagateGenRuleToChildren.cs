using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
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
    [Extension("44FBD9D5-06E9-44B0-92FD-3769FD6C87AC", "Propagate Auto Gen Rule to children Context Aware Action", 55, ExecutionMode.Expert)]
    public class PropagateGenRuleToChildren : IIdentityContextAwareAction, IDesktopAlertAwareExtension, IAsker
    {
        private class Context
        {
            public IEnumerable<IPropertiesContainer> Containers { get; set; }
            public SelectionRule Rule { get; set; }
        }

        public Scope Scope => Scope.ThreatType | Scope.Weakness;
        public string Label => "Propagate Auto Gen Rule to children";
        public string Group => "AutoGen";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            var result = false;

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
            var result = false;

            IEnumerable<IPropertiesContainer> children = null;
            SelectionRule rule = null;
            if (identity is IThreatType threatType)
            {
                children = threatType.Mitigations;
                rule = threatType.GetRule(threatType.Model);
            } else if (identity is IWeakness weakness)
            {
                children = weakness.Mitigations;
                rule = weakness.GetRule(weakness.Model);
            }

            if (children?.Any() ?? false)
            { 
                Ask?.Invoke(this, new Context() { Containers = children, Rule = rule },
                    "Propagate Auto Gen Rule to children", $"Are you sure you want to replace the existing rule to all Mitigations associated to '{identity.Name}' with the rule assigned to the latter?",
                    true, RequestOptions.YesNo);
                result = true;
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
                var containers = c.Containers?.ToArray();
                if (containers?.Any() ?? false)
                {
                    foreach (var container in containers)
                    {
                        container.SetRule(c.Rule);
                    }
                }
                ShowMessage?.Invoke("Auto Gen Rule copied successfully.");
            }
        }
    }
}
