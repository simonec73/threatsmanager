using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ProcessList
{
    public partial class ProcessListPanel
    {
        private ContextMenuStrip _contextMenu;

        public Scope SupportedScopes => Scope.Process;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menu = new MenuDefinition(actions, SupportedScopes);
            _contextMenu = menu.CreateMenu();
            menu.MenuClicked += OnMenuClicked;
        }

        private void OnMenuClicked(IContextAwareAction action, object context)
        {
            if (context is IProcess process)
                action.Execute(process);
        }
    }
}