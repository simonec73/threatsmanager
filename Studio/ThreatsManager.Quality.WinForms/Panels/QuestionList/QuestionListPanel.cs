using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Dialogs;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Panels.QuestionList
{
    public partial class QuestionListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject
    {
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private SelectionRule _filteringRule;
        private QuestionsPropertySchemaManager _schemaManager;

        public QuestionListPanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel model)
        {
            _model = model;
            _schemaManager = new QuestionsPropertySchemaManager(model);

            InitializeGrid();
            LoadModel();
        }
        #endregion

        public bool IsInitialized => _model != null;

        public IActionDefinition ActionDefinition => new ActionDefinition(Id, "QuestionList", "Question List", 
            Properties.Resources.speech_balloon_question_big, Properties.Resources.speech_balloon_question);

        private void InitializeGrid()
        {
            lock (_grid)
            {
                GridPanel panel = _grid.PrimaryGrid;
                panel.ShowTreeButtons = false;
                panel.ShowTreeLines = false;
                panel.AllowRowDelete = false;
                panel.AllowRowInsert = false;
                panel.AllowRowResize = true;
                panel.ShowRowDirtyMarker = false;
                panel.ShowRowHeaders = false;
                panel.InitialActiveRow = RelativeRow.None;

                panel.Columns.Add(new GridColumn("Question")
                {
                    HeaderText = "Question",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                var ddc = panel.Columns["Question"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }
 
                panel.Columns.Add(new GridColumn("AutoGenRule")
                {
                    HeaderText = "Automatic Generation Rule",
                    DataType = typeof(string),
                    Width = 200,
                    EditorType = typeof(GridButtonXEditControl)
                });
                var bc = panel.Columns["AutoGenRule"].EditControl as GridButtonXEditControl;
                if (bc != null)
                {
                    bc.Click += BcButtonClick;
                }
            }
        }

        [InitializationRequired]
        private void BcButtonClick(object sender, EventArgs e)
        {
            if (sender is GridButtonXEditControl bc && bc.EditorCell.GridRow.Tag is Question question)
            {
                using (var dialog = new RuleEditDialog())
                {
                    dialog.Initialize(_model, bc.EditorCell.GridRow.Cells["Question"].FormattedValue);
                    dialog.Rule = question.Rule;

                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        question.Rule = dialog.Rule;
                        var row = bc.EditorCell.GridRow;
                        if (row != null)
                            row.Cells[1].Value = HasSelectionRule(question) ? "Edit Rule" : "Create Rule";
                    }
                }
            }
        }

        private void LoadModel()
        {
            try
            {
                _grid.SuspendLayout();
                _loading = true;
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var questions = _schemaManager?.GetQuestions()?.OrderBy(x => x.Text).ToArray();
                if (questions?.Any() ?? false)
                {
                    var filter = _filter.Text;
                    var filterRule = _filteringRule?.Root.ToString();

                    foreach (var question in questions)
                    {
                        if (IsSelected(question, filter, filterRule))
                        {
                            AddGridRow(question, panel);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private GridRow AddGridRow([NotNull] Question question, [NotNull] GridPanel panel)
        {
            bool rule = HasSelectionRule(question);

            var row = new GridRow(
                question.Text,
                rule ? "Edit Rule" : "Create Rule");
            row.Tag = question;
            panel.Rows.Add(row);
            row.Cells[0].PropertyChanged += OnQuestionCellChanged;

            return row;
        }

        private void OnQuestionCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is Question question)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Question":
                                question.Text = (string) cell.Value;
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

        private GridRow GetRow([NotNull] Question question)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == question)
                {
                    result = row;
                    break;
                }
            }

            return result;
        }

        #region Auxiliary private members.
        private GridRow GetRow(Point position)
        {
            GridRow result = null;

            GridElement item = _grid.GetElementAt(position);

            if (item is GridCell cell)
                result = cell.GridRow;

            return result;
        }

        void DdcButtonClearClick(object sender, CancelEventArgs e)
        {
            GridTextBoxDropDownEditControl ddc =
                sender as GridTextBoxDropDownEditControl;

            if (ddc != null)
            {
                ddc.Text = null;
                e.Cancel = true;
            }
        }

        private bool IsSelected([NotNull] Question item, string filter, string filterRule)
        {
            bool result;

            if (string.IsNullOrWhiteSpace(filter))
                result = true;
            else
            {
                result = !string.IsNullOrWhiteSpace(item.Text) &&
                         item.Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            if (result && _ruleFilter.Checked && !string.IsNullOrWhiteSpace(filterRule))
            {
                result = FilterRule(item.Rule, filterRule);
            }

            return result;
        }

        private bool FilterRule(SelectionRule rule, [Required] string filterRule)
        {
            var result = false;

            if (rule != null)
            {
                var ruleText = rule.ToString();
                if (ruleText.ToLower().Contains(filterRule.ToLower()))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool HasSelectionRule(Question question)
        {
            return question?.Rule?.Root != null;
        }

        private void _filter_ButtonCustomClick(object sender, EventArgs e)
        {
            _filter.Text = null;
        }

        private void _apply_Click(object sender, EventArgs e)
        {
            LoadModel();
        }

        private void _filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                LoadModel();
            } else if (e.KeyChar == (char) Keys.Escape)
            {
                e.Handled = true;
                _filter.Text = string.Empty;
            }
        }
        #endregion

        private void _ruleFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (_ruleFilter.Checked)
            {
                using (var dialog = new RuleFilterDialog())
                {
                    dialog.Initialize(_model);
                    dialog.Rule = _filteringRule;

                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        _filteringRule = dialog.Rule;
                        _ruleFilter.Image = Properties.Resources.checkbox_small;
                    }
                    else
                    {
                        _ruleFilter.Checked = false;
                        _ruleFilter.Image = null;
                    }
                }
            }
            else
            {
                _ruleFilter.Image = null;
            }
        }

        private void _grid_CellActivated(object sender, GridCellActivatedEventArgs e)
        {
            if (!_loading)
            {
                _currentRow = e.NewActiveCell.GridRow;
                ChangeCustomActionStatus?.Invoke("RemoveQuestion", true);
            }
        }

        private void _grid_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!_loading)
            {
                if (e.NewActiveRow is GridRow gridRow)
                {
                    _currentRow = gridRow;
                    ChangeCustomActionStatus?.Invoke("RemoveQuestion", true);
                }
            }
        }
    }
}
