using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.Validator;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsManageIterationsDialog : Form
    {
        #region Nested classes.
        class DevOpsDateTimePicker : GridDateTimePickerEditControl
        {
            public DevOpsDateTimePicker()
            {
                ShowCheckBox = true;
            }

            public override string Text
            {
                get => EditorCell.Value == null ? string.Empty : ((DateTime)Value).ToShortDateString();
                set => base.Text = value;
            }

            public override bool BeginEdit(bool selectAll)
            {
                base.BeginEdit(selectAll);

                if (EditorCell.Value == null || (DateTime?)EditorCell.Value == MinDate)
                {
                    if (Checked == true)
                        Checked = false;
                }
                else
                {
                    if (Checked == false)
                        Checked = true;
                }

                return (false);
            }

            public override bool EndEdit()
            {
                if (EditorCell.Value != null)
                {
                    if (Checked == false || (DateTime?)EditorCell.Value == MinDate)
                        EditorCell.Value = null;
                }

                return base.EndEdit();
            }
        }

        class DateRange
        {
            public DateRange(DateTime start, DateTime end)
            {
                Start = start;
                End = end;
            }

            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public bool IsInRange(DateTime date)
            {
                return (date > Start && date < End);
            }

            public bool IsInRange(DateRange range)
            {
                return (Start > range.Start && Start < range.End);
            }
        }
        #endregion

        private readonly IThreatModel _model;
        private readonly DevOpsConfigPropertySchemaManager _schemaManager;
        private bool _loading;

        public DevOpsManageIterationsDialog()
        {
            InitializeComponent();
            InitializeGrid();
        }

        public DevOpsManageIterationsDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;
            _schemaManager = new DevOpsConfigPropertySchemaManager(model);

            try
            {
                _loading = true;
                var iterations = _schemaManager.GetIterations()?.ToArray();
                if (iterations?.Any() ?? false)
                {
                    foreach (var iteration in iterations)
                    {
                        AddIteration(iteration);
                    }
                }
                else
                {
                    _initialize.Enabled = true;
                }

                var connector = DevOpsManager.GetConnector(model);
                _load.Enabled = connector != null;
            }
            finally
            {
                _loading = false;
            }
        }

        private void InitializeGrid()
        {
            var grid = _grid.PrimaryGrid;

            grid.Name = "Iterations";
            grid.AllowRowDelete = false;
            grid.AllowRowInsert = false;
            grid.AllowRowResize = false;
            grid.ShowRowDirtyMarker = false;
            grid.ShowTreeButtons = false;
            grid.ShowTreeLines = false;
            grid.ShowRowHeaders = false;
            grid.InitialSelection = RelativeSelection.None;

            grid.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Name",
                Width = 400,
                DataType = typeof(string),
                AllowEdit = true
            });

            grid.Columns.Add(new GridColumn("Start")
            {
                HeaderText = "Start Date",
                DataType = typeof(DateTime),
                AllowEdit = true,
                EditorType = typeof(DevOpsDateTimePicker),
                Width = 120
            });

            grid.Columns.Add(new GridColumn("End")
            {
                HeaderText = "End Date",
                DataType = typeof(DateTime),
                AllowEdit = true,
                EditorType = typeof(DevOpsDateTimePicker),
                Width = 120
            });
        }

        private void _add_Click(object sender, EventArgs e)
        {
            var iteration = new Iteration()
            {
                Id = Guid.NewGuid().ToString()
            };

            if (CalculateNextIteration(out var start, out var end))
            {
                iteration.Start = start;
                iteration.End = end;
            }

            AddIteration(iteration);
            SaveIterations();
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            var selected = _grid.GetSelectedCells()?.OfType<GridCell>().Select(x => x.GridRow).Distinct().ToArray();
            if (selected?.Any() ?? false)
            {
                if (MessageBox.Show("You are about to remove one or more Iterations.\nThis will not allow to understand when some Mitigations have been identified.\nAre you sure?", 
                    "Remove Iterations", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    foreach (var row in selected)
                        _grid.PrimaryGrid.Rows.Remove(row);

                    SaveIterations();
                }
            }
        }

        private void _clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to clear the configured Iterations.\nThis will not allow to understand when Mitigations have been identified.\nAre you sure?", 
                "Clear Iterations", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                _grid.PrimaryGrid.Rows.Clear();
                SaveIterations();
                _initialize.Enabled = true;
            }
        }
        
        private void _initialize_Click(object sender, EventArgs e)
        {
            _initializeGroup.Visible = !_initializeGroup.Visible;
        }

        private async void _load_Click(object sender, EventArgs e)
        {
            var connector = DevOpsManager.GetConnector(_model);
            if (connector != null)
            {
                var iterations = (await connector.GetIterationsAsync())?.ToArray();
                if (iterations?.Any() ?? false)
                {
                    try
                    {
                        _loading = true;
                        foreach (var iteration in iterations)
                        {
                            AddIteration(iteration);
                        }
                        SaveIterations();
                    }
                    finally
                    {
                        _loading = false;
                    }

                    _initialize.Enabled = false;
                }
            }
        }

        private void AddIteration([NotNull] Iteration iteration)
        {
            if (!IterationExist(iteration))
            {
                var row = new GridRow(iteration.Name, iteration.Start, iteration.End)
                {
                    Tag = iteration
                };
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged += OnPropertyChanged;
                _grid.PrimaryGrid.Rows.Add(row);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = sender as GridCell;
            var propertyName = e.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    if (row.Tag is Iteration iteration)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                iteration.Name = (string) cell.Value;
                                break;
                            case "Url":
                                iteration.Url = (string) cell.Value;
                                break;
                            case "Start":
                                iteration.Start = (DateTime?) cell.Value;
                                break;
                            case "End":
                                iteration.End = (DateTime?) cell.Value;
                                break;
                        }
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private bool IterationExist([NotNull] Iteration iteration)
        {
            return GetIterations().Any(x => string.CompareOrdinal(x.Id, iteration.Id) == 0);
        }

        private IEnumerable<Iteration> GetIterations()
        {
            return _grid.PrimaryGrid.Rows.Select(x => x.Tag).OfType<Iteration>();
        }

        private void SaveIterations()
        {
            _schemaManager.SetIterations(GetIterations());
        }

        private void _close_Click(object sender, EventArgs e)
        {
            _validatorMessageContainer.Visible = false;
            if (_validator.Validate(_grid))
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.None;
        }

        private void _startEndValidator_ValidateValue(object sender, DevComponents.DotNetBar.Validator.ValidateValueEventArgs e)
        {
            if (e.ControlToValidate == _grid && sender is CustomValidator validator)
            {
                e.IsValid = true;

                var iterations = GetIterations()?.ToArray();
                if (iterations?.Any() ?? false)
                {
                    foreach (var iteration in iterations)
                    {
                        if ((iteration.Start ?? DateTime.MinValue) > (iteration.End ?? DateTime.MaxValue))
                        {
                            e.IsValid = false;
                            _validatorMessage.Text = validator.ErrorMessage;
                            _validatorMessageContainer.Visible = true;
                            break;
                        }
                    }

                }
            }
        }

        private void _overlapValidator_ValidateValue(object sender, DevComponents.DotNetBar.Validator.ValidateValueEventArgs e)
        {
            if (e.ControlToValidate == _grid && sender is CustomValidator validator)
            {
                e.IsValid = true;

                var iterations = GetIterations()?.ToArray();
                if (iterations?.Any() ?? false)
                {
                    List<DateRange> ranges = new List<DateRange>();

                    foreach (var iteration in iterations)
                    {
                        if (iteration.Start.HasValue || iteration.End.HasValue)
                        {
                            var range = new DateRange(iteration.Start ?? DateTime.MinValue, iteration.End ?? DateTime.MaxValue);
                            if (!IsInAnyRange(range, ranges))
                            {
                                ranges.Add(range);
                            }
                            else
                            {
                                e.IsValid = false;
                                _validatorMessage.Text = validator.ErrorMessage;
                                _validatorMessageContainer.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool IsInAnyRange(DateRange range, List<DateRange> ranges)
        {
            bool result = false;

            if (ranges.Any())
            {
                foreach (var curr in ranges)
                {
                    if (curr.IsInRange(range) || range.IsInRange(curr))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private void _generate_Click(object sender, EventArgs e)
        {
            GenerateIterations(_paramStartDate.Value.Date, (int) _paramCount.Value, (int) _paramDuration.Value, 
                _paramMonday.Checked, _paramTuesday.Checked, _paramWednesday.Checked, _paramThursday.Checked, 
                _paramFriday.Checked, _paramSaturday.Checked, _paramSunday.Checked);
            
            _initializeGroup.Visible = false;
            _initialize.Enabled = false;
        }

        private void GenerateIterations(DateTime start, int count, int duration, 
            bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday,
            string prefix = "Iteration")
        {
            DateTime curr;
            DateTime end;
            int days;
            bool started;
            for (int i = 0; i < count; i++)
            {
                days = 0;
                curr = start;
                end = start;
                started = false;

                do
                {
                    switch (curr.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            if (sunday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Monday:
                            if (monday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Tuesday:
                            if (tuesday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Wednesday:
                            if (wednesday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Thursday:
                            if (thursday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Friday:
                            if (friday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                        case DayOfWeek.Saturday:
                            if (saturday)
                            {
                                if (!started)
                                {
                                    start = curr;
                                    started = true;
                                }
                                end = curr;
                                days++;
                            }
                            break;
                    }

                    curr = curr.AddDays(1.0);
                } while (days < duration);
                    
                var iteration = new Iteration()
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = $"{prefix} {i + 1}",
                    Start = start,
                    End = end
                };
                AddIteration(iteration);

                start = end.AddDays(1.0);
            }

            SaveIterations();
        }

        private bool CalculateNextIteration(out DateTime start, out DateTime end)
        {
            bool result = false;

            start = DateTime.Today;
            end = DateTime.Today;

            var iterations = _schemaManager.GetIterations()?.ToArray();
            if ((iterations?.Length ?? 0) > 1)
            {
                var daysofweek = iterations
                    .Where(x => x.Start.HasValue)
                    .Select(x => x.Start.Value.DayOfWeek);
                if (daysofweek.Any())
                {
                    var dayofweek = daysofweek.First();
                    if (daysofweek.All(x => x == dayofweek))
                    {
                        if (iterations.Any(x => x.End.HasValue))
                        {
                            start = iterations.Where(x => x.End.HasValue).Max(x => x.End.Value);

                            do
                            {
                                start = start.AddDays(1.0);
                            } while (start.DayOfWeek != dayofweek);

                            var duration = CalculateDuration(iterations);
                            if (duration > 0)
                            {
                                end = start.AddDays(duration);
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private int CalculateDuration([NotNull] IEnumerable<Iteration> iterations)
        {
            int result = 0;

            var durations = iterations
                .Where(x => x.Start.HasValue && x.End.HasValue)
                .Select(x => (x.End.Value - x.Start.Value).Days);

            if (durations.Any())
            {
                var duration = durations.First();
                if (durations.All(x => x == duration))
                {
                    result = duration;
                }
            }

            return result;
        }
    }
}
