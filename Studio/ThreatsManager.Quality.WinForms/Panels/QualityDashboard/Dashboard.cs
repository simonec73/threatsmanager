using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    public partial class Dashboard : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension
    {
        private readonly QualityAnalyzersManager _analyzersManager = new QualityAnalyzersManager();
        private IThreatModel _model;
        private GridRow _selectedRow;

        public Dashboard()
        {
            InitializeComponent();
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel model)
        {
            _model = model;

            _container.SuspendLayout();
            var overall = new OverallPanel();
            overall.Dock = DockStyle.Fill;
            _container.Controls.Add(overall, 0, 0);

            var analyzers = QualityAnalyzersManager.QualityAnalyzers?.ToArray();
            if (analyzers?.Any() ?? false)
            {
                var rows = Math.DivRem(analyzers.Count() + 1, 4, out var remainder);

                if (remainder == 0)
                    _container.RowCount = rows;
                else
                    _container.RowCount = rows + 1;
                
                int row = 0;
                int col = 1;

                foreach (var analyzer in analyzers)
                {
                    var checkPanel = new CheckPanel(analyzer);
                    checkPanel.Dock = DockStyle.Fill;
                    _container.Controls.Add(checkPanel, col, row);
                    checkPanel.ItemSelected += item => _itemEditor.Item = item;
                    checkPanel.ShowMenu += ShowMenu;
                    if (col == 3)
                    {
                        row++;
                        col = 0;
                    }
                    else
                    {
                        col++;
                    }
                }

                for (int i = 0; i < _container.RowStyles.Count; i++)
                {
                    _container.RowStyles[i].SizeType = SizeType.Absolute;
                    _container.RowStyles[i].Height = 300 * Dpi.Factor.Height;
                }
                for (int j = _container.RowStyles.Count; j < _container.RowCount; j++)
                    _container.RowStyles.Add(new RowStyle(SizeType.Absolute, 300  * Dpi.Factor.Height));

                Analyze();
            }
            _container.ResumeLayout();
        }

        private void ShowMenu(GridRow row, Point point, Scope scope)
        {
            switch (scope)
            {
                case Scope.ExternalInteractor:
                    _selectedRow = row;
                    _externalInteractorMenu?.Show(point);
                    break;
                case Scope.Process:
                    _selectedRow = row;
                    _processMenu?.Show(point);
                    break;
                case Scope.DataStore:
                    _selectedRow = row;
                    _dataStoreMenu?.Show(point);
                    break;
                case Scope.DataFlow:
                    _selectedRow = row;
                    _flowMenu?.Show(point);
                    break;
                case Scope.TrustBoundary:
                    _selectedRow = row;
                    _trustBoundaryMenu?.Show(point);
                    break;
                case Scope.ThreatModel:
                    _selectedRow = row;
                    _threatModelMenu?.Show(point);
                    break;
                case Scope.ThreatEvent:
                    _selectedRow = row;
                    _threatEventMenu.Show(point);
                    break;
                case Scope.Diagram:
                    _selectedRow = row;
                    _diagramMenu.Show(point);
                    break;
                default:
                    _selectedRow = null;
                    break;
            }
        }
        #endregion

        public bool IsInitialized => _model != null;

        public IActionDefinition ActionDefinition => new ActionDefinition(Id, "QualityDashboard", "Quality Dashboard",
            Properties.Resources.wax_seal_big, Properties.Resources.wax_seal);

        private void Analyze()
        {
            var healthIndex = _analyzersManager.Analyze(_model, 
                QualityPropertySchemaManager.IsFalsePositive, 
                out var outcomes);

            var controls = _container.Controls.OfType<CheckPanel>().ToArray();
            if (controls.Any())
            {
                foreach (var control in controls)
                {
                    var outcome = outcomes
                        .FirstOrDefault(x => string.CompareOrdinal(x.Id, control.Id) == 0);
                    if (outcome == null)
                    {
                        control.Visible = false;
                    }
                    else
                    {
                        control.Visible = true;
                        control.SetThresholds(outcome.MinRed, outcome.MaxRed, outcome.MinYellow,
                            outcome.MaxYellow, outcome.MinGreen, outcome.MaxGreen);
                        if (outcome.OkByDefinition)
                            control.ShowValue();
                        else
                            control.ShowValue(outcome.Value, outcome.Findings);
                    }
                }
            }

            if (_container.Controls[0] is OverallPanel overallPanel)
            {
                overallPanel.SetHealthIndex(healthIndex);
            }
        }
    }
}
