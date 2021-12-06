using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoThreatGeneration.Dialogs
{
    public partial class RuleEditDialog : Form, IRuleEditorDialog, IInitializableObject
    {
        private IThreatModel _model;

        public RuleEditDialog()
        {
            InitializeComponent();
        }

        #region Public members.
        public void Initialize([NotNull] IThreatType threatType)
        {
            _model = threatType.Model;
            _threatType.Text = threatType.Name;
            _ruleEditor.Initialize(_model);
        }

        public SelectionRule Rule
        {
            get => _ruleEditor.Rule;
            set => _ruleEditor.Rule = value;
        }

        public bool IsInitialized => _model != null;
        #endregion

        #region Private member functions: other auxiliary functions.
        private void _ok_Click(object sender, EventArgs e)
        {
            if (_ruleEditor.ValidateRule())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Private member functions: testing.
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void _test_Click(object sender, EventArgs e)
        {
            _selectedInteractors.Items.Clear();
            _selectedProcesses.Items.Clear();
            _selectedDataStores.Items.Clear();
            _selectedDataFlows.Items.Clear();
            _modelSelected.Visible = false;

            var rule = Rule;

            if (rule != null && _ruleEditor.ValidateRule())
            {
                var entities = _model?.Entities?.OrderBy(x => x.Name).ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        if (rule.Evaluate(entity))
                        {
                            var builder = new StringBuilder();
                            builder.AppendLine(entity.Name);
                            var diagrams = _model.Diagrams
                                .Where(x => x.Entities?.Any(y => y.AssociatedId == entity.Id) ?? false)
                                .ToArray();
                            if (diagrams?.Any() ?? false)
                            {
                                builder.AppendLine();
                                builder.AppendLine("Diagrams containing the Entity:");
                                foreach (var diagram in diagrams)
                                {
                                    builder.AppendLine($"- {diagram.Name}");
                                }
                            }

                            var item = new ListBoxItem()
                            {
                                Text = entity.ToString(),
                                Image = entity.GetImage(ImageSize.Small),
                                Tooltip = builder.ToString()
                            };

                            if (entity is IExternalInteractor)
                            {
                                _selectedInteractors.Items.Add(item);
                            }
                            else if (entity is IProcess)
                            {
                                _selectedProcesses.Items.Add(item);
                            }
                            else if (entity is IDataStore)
                            {
                                _selectedDataStores.Items.Add(item);
                            }
                        }
                    }
                }

                var dataFlows = _model?.DataFlows?.OrderBy(x => x.Name).ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine(dataFlow.Name);
                        var diagrams = _model.Diagrams
                            .Where(x => x.Links?.Any(y => y.AssociatedId == dataFlow.Id) ?? false)
                            .ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            builder.AppendLine();
                            builder.AppendLine("Diagrams containing the Flow:");
                            foreach (var diagram in diagrams)
                            {
                                builder.AppendLine($"- {diagram.Name}");
                            }
                        }

                        if (rule.Evaluate(dataFlow))
                        {
                            _selectedDataFlows.Items.Add(new ListBoxItem()
                            {
                                Text = dataFlow.ToString(),
                                Image = Resources.flow_small,
                                Tooltip = builder.ToString()
                            });
                        }
                    }
                }

                _modelSelected.Visible = rule.Evaluate(_model);
            }
        }
        #endregion
    }
}
