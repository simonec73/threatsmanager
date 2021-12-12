using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public partial class KnownMitigationListPanel
    {
        private ContextMenuStrip _mitigationMenu;
        private ContextMenuStrip _threatTypeMitigationMenu;

        public Scope SupportedScopes => Scope.Mitigation | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuMitigation = new MenuDefinition(actions, Scope.Mitigation);
            _mitigationMenu = menuMitigation.CreateMenu();
            menuMitigation.MenuClicked += OnMitigationMenuClicked;

            var menuThreatTypeMitigation = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _threatTypeMitigationMenu = menuThreatTypeMitigation.CreateMenu();
            menuThreatTypeMitigation.MenuClicked += OnThreatTypeMitigationMenuClicked;
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