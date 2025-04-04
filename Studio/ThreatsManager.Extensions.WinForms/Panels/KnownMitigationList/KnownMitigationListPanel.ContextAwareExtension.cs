﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public partial class KnownMitigationListPanel
    {
        private ContextMenuStrip _mitigationMenu;
        private ContextMenuStrip _threatTypeMitigationMenu;
        private IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _menuMitigation;
        private MenuDefinition _menuThreatTypeMitigation;

        public Scope SupportedScopes => Scope.Mitigation | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _menuMitigation = new MenuDefinition(actions, Scope.Mitigation);
            _mitigationMenu = _menuMitigation.CreateMenu();
            _menuMitigation.MenuClicked += OnMitigationMenuClicked;

            _menuThreatTypeMitigation = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _threatTypeMitigationMenu = _menuThreatTypeMitigation.CreateMenu();
            _menuThreatTypeMitigation.MenuClicked += OnThreatTypeMitigationMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("KnownMitigationList"))
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

        private void OnMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IMitigation mitigation)
                action.Execute(mitigation);
        }

        private void OnThreatTypeMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatTypeMitigation mitigation)
                action.Execute(mitigation);
        }
    }
}