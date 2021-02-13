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

namespace ThreatsManager.AutoThreatGeneration.Panels.ThreatTypeList
{
    public partial class AutoThreatTypeListPanel
    {
        private ContextMenuStrip _threatTypeMenu;
        private ContextMenuStrip _threatTypeMitigationMenu;

        public Scope SupportedScopes => Scope.ThreatType | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuThreatTypes = new MenuDefinition(actions, Scope.ThreatType);
            _threatTypeMenu = menuThreatTypes.CreateMenu();
            menuThreatTypes.MenuClicked += OnThreatTypeMenuClicked;

            var menuThreatTypeMitigation = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _threatTypeMitigationMenu = menuThreatTypeMitigation.CreateMenu();
            menuThreatTypeMitigation.MenuClicked += OnThreatTypeMitigationMenuClicked;
        }

        private void OnThreatTypeMenuClicked(Point point, IContextAwareAction action)
        {
            var selectedRow = GetRow(_grid.PointToClient(point));
            if (selectedRow != null && selectedRow.Tag is IThreatType threatType)
                action.Execute(threatType);
        }

        private void OnThreatTypeMitigationMenuClicked(Point point, IContextAwareAction action)
        {
            var selectedRow = GetRow(_grid.PointToClient(point));
            if (selectedRow != null && selectedRow.Tag is IThreatTypeMitigation mitigation)
                action.Execute(mitigation);
        }
    }
}