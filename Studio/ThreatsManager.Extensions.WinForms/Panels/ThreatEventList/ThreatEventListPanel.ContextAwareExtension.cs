using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public partial class ThreatEventListPanel
    {
        private ContextMenuStrip _threatEventMenu;
        private ContextMenuStrip _threatTypeMenu;
        private ContextMenuStrip _scenarioMenu;
        private ContextMenuStrip _threatEventMitigationMenu;

        public Scope SupportedScopes => Scope.ThreatEvent | Scope.ThreatType | Scope.ThreatEventScenario | Scope.ThreatEventMitigation;

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
    }
}