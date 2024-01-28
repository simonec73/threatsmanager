using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ImportedList
{
    public partial class ImportedListPanel
    {
        private ContextMenuStrip _diagramMenu;
        private ContextMenuStrip _flowMenu;
        private ContextMenuStrip _interactorMenu;
        private ContextMenuStrip _processMenu;
        private ContextMenuStrip _dataStoreMenu;
        private ContextMenuStrip _trustBoundaryMenu;
        private ContextMenuStrip _flowTemplateMenu;
        private ContextMenuStrip _entityTemplateMenu;
        private ContextMenuStrip _trustBoundaryTemplateMenu;
        private ContextMenuStrip _mitigationMenu;
        private ContextMenuStrip _severityMenu;
        private ContextMenuStrip _strengthMenu;
        private ContextMenuStrip _actorMenu;
        private ContextMenuStrip _threatEventMenu;
        private ContextMenuStrip _threatTypeMenu;
        private ContextMenuStrip _threatTypeMitigationMenu;
        private ContextMenuStrip _threatTypeWeaknessMenu;
        private ContextMenuStrip _vulnerabilityMenu;
        private ContextMenuStrip _weaknessMenu;
        private ContextMenuStrip _weaknessMitigationMenu;
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Diagram | Scope.DataFlow | Scope.Entity |
            Scope.TrustBoundary | Scope.FlowTemplate | Scope.EntityTemplate |
            Scope.TrustBoundaryTemplate | Scope.Mitigation | Scope.Severity |
            Scope.Strength | Scope.ThreatActor | Scope.ThreatEvent |
            Scope.ThreatType | Scope.ThreatTypeMitigation | Scope.ThreatTypeWeakness |
            Scope.Vulnerability | Scope.Weakness | Scope.WeaknessMitigation;

        public void SetContextAwareActions(IEnumerable<IContextAwareAction> actions)
        {
            var diagramMenu = new MenuDefinition(actions, Scope.Diagram);
            _diagramMenu = diagramMenu.CreateMenu();
            diagramMenu.MenuClicked += OnDiagramMenuClicked;

            var flowMenu = new MenuDefinition(actions, Scope.DataFlow);
            _flowMenu = flowMenu.CreateMenu();
            flowMenu.MenuClicked += OnFlowMenuClicked;

            var interactorMenu = new MenuDefinition(actions, Scope.ExternalInteractor);
            _interactorMenu = interactorMenu.CreateMenu();
            interactorMenu.MenuClicked += OnInteractorMenuClicked;

            var processMenu = new MenuDefinition(actions, Scope.Process);
            _processMenu = processMenu.CreateMenu();
            processMenu.MenuClicked += OnProcessMenuClicked;

            var dataStoreMenu = new MenuDefinition(actions, Scope.DataStore);
            _dataStoreMenu = dataStoreMenu.CreateMenu();
            dataStoreMenu.MenuClicked += OnDataStoreMenuClicked;

            var trustBoundaryMenu = new MenuDefinition(actions, Scope.TrustBoundary);
            _trustBoundaryMenu = trustBoundaryMenu.CreateMenu();
            trustBoundaryMenu.MenuClicked += OnTrustBoundaryMenuClicked;

            var flowTemplateMenu = new MenuDefinition(actions, Scope.FlowTemplate);
            _flowTemplateMenu = flowTemplateMenu.CreateMenu();
            flowTemplateMenu.MenuClicked += OnFlowTemplateMenuClicked;

            var entityTemplateMenu = new MenuDefinition(actions, Scope.EntityTemplate);
            _entityTemplateMenu = entityTemplateMenu.CreateMenu();
            entityTemplateMenu.MenuClicked += OnEntityTemplateMenuClicked;

            var trustBoundaryTemplateMenu = new MenuDefinition(actions, Scope.TrustBoundaryTemplate);
            _trustBoundaryTemplateMenu = trustBoundaryTemplateMenu.CreateMenu();
            trustBoundaryTemplateMenu.MenuClicked += OnTrustBoundaryTemplateMenuClicked;

            var mitigationMenu = new MenuDefinition(actions, Scope.Mitigation);
            _mitigationMenu = mitigationMenu.CreateMenu();
            mitigationMenu.MenuClicked += OnMitigationMenuClicked;

            var severityMenu = new MenuDefinition(actions, Scope.Severity);
            _severityMenu = severityMenu.CreateMenu();
            severityMenu.MenuClicked += OnSeverityMenuClicked;

            var strengthMenu = new MenuDefinition(actions, Scope.Strength);
            _strengthMenu = strengthMenu.CreateMenu();
            strengthMenu.MenuClicked += OnStrengthMenuClicked;

            var actorMenu = new MenuDefinition(actions, Scope.ThreatActor);
            _actorMenu = actorMenu.CreateMenu();
            actorMenu.MenuClicked += OnActorMenuClicked;

            var threatEventMenu = new MenuDefinition(actions, Scope.ThreatEvent);
            _threatEventMenu = threatEventMenu.CreateMenu();
            threatEventMenu.MenuClicked += OnThreatEventMenuClicked;

            var threatTypeMenu = new MenuDefinition(actions, Scope.ThreatType);
            _threatTypeMenu = threatTypeMenu.CreateMenu();
            threatTypeMenu.MenuClicked += OnThreatTypeMenuClicked;

            var threatTypeMitigationMenu = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _threatTypeMitigationMenu = threatTypeMitigationMenu.CreateMenu();
            threatTypeMitigationMenu.MenuClicked += OnThreatTypeMitigationMenuClicked;

            var threatTypeWeakness = new MenuDefinition(actions, Scope.ThreatTypeWeakness);
            _threatTypeWeaknessMenu = threatTypeWeakness.CreateMenu();
            threatTypeWeakness.MenuClicked += OnThreatTypeWeaknessMenuClicked;

            var vulnerabilityMenu = new MenuDefinition(actions, Scope.Vulnerability);
            _vulnerabilityMenu = vulnerabilityMenu.CreateMenu();
            vulnerabilityMenu.MenuClicked += OnVulnerabilityMenuClicked;

            var weaknessMenu = new MenuDefinition(actions, Scope.Weakness);
            _weaknessMenu = weaknessMenu.CreateMenu();
            weaknessMenu.MenuClicked += OnWeaknessMenuClicked;

            var weaknessMitigation = new MenuDefinition(actions, Scope.WeaknessMitigation);
            _weaknessMitigationMenu = weaknessMitigation.CreateMenu();
            weaknessMitigation.MenuClicked += OnWeaknessMitigationMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("ImportedList"))
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

        private void OnDiagramMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDiagram diagram)
                action.Execute(diagram);
        }

        private void OnFlowMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDataFlow dataFlow)
                action.Execute(dataFlow);
        }

        private void OnInteractorMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IExternalInteractor interactor)
                action.Execute(interactor);
        }

        private void OnProcessMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IProcess process)
                action.Execute(process);
        }

        private void OnDataStoreMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDataStore dataStore)
                action.Execute(dataStore);
        }

        private void OnTrustBoundaryMenuClicked(IContextAwareAction action, object context)
        {
            if (context is ITrustBoundary boundary)
                action.Execute(boundary);
        }

        private void OnFlowTemplateMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IFlowTemplate template)
                action.Execute(template);
        }

        private void OnEntityTemplateMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IEntityTemplate template)
                action.Execute(template);
        }

        private void OnTrustBoundaryTemplateMenuClicked(IContextAwareAction action, object context)
        {
            if (context is ITrustBoundaryTemplate template)
                action.Execute(template);
        }

        private void OnMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IMitigation mitigation)
                action.Execute(mitigation);
        }

        private void OnSeverityMenuClicked(IContextAwareAction action, object context)
        {
            if (context is ISeverity severity)
                action.Execute(severity);
        }

        private void OnStrengthMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IStrength strength)
                action.Execute(strength);
        }

        private void OnActorMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatActor actor)
                action.Execute(actor);
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

        private void OnThreatTypeMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDataFlow dataFlow)
                action.Execute(dataFlow);
        }

        private void OnThreatTypeWeaknessMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatTypeMitigation mitigation)
                action.Execute(mitigation);
        }

        private void OnVulnerabilityMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IVulnerability vulnerability)
                action.Execute(vulnerability);
        }

        private void OnWeaknessMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IWeakness weakness)
                action.Execute(weakness);
        }

        private void OnWeaknessMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IWeaknessMitigation mitigation)
                action.Execute(mitigation);
        }
    }
}