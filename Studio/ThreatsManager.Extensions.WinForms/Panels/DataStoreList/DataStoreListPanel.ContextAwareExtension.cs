using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.DataStoreList
{
    public partial class DataStoreListPanel
    {
        private ContextMenuStrip _contextMenu;
        private IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _menu;

        public Scope SupportedScopes => Scope.DataStore;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _menu = new MenuDefinition(actions, SupportedScopes);
            _contextMenu = _menu.CreateMenu();
            _menu.MenuClicked += OnMenuClicked;

            _actions = actions?.ToArray();

            foreach (var action in _actions)
            {
                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction &&
                    commandsBarContextAwareAction.IsVisible("DataStoreList"))
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

        private void OnMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDataStore dataStore)
                action.Execute(dataStore);
        }
    }
}