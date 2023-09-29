using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public partial class ThreatEventListPanel
    {
        private ContextMenuStrip _threatEventMenu;
        private ContextMenuStrip _threatTypeMenu;
        private ContextMenuStrip _scenarioMenu;
        private ContextMenuStrip _threatEventMitigationMenu;
        private ContextMenuStrip _vulnerabilityMenu;
        private ContextMenuStrip _vulnerabilityMitigationMenu;
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.ThreatEvent | Scope.ThreatType | 
            Scope.ThreatEventScenario | Scope.ThreatEventMitigation |
            Scope.Vulnerability | Scope.VulnerabilityMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuThreatEvents = new MenuDefinition(actions, Scope.ThreatEvent);
            _threatEventMenu = menuThreatEvents.CreateMenu();
            menuThreatEvents.MenuClicked += OnThreatEventMenuClicked;

            var menuThreatTypes = new MenuDefinition(actions, Scope.ThreatType);
            _threatTypeMenu = menuThreatTypes.CreateMenu();
            menuThreatTypes.MenuClicked += OnThreatTypeMenuClicked;

            var menuScenario = new MenuDefinition(actions, Scope.ThreatEventScenario);
            _scenarioMenu = menuScenario.CreateMenu();
            menuScenario.MenuClicked += OnScenarioMenuClicked;
 
            var menuThreatEventMitigation = new MenuDefinition(actions, Scope.ThreatEventMitigation);
            _threatEventMitigationMenu = menuThreatEventMitigation.CreateMenu();
            menuThreatEventMitigation.MenuClicked += OnThreatEventMitigationMenuClicked;

            var menuVulnerability = new MenuDefinition(actions, Scope.Vulnerability);
            _vulnerabilityMenu = menuVulnerability.CreateMenu();
            menuVulnerability.MenuClicked += OnVulnerabilityMenuClicked;

            var menuVulnerabilityMitigation = new MenuDefinition(actions, Scope.VulnerabilityMitigation);
            _vulnerabilityMitigationMenu = menuVulnerabilityMitigation.CreateMenu();
            menuVulnerabilityMitigation.MenuClicked += OnVulnerabilityMitigationMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("ThreatEventList"))
                {
                    var commandsBar = commandsBarContextAwareAction.CommandsBar;
                    if (commandsBar != null)
                    {
                        if (_commandsBarContextAwareActions == null)
                            _commandsBarContextAwareActions = new Dictionary<string, List<ICommandsBarDefinition>>();
                        List<ICommandsBarDefinition> list;
                        if (_commandsBarContextAwareActions.ContainsKey(commandsBar.Name))
                            list = _commandsBarContextAwareActions[commandsBar.Name];
                        else
                        {
                            list = new List<ICommandsBarDefinition>();
                            _commandsBarContextAwareActions.Add(commandsBar.Name, list);
                        }

                        list.Add(commandsBar);
                    }
                }
            }
        }

        private void OnThreatEventMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatEvent threatEvent)
                action.Execute(threatEvent);
        }

        private void OnThreatTypeMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatType threatType)
                action.Execute(threatType);
        }

        private void OnScenarioMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatEventScenario scenario)
                action.Execute(scenario);
        }

        private void OnThreatEventMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatEventMitigation mitigation)
                action.Execute(mitigation);
        }

        private void OnVulnerabilityMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IVulnerability vulnerability)
                action.Execute(vulnerability);
        }

        private void OnVulnerabilityMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IVulnerabilityMitigation vulnerabilityMitigation)
                action.Execute(vulnerabilityMitigation);
        }
    }
}