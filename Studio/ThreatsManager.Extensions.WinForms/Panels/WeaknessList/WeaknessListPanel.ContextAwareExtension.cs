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

namespace ThreatsManager.Extensions.Panels.WeaknessList
{
    public partial class WeaknessListPanel
    {
        private ContextMenuStrip _weaknessMenu;
        private ContextMenuStrip _weaknessMitigationMenu;

        public Scope SupportedScopes => Scope.ThreatType | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuThreatTypes = new MenuDefinition(actions, Scope.ThreatType);
            _weaknessMenu = menuThreatTypes.CreateMenu();
            menuThreatTypes.MenuClicked += OnThreatTypeMenuClicked;

            var menuThreatTypeMitigation = new MenuDefinition(actions, Scope.ThreatTypeMitigation);
            _weaknessMitigationMenu = menuThreatTypeMitigation.CreateMenu();
            menuThreatTypeMitigation.MenuClicked += OnThreatTypeMitigationMenuClicked;
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