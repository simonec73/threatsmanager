using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities;
using ExCSS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Icons;

namespace ThreatsManager.Extensions.Panels.ThreatModel
{
    public partial class ThreatModelPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Threat Model";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_commandsBarContextAwareActions?.Any() ?? false)
                {
                    foreach (var definitions in _commandsBarContextAwareActions.Values)
                    {
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                command.Enabled = true;
                                actions.Add(command);
                            }
                        }

                        result.Add(new CommandsBarDefinition(definitions[0].Name, definitions[0].Label, actions));
                    }
                }

                result.Add(new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Refresh", "Refresh List",
                        Resources.refresh_big,
                        Resources.refresh,
                        true, Shortcut.F5)
                }));

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "Refresh":
                        var item = _itemEditor.Item;
                        _itemEditor.Item = null;
                        _itemEditor.Item = item;
                        break;
                    default:
                        if (_itemEditor.Item is IThreatModel model)
                        {
                            if (action.Tag is IIdentityContextAwareAction identityContextAwareAction)
                            {
                                if (identityContextAwareAction.Execute(model))
                                {
                                    text = identityContextAwareAction.Label;
                                    _itemEditor.Item = null;
                                    _itemEditor.Item = model;
                                }
                                else
                                {
                                    text = $"{identityContextAwareAction.Label} failed.";
                                    warning = true;
                                }
                            }
                            else if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                            {
                                if (identitiesContextAwareAction.Execute(new[] { model }))
                                {
                                    text = identitiesContextAwareAction.Label;
                                    _itemEditor.Item = null;
                                    _itemEditor.Item = model;
                                }
                                else
                                {
                                    text = $"{identitiesContextAwareAction.Label} failed.";
                                    warning = true;
                                }
                            }
                            else if (action.Tag is IPropertiesContainersContextAwareAction pcContextAwareAction)
                            {
                                if (pcContextAwareAction.Execute(new[] { model }))
                                {
                                    text = pcContextAwareAction.Label;
                                    _itemEditor.Item = null;
                                    _itemEditor.Item = model;
                                }
                                else
                                {
                                    text = $"{pcContextAwareAction.Label} failed.";
                                    warning = true;
                                }
                            }
                        }
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }
    }
}
