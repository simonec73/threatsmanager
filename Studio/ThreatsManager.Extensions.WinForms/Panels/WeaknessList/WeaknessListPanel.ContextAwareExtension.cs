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

namespace ThreatsManager.Extensions.Panels.WeaknessList
{
    public partial class WeaknessListPanel
    {
        private ContextMenuStrip _weaknessMenu;
        private ContextMenuStrip _weaknessMitigationMenu;
        private IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _menuWeakness;
        private MenuDefinition _menuWeaknessMitigation;

        public Scope SupportedScopes => Scope.Weakness | Scope.WeaknessMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _menuWeakness = new MenuDefinition(actions, Scope.Weakness);
            _weaknessMenu = _menuWeakness.CreateMenu();
            _menuWeakness.MenuClicked += OnWeaknessMenuClicked;

            _menuWeaknessMitigation = new MenuDefinition(actions, Scope.WeaknessMitigation);
            _weaknessMitigationMenu = _menuWeaknessMitigation.CreateMenu();
            _menuWeaknessMitigation.MenuClicked += OnWeaknessMitigationMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("TrustBoundaryList"))
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