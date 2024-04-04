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

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public partial class ThreatTypeListPanel
    {
        private ContextMenuStrip _threatTypeMenu;
        private ContextMenuStrip _threatTypeMitigationMenu;
        private IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _menuThreatTypes;
        private MenuDefinition _menuThreatTypeMitigation;
        public Scope SupportedScopes => Scope.ThreatType | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _menuThreatTypes = new MenuDefinition(actions, Scope.ThreatType);
            _threatTypeMenu = _menuThreatTypes.CreateMenu();
            _menuThreatTypes.MenuClicked += OnThreatTypeMenuClicked;

            _menuThreatTypeMitigation = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _threatTypeMitigationMenu = _menuThreatTypeMitigation.CreateMenu();
            _menuThreatTypeMitigation.MenuClicked += OnThreatTypeMitigationMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("ThreatTypeList"))
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

        private void OnThreatTypeMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatType threatType)
                action.Execute(threatType);
        }

        private void OnThreatTypeMitigationMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IThreatTypeMitigation mitigation)
                action.Execute(mitigation);
        }
    }
}