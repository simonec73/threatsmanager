using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoThreatGeneration.Actions;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoThreatGeneration.Dialogs
{
    public partial class MitigationRuleEditDialog : Form, IRuleEditorDialog, IInitializableObject
    {
        private IThreatModel _model;
        private SelectionRule _threatTypeRule;

        public MitigationRuleEditDialog()
        {
            InitializeComponent();
        }

        #region Public members.
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public bool Initialize([NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            bool result = false;

            _model = threatTypeMitigation.Model;
            _threatType.Text = threatTypeMitigation.ThreatType.Name;
            _mitigation.Text = threatTypeMitigation.Mitigation.Name;

            _strengthOverride.SelectedIndex = 0;
            var strengths = _model.Strengths?.ToArray();
            if (strengths?.Any() ?? false)
                _strengthOverride.Items.AddRange(strengths);

            _statusOverride.SelectedIndex = 0;
            _statusOverride.Items.AddRange(EnumExtensions.GetEnumLabels<MitigationStatus>().ToArray());

            _severityOverride.SelectedIndex = 0;
            var severities = _model.Severities?.Where(x => x.Visible).ToArray();
            if (severities?.Any() ?? false)
                _severityOverride.Items.AddRange(severities);

            _threatTypeRule = threatTypeMitigation.ThreatType.GetRule();
            if (_threatTypeRule != null)
            {
                result = true;
                _ruleEditor.Initialize(_model);
            }

            return result;
        }

        public SelectionRule Rule
        {
            get
            {
                SelectionRule result = _ruleEditor.Rule;

                if (result != null)
                {
                    var mitigationSelectionRule = new MitigationSelectionRule(result);

                    if (_strengthOverride.SelectedItem is IStrength strength)
                        mitigationSelectionRule.StrengthId = strength.Id;
                    else
                        mitigationSelectionRule.StrengthId = null;

                    if (_statusOverride.SelectedItem is string statusText)
                        mitigationSelectionRule.Status = statusText.GetEnumValue<MitigationStatus>();
                    else
                        mitigationSelectionRule.Status = null;

                    if (_severityOverride.SelectedItem is ISeverity severity)
                        mitigationSelectionRule.SeverityId = severity.Id;
                    else
                        mitigationSelectionRule.SeverityId = null;

                    result = mitigationSelectionRule;
                }

                return result;
            }

            set
            {
                _ruleEditor.Rule = value;

                if (value is MitigationSelectionRule rule)
                {
                    _strengthOverride.SelectedIndex = 0;
                    int strengthsCount = _strengthOverride.Items.Count;
                    if (rule.StrengthId.HasValue)
                    {
                        for (int i = 1; i < strengthsCount; i++)
                        {
                            if (_strengthOverride.Items[i] is IStrength current && current.Id == rule.StrengthId)
                            {
                                _strengthOverride.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    _statusOverride.SelectedItem = rule.Status.GetEnumLabel();

                    _severityOverride.SelectedIndex = 0;
                    int severitiesCount = _severityOverride.Items.Count;
                    if (rule.SeverityId.HasValue)
                    {
                        for (int i = 1; i < severitiesCount; i++)
                        {
                            if (_severityOverride.Items[i] is ISeverity current && current.Id == rule.SeverityId)
                            {
                                _severityOverride.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
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

            if (_threatTypeRule != null && rule != null)
            {
                var entities = _model?.Entities?.OrderBy(x => x.Name).ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        if (_threatTypeRule.Evaluate(entity) && rule.Evaluate(entity))
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
                        if (_threatTypeRule.Evaluate(dataFlow) && rule.Evaluate(dataFlow))
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

                            _selectedDataFlows.Items.Add(new ListBoxItem()
                            {
                                Text = dataFlow.ToString(),
                                Image = Icons.Resources.flow_small,
                                Tooltip = builder.ToString()
                            });
                        }
                    }
                }

                _modelSelected.Visible = _threatTypeRule.Evaluate(_model) && rule.Evaluate(_model);
            }
        }
        #endregion

        #region Private member functions: Strength, Status and Severity management.
        private void _statusOverride_SelectedIndexChanged(object sender, EventArgs e)
        {
            _severityOverride.Enabled = CanSeverityBeOverridden();
        }

        private bool CanSeverityBeOverridden()
        {
            bool result = false;

            var currentStatus = _statusOverride.SelectedItem;
            if (currentStatus is string statusText)
            {
                var status = statusText.GetEnumValue<MitigationStatus>();
                result = status == MitigationStatus.Existing || status == MitigationStatus.Implemented;
            }

            return result;
        }
        #endregion
    }
}
