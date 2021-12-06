using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Dialogs;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    public partial class Dashboard
    {
        private ContextMenuStrip _externalInteractorMenu;
        private ContextMenuStrip _processMenu;
        private ContextMenuStrip _dataStoreMenu;
        private ContextMenuStrip _flowMenu;
        private ContextMenuStrip _trustBoundaryMenu;
        private ContextMenuStrip _threatModelMenu;
        private ContextMenuStrip _threatEventMenu;
        private ContextMenuStrip _diagramMenu;

        private class MenuDefinitionEx : MenuDefinition
        {
            public event Action<Point> FalsePositiveClicked;

            public MenuDefinitionEx([NotNull] IEnumerable<IContextAwareAction> actions, Scope scope) : base(actions,
                scope)
            {

            }

            public override ContextMenuStrip CreateMenu()
            {
                ContextMenuStrip result = new ContextMenuStrip();

                if (_buckets?.Any() ?? false)
                {
                    result.Items.Add(new ToolStripMenuItem("Set False Positive", null, SetFalsePositive));
                    foreach (var bucket in _buckets)
                    {
                        result.Items.Add(new ToolStripSeparator());
                        AddMenu(result, _actions[bucket]);
                    }
                }

                return result;
            }
            
            private void SetFalsePositive(object sender, EventArgs e)
            {
                if (sender is ToolStripMenuItem menuItem)
                {
                    var point = menuItem.GetCurrentParent().Location;
                    FalsePositiveClicked?.Invoke(point);
                }
            }
        }

        public Scope SupportedScopes => Scope.All;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var menuExternalInteractor = new MenuDefinitionEx(actions, Scope.ExternalInteractor);
            _externalInteractorMenu = menuExternalInteractor.CreateMenu();
            menuExternalInteractor.MenuClicked += OnExternalInteractorMenuClicked;
            menuExternalInteractor.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuProcess = new MenuDefinitionEx(actions, Scope.Process);
            _processMenu = menuProcess.CreateMenu();
            menuProcess.MenuClicked += OnProcessMenuClicked;
            menuProcess.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuDataStore = new MenuDefinitionEx(actions, Scope.DataStore);
            _dataStoreMenu = menuDataStore.CreateMenu();
            menuDataStore.MenuClicked += OnDataStoreMenuClicked;
            menuDataStore.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuDataFlow = new MenuDefinitionEx(actions, Scope.DataFlow);
            _flowMenu = menuDataFlow.CreateMenu();
            menuDataFlow.MenuClicked += OnDataFlowMenuClicked;
            menuDataFlow.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuTrustBoundary = new MenuDefinitionEx(actions, Scope.TrustBoundary);
            _trustBoundaryMenu = menuTrustBoundary.CreateMenu();
            menuTrustBoundary.MenuClicked += OnTrustBoundaryMenuClicked;
            menuTrustBoundary.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuThreatModel = new MenuDefinitionEx(actions, Scope.ThreatModel);
            _threatModelMenu = menuThreatModel.CreateMenu();
            menuThreatModel.MenuClicked += OnThreatModelMenuClicked;
            menuThreatModel.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuThreatEvent = new MenuDefinitionEx(actions, Scope.ThreatEvent);
            _threatEventMenu = menuThreatEvent.CreateMenu();
            menuThreatEvent.MenuClicked += OnThreatEventMenuClicked;
            menuThreatEvent.FalsePositiveClicked += OnFalsePositiveClicked;

            var menuDiagram = new MenuDefinitionEx(actions, Scope.Diagram);
            _diagramMenu = menuDiagram.CreateMenu();
            menuDiagram.MenuClicked += OnDiagramMenuClicked;
            menuDiagram.FalsePositiveClicked += OnFalsePositiveClicked;
        }

        private void OnExternalInteractorMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IExternalInteractor interactor)
                action.Execute(interactor);
        }

        private void OnProcessMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IProcess process)
                action.Execute(process);
        }

        private void OnDataStoreMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IDataStore dataStore)
                action.Execute(dataStore);
        }

        private void OnDataFlowMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IDataFlow dataFlow)
                action.Execute(dataFlow);
        }

        private void OnTrustBoundaryMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is ITrustBoundary trustBoundary)
                action.Execute(trustBoundary);
        }

        private void OnThreatModelMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IThreatModel model)
                action.Execute(model);
        }

        private void OnThreatEventMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IThreatEvent threatEvent)
                action.Execute(threatEvent);
        }

        private void OnDiagramMenuClicked(IContextAwareAction action, object context)
        {
            if (_selectedRow != null && _selectedRow.Tag is IDiagram diagram)
                action.Execute(diagram);
        }

        private void OnFalsePositiveClicked(Point obj)
        {
            if (_selectedRow?.SuperGrid.Parent.Parent is CheckPanel checkPanel &&
                _selectedRow?.Tag is IPropertiesContainer container)
            {
                var qualityAnalyzer = checkPanel.QualityAnalyzerName;
                if (!string.IsNullOrWhiteSpace(qualityAnalyzer))
                {
                    using (var dialog = new FalsePositiveReasonDialog(qualityAnalyzer, container))
                    {
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var schemaManager = new QualityPropertySchemaManager(_model);
                            schemaManager.SetFalsePositive(container, checkPanel.Id, dialog.Reason);

                            Analyze();
                        }
                    }
                }
            }
        }
    }
}