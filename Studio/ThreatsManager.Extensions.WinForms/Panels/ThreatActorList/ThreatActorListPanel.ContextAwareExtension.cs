﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatActorList
{
    public partial class ThreatActorListPanel
    {
        private ContextMenuStrip _contextMenu;

        public Scope SupportedScopes => Scope.ThreatActor;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menu = new MenuDefinition(actions, SupportedScopes);
            _contextMenu = menu.CreateMenu();
            menu.MenuClicked += OnMenuClicked;
        }

        private void OnMenuClicked(Point point, IContextAwareAction action)
        {
            var selectedRow = GetRow(_grid.PointToClient(point));
            if (selectedRow != null && selectedRow.Tag is IThreatActor actor)
                action.Execute(actor);
        }
    }
}