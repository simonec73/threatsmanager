using System.Collections.Generic;
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

        public Scope SupportedScopes => Scope.Weakness | Scope.WeaknessMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuWeakness = new MenuDefinition(actions, Scope.Weakness);
            _weaknessMenu = menuWeakness.CreateMenu();
            menuWeakness.MenuClicked += OnWeaknessMenuClicked;

            var menuWeaknessMitigation = new MenuDefinition(actions, Scope.WeaknessMitigation);
            _weaknessMitigationMenu = menuWeaknessMitigation.CreateMenu();
            menuWeaknessMitigation.MenuClicked += OnWeaknessMitigationMenuClicked;
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