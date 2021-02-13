using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    public partial class CheckPanel : UserControl
    {
        private bool _redToGreen;
        private double _min;
        private double _intermediate1;
        private double _intermediate2;
        private double _max;

        public CheckPanel()
        {
            InitializeComponent();

            _header.Image = Dpi.Factor.Width >= 1.5 ? Properties.Resources.emoticon_smile :
                Properties.Resources.emoticon_smile_small;
        }

        public CheckPanel([NotNull] IQualityAnalyzer analyzer) : this()
        {
            _header.Text = analyzer.Label;
            _header.Tooltip = $"{analyzer.Description}\nFactor: {analyzer.MultiplicationFactor}";
            Id = analyzer.GetExtensionId();
        }

        public event Action<object> ItemSelected;

        public event Action<GridRow, Point, Scope> ShowMenu;

        public string Id { get; private set; }

        public string QualityAnalyzerName => _header.Text;

        public void ShowValue()
        {
            _grid.PrimaryGrid.Rows.Clear();
            _header.Image = Dpi.Factor.Width >= 1.5 ? Properties.Resources.emoticon_smile :
                Properties.Resources.emoticon_smile_small;
            _gauge.LinearScales[0].Pointers[0].Value = _min;
            _gauge.LinearScales[0].Pointers[0].Tooltip = _min.ToString("F1");
        }

        public void ShowValue(double value, IEnumerable<object> items)
        {
            _grid.PrimaryGrid.Rows.Clear();

            _gauge.LinearScales[0].Pointers[0].Tooltip = value.ToString("F1");
            if (value < _min)
                _gauge.LinearScales[0].Pointers[0].Value = _min;
            else if (value > _max)
                _gauge.LinearScales[0].Pointers[0].Value = _max;
            else
                _gauge.LinearScales[0].Pointers[0].Value = value;

            if (_redToGreen)
            {
                if (value <= _intermediate1)
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_frown
                        : Properties.Resources.emoticon_frown_small;
                else if (!Double.IsNaN(_intermediate2) && value <= _intermediate2)
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_straight_face
                        : Properties.Resources.emoticon_straight_face_small;
                else
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_smile
                        : Properties.Resources.emoticon_smile_small;
            }
            else
            {
                if (value < _intermediate1)
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_smile
                        : Properties.Resources.emoticon_smile_small;
                else if (!Double.IsNaN(_intermediate2) && value < _intermediate2)
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_straight_face
                        : Properties.Resources.emoticon_straight_face_small;
                else
                    _header.Image = Dpi.Factor.Width >= 1.5
                        ? Properties.Resources.emoticon_frown
                        : Properties.Resources.emoticon_frown_small;
            }

            var list = items?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    AddItemToList(item);
                }
            }

            _grid.PrimaryGrid.Columns[0].ToolTip = $"Count: {(list?.Length ?? 0)}";
        }

        public Bitmap GetGaugeBitmap()
        {
            var result = new Bitmap(_gauge.Width, _gauge.Height);
            _gauge.DrawToBitmap(result, _gauge.ClientRectangle);

            return result;
        }

        public void SetThresholds(double minRed, double maxRed, double minYellow, double maxYellow, double minGreen, double maxGreen)
        {
            if (minRed < minGreen)
            {
                _redToGreen = true;
                if (Double.IsNaN(minYellow) || double.IsNaN(maxYellow))
                {
                    if (Double.IsNaN(minRed) || double.IsNaN(maxRed))
                    {
                        _min = minGreen;
                        _intermediate1 = double.NaN;
                    }
                    else
                    {
                        _min = minRed;
                        _intermediate1 = (maxRed + minGreen) / 2;
                    }
                    _intermediate2 = Double.NaN;
                }
                else
                {
                    _min = minRed;
                    _intermediate1 = (maxRed + minYellow) / 2;
                    _intermediate2 = (maxYellow + minGreen) / 2;
                }
                _max = maxGreen;
            }
            else
            {
                _redToGreen = false;
                _min = minGreen;
                if (Double.IsNaN(minYellow) || double.IsNaN(maxYellow))
                {
                    if (double.IsNaN(minRed) || double.IsNaN(maxRed))
                    {
                        _intermediate1 = double.NaN;
                        _max = maxGreen;
                    }
                    else
                    {
                        _intermediate1 = (maxGreen + minRed) / 2;
                        _max = maxRed;
                    }
                    _intermediate2 = Double.NaN;
                }
                else
                {
                    _intermediate1 = (maxGreen + minYellow) / 2;
                    _intermediate2 = (maxYellow + minRed) / 2;
                    _max = maxRed;
                }
            }
            _gauge.LinearScales[0].MinValue = _min;
            _gauge.LinearScales[0].MaxValue = _max;

            if (Double.IsNaN(_intermediate1))
                _gauge.LinearScales[0].Sections["Red"].Visible = false;
            else
            {
                _gauge.LinearScales[0].Sections["Red"].Visible = true;
                _gauge.LinearScales[0].Sections["Red"].StartValue = minRed;
                _gauge.LinearScales[0].Sections["Red"].EndValue = maxRed;
                _gauge.LinearScales[0].Sections["Red"].Tooltip = $"From {minRed.ToString("F1")} to {maxRed.ToString("F1")}";
            }
            if (Double.IsNaN(_intermediate2))
                _gauge.LinearScales[0].Sections["Yellow"].Visible = false;
            else
            {
                _gauge.LinearScales[0].Sections["Yellow"].Visible = true;
                _gauge.LinearScales[0].Sections["Yellow"].StartValue = minYellow;
                _gauge.LinearScales[0].Sections["Yellow"].EndValue = maxYellow;
                _gauge.LinearScales[0].Sections["Yellow"].Tooltip = $"From {minYellow.ToString("F1")} to {maxYellow.ToString("F1")}";
            }
            _gauge.LinearScales[0].Sections["Green"].StartValue = minGreen;
            _gauge.LinearScales[0].Sections["Green"].EndValue = maxGreen;
            _gauge.LinearScales[0].Sections["Green"].Tooltip = $"From {minGreen.ToString("F1")} to {maxGreen.ToString("F1")}";
        }

        private void AddItemToList([NotNull] object item)
        {
            var row = new GridRow(item.ToString())
            {
                Tag = item
            };

            if (item is IIdentity identity)
            {
                row.Cells[0].CellStyles.Default.Image = identity.GetImage(ImageSize.Small);
            }

            _grid.PrimaryGrid.Rows.Add(row);
        }

        private void _grid_CellClick(object sender, GridCellClickEventArgs e)
        {
            var row = e.GridCell.GridRow.Tag;
            
            if (row != null)
                ItemSelected?.Invoke(row);
        }

        private void _grid_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                var row = GetRow(e.Location);

                Scope scope;

                if (row?.Tag is IExternalInteractor)
                    scope = Scope.ExternalInteractor;
                else if (row?.Tag is IProcess)
                    scope = Scope.Process;
                else if (row?.Tag is IDataStore)
                    scope = Scope.DataStore;
                else if (row?.Tag is IDataFlow)
                    scope = Scope.DataFlow;
                else if (row?.Tag is ITrustBoundary)
                    scope = Scope.TrustBoundary;
                else if (row?.Tag is IThreatModel)
                    scope = Scope.ThreatModel;
                else if (row?.Tag is IThreatEvent)
                    scope = Scope.ThreatEvent;
                else if (row?.Tag is IDiagram)
                    scope = Scope.Diagram;
                else
                    scope = Scope.Undefined;

                if (scope != Scope.Undefined)
                {
                    ShowMenu?.Invoke(row, _grid.PointToScreen(e.Location), scope);
                }
            }
        }

        private GridRow GetRow(Point position)
        {
            GridRow result = null;

            GridElement item = _grid.GetElementAt(position);

            if (item is GridCell cell)
                result = cell.GridRow;

            return result;
        }
    }
}
