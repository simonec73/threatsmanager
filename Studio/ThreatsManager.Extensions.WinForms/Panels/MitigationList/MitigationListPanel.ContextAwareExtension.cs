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

namespace ThreatsManager.Extensions.Panels.MitigationList
{
    public partial class MitigationListPanel
    {
        private ContextMenuStrip _mitigationMenu;
        private ContextMenuStrip _threatEventMitigationMenu;

        public Scope SupportedScopes => Scope.Mitigation | Scope.ThreatEventMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuMitigation = new MenuDefinition(actions, Scope.Mitigation);
            _mitigationMenu = menuMitigation.CreateMenu();
            menuMitigation.MenuClicked += OnMitigationMenuClicked;

            var menuThreatEventMitigation = new MenuDefinition(actions, Scope.ThreatEventMitigation);
            _threatEventMitigationMenu = menuThreatEventMitigation.CreateMenu();
            menuThreatEventMitigation.MenuClicked += OnThreatEventMitigationMenuClicked;
        }

        private void OnMitigationMenuClicked(Point point, IContextAwareAction action)
        {
            var selectedRow = GetRow(_grid.PointToClient(point));
            if (selectedRow != null && selectedRow.Tag is IMitigation mitigation)
                action.Execute(mitigation);
        }

        private void OnThreatEventMitigationMenuClicked(Point point, IContextAwareAction action)
        {
            var selectedRow = GetRow(_grid.PointToClient(point));
            if (selectedRow != null && selectedRow.Tag is IThreatEventMitigation mitigation)
                action.Execute(mitigation);
        }
    }
}