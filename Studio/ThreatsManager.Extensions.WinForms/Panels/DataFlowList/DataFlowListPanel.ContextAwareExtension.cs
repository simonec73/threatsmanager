using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.DataFlowList
{
    public partial class DataFlowListPanel
    {
        private ContextMenuStrip _contextMenu;

        public Scope SupportedScopes => Scope.DataFlow;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menu = new MenuDefinition(actions, SupportedScopes);
            _contextMenu = menu.CreateMenu();
            menu.MenuClicked += OnMenuClicked;
        }

        private void OnMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IDataFlow dataFlow)
                action.Execute(dataFlow);
        }
    }
}