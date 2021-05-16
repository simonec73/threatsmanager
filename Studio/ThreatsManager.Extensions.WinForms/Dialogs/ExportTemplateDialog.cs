using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ExportTemplateDialog : Form, IInitializableObject
    {
        private readonly IThreatModel _model;
        private static readonly IEnumerable<IPropertySchemasExtractor> _extractors;

        static ExportTemplateDialog()
        {
            _extractors = ExtensionUtils.GetExtensions<IPropertySchemasExtractor>();
        }

        public ExportTemplateDialog()
        {
            InitializeComponent();
            if (Dpi.Factor.Height >= 2)
                _wizard.HeaderImage = Properties.Resources.astrologer_huge;

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            AddSpellCheck(_name);
            AddSpellCheck(_description);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public ExportTemplateDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;

            _name.Text = model.Name;
            _description.Text = model.Description;

            var schemas = _model.Schemas?
                .Where(x => !x.NotExportable)
                .OrderBy(x => x.Name).ToArray();
            if (schemas?.Any() ?? false)
            {
                _schemas.Items.AddRange(schemas);
            }
            else
            {
                _fullySchemas.Enabled = false;
                _fullySchemas.CheckState = CheckState.Indeterminate;
            }

            var entityTemplates = _model.EntityTemplates?.OrderBy(x => x.Name).ToArray();
            if (entityTemplates?.Any() ?? false)
            {
                _itemTemplates.Items.AddRange(entityTemplates);
            }

            var flowTemplates = _model.FlowTemplates?.OrderBy(x => x.Name).ToArray();
            if (flowTemplates?.Any() ?? false)
            {
                _itemTemplates.Items.AddRange(flowTemplates);
            }

            var trustBoundaryTemplates = _model.TrustBoundaryTemplates?.OrderBy(x => x.Name).ToArray();
            if (trustBoundaryTemplates?.Any() ?? false)
            {
                _itemTemplates.Items.AddRange(trustBoundaryTemplates);
            }

            if (_itemTemplates.Items.Count == 0)
            {
                _fullyItemTemplates.Enabled = false;
                _fullyItemTemplates.CheckState = CheckState.Indeterminate;
            }

            var threatActors = _model.ThreatActors?.OrderBy(x => x.Name).ToArray();
            if (threatActors?.Any() ?? false)
            {
                _threatActors.Items.AddRange(threatActors);
            }
            else
            {
                _fullyThreatActors.Enabled = false;
                _fullyThreatActors.CheckState = CheckState.Indeterminate;
            }

            var threatTypes = _model.ThreatTypes?.OrderBy(x => x.Name).ToArray();
            if (threatTypes?.Any() ?? false)
            {
                _threatTypes.Items.AddRange(threatTypes);
            }
            else
            {
                _fullyThreatTypes.Enabled = false;
                _fullyThreatTypes.CheckState = CheckState.Indeterminate;
            }

            var mitigations = _model.Mitigations?.OrderBy(x => x.Name).ToArray();
            if (mitigations?.Any() ?? false)
            {
                _mitigations.Items.AddRange(mitigations);
            }
            else
            {
                _fullyMitigations.Enabled = false;
                _fullyMitigations.CheckState = CheckState.Indeterminate;
            }
        }

        public bool IsInitialized => _model != null;

        #region Buttons management.
        private void _checkAllSchemas_Click(object sender, EventArgs e)
        {
            CheckAll(_schemas);
            _pageSchemas.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _uncheckAllSchemas_Click(object sender, EventArgs e)
        {
            UncheckAll(_schemas);
            _pageSchemas.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _checkAllItemTemplates_Click(object sender, EventArgs e)
        {
            CheckAll(_itemTemplates);
            _pageItemTemplates.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _uncheckAllItemTemplates_Click(object sender, EventArgs e)
        {
            UncheckAll(_itemTemplates);
            _pageItemTemplates.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _checkAllThreatActors_Click(object sender, EventArgs e)
        {
            CheckAll(_threatActors);
            _pageThreatActors.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _uncheckAllThreatActors_Click(object sender, EventArgs e)
        {
            UncheckAll(_threatActors);
            _pageThreatActors.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _checkAllThreatTypes_Click(object sender, EventArgs e)
        {
            CheckAll(_threatTypes);
            _pageThreatTypes.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _uncheckAllThreatTypes_Click(object sender, EventArgs e)
        {
            UncheckAll(_threatTypes);
            _pageThreatTypes.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _checkAllMitigations_Click(object sender, EventArgs e)
        {
            CheckAll(_mitigations);
            _pageMitigations.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _uncheckAllMitigations_Click(object sender, EventArgs e)
        {
            UncheckAll(_mitigations);
            _pageMitigations.NextButtonEnabled = AnythingSelected(true) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void CheckAll([NotNull] CheckedListBox control)
        {
            ChangeCheckAllStatus(control, true);
        }

        private void UncheckAll([NotNull] CheckedListBox control)
        {
            ChangeCheckAllStatus(control, false);
        }

        private void ChangeCheckAllStatus([NotNull] CheckedListBox control, bool checkedStatus)
        {
            int count = control.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    control.SetItemChecked(i, checkedStatus);
                }
            }
        }

        private void _browse_Click(object sender, EventArgs e)
        {
            if (_saveFile.ShowDialog(ActiveForm) == DialogResult.OK)
            {
                _fileName.Text = _saveFile.FileName;
                _pageFile.NextButtonEnabled = eWizardButtonState.Auto;
            }
        }

        private void _wizard_FinishButtonClick(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var definition = GetDuplicationDefinition();

            _model.SaveTemplate(definition, _name.Text, _description.Text, _fileName.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void _wizard_CancelButtonClick(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        #region Perform duplication.
        private DuplicationDefinition GetDuplicationDefinition()
        {
            DuplicationDefinition result = new DuplicationDefinition();
            List<Guid> schemas = new List<Guid>();
            List<Guid> mitigations = new List<Guid>();
            List<int> severities = new List<int>();
            List<int> strengths = new List<int>();

            AddThreatActors(result, schemas);
            AddThreatTypes(result, schemas, mitigations, severities, strengths);
            AddMitigations(result, mitigations, schemas);
            AddItemTemplates(result, schemas);
            AddStrengths(result, strengths, schemas);
            AddSeverities(result, severities, schemas);
            AddPropertySchemas(result, schemas);

            return result;
        }

        private void AddThreatActors([NotNull] DuplicationDefinition definition, [NotNull] List<Guid> schemas)
        {
            if (_fullyThreatActors.Checked)
            {
                definition.AllThreatActors = true;
                AddSchemas(schemas, _model.ThreatActors?.Select(x => x.Properties));
            }
            else
            {
                var threatActors = _threatActors.CheckedItems.OfType<IThreatActor>().ToArray();
                if (threatActors.Any())
                {
                    definition.ThreatActors = threatActors.Select(x => x.Id).ToArray();
                    AddSchemas(schemas, threatActors.Select(x => x.Properties));
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void AddStrengths([NotNull] DuplicationDefinition definition,
            [NotNull] List<int> strengths, [NotNull] List<Guid> schemas)
        {
            definition.Strengths = strengths;
            AddSchemas(schemas, _model.Strengths?
                .Where(x => strengths?.Contains(x.Id) ?? false)
                .Select(x => x.Properties));
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void AddSeverities([NotNull] DuplicationDefinition definition,
            [NotNull] List<int> severities, [NotNull] List<Guid> schemas)
        {
            definition.Severities = severities;
            AddSchemas(schemas, _model.Severities?
                .Where(x => severities?.Contains(x.Id) ?? false)
                .Select(x => x.Properties));
        }

        private void AddThreatTypes([NotNull] DuplicationDefinition definition, [NotNull] List<Guid> schemas,
            [NotNull] List<Guid> mitigations, [NotNull] List<int> severities, [NotNull] List<int> strengths)
        {
            if (_fullyThreatTypes.Checked)
            {
                definition.AllThreatTypes = true;
                AddMitigations(mitigations, _model.ThreatTypes);
                AddSeverities(severities, _model.ThreatTypes);
                AddStrengths(strengths, _model.ThreatTypes);
                AddSchemas(schemas, _model.ThreatTypes?.Select(x => x.Properties));
            }
            else
            {
                var threatTypes = _threatTypes.CheckedItems.OfType<IThreatType>().ToArray();
                if (threatTypes.Any())
                {
                    definition.ThreatTypes = threatTypes.Select(x => x.Id).ToArray();

                    AddMitigations(mitigations, threatTypes);
                    AddSeverities(severities, threatTypes);
                    AddStrengths(strengths, threatTypes);
                    AddSchemas(schemas, threatTypes.Select(x => x.Properties));
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void AddMitigations([NotNull] DuplicationDefinition definition, 
            [NotNull] List<Guid> mitigations, [NotNull] List<Guid> schemas)
        {
            if (_fullyMitigations.Checked)
            {
                definition.AllMitigations = true;
                AddSchemas(schemas, _model.Mitigations?.Select(x => x.Properties));
            }
            else
            {
                var m = _mitigations.CheckedItems.OfType<IMitigation>().Select(x => x.Id).ToArray();
                IEnumerable<Guid> mitigationIds = null;
                if (m.Any())
                {
                    if (mitigations.Any())
                        mitigationIds = mitigations.Union(m).ToArray();
                    else
                        mitigationIds = m;
                }
                else
                {
                    if (mitigations.Any())
                        mitigationIds = mitigations;
                }

                definition.Mitigations = mitigationIds;
                AddSchemas(schemas, _model.Mitigations
                    .Where(x => mitigationIds?.Contains(x.Id) ?? false)
                    .Select(x => x.Properties));
            }
        }

        private void AddItemTemplates([NotNull] DuplicationDefinition definition, [NotNull] List<Guid> schemas)
        {
            if (_fullyItemTemplates.Checked)
            {
                definition.AllEntityTemplates = true;
                definition.AllFlowTemplates = true;
                definition.AllTrustBoundaryTemplates = true;
                AddSchemas(schemas, _model.EntityTemplates?.Select(x => x.Properties));
                AddSchemas(schemas, _model.FlowTemplates?.Select(x => x.Properties));
                AddSchemas(schemas, _model.TrustBoundaryTemplates?.Select(x => x.Properties));
            }
            else
            {
                var entityTemplates = _itemTemplates.CheckedItems.OfType<IEntityTemplate>().ToArray();
                if (entityTemplates.Any())
                {
                    definition.EntityTemplates = entityTemplates.Select(x => x.Id).ToArray();
                    AddSchemas(schemas, entityTemplates.Select(x => x.Properties));
                }
                var flowTemplates = _itemTemplates.CheckedItems.OfType<IFlowTemplate>().ToArray();
                if (flowTemplates.Any())
                {
                    definition.FlowTemplates = flowTemplates.Select(x => x.Id).ToArray();
                    AddSchemas(schemas, flowTemplates.Select(x => x.Properties));
                }

                var trustBoundaryTemplates = _itemTemplates.CheckedItems.OfType<ITrustBoundaryTemplate>().ToArray();
                if (trustBoundaryTemplates.Any())
                {
                    definition.TrustBoundaryTemplates = trustBoundaryTemplates.Select(x => x.Id).ToArray();
                    AddSchemas(schemas, trustBoundaryTemplates.Select(x => x.Properties));
                }
            }
        }

        private void AddPropertySchemas([NotNull] DuplicationDefinition definition, [NotNull] List<Guid> schemas)
        {
            IEnumerable<Guid> additional;
            if (_fullySchemas.Checked)
            {
                additional = _model.Schemas?.Where(x => !x.NotExportable).Select(x => x.Id).ToArray();
            }
            else
            {
                additional = _schemas.CheckedItems.OfType<IPropertySchema>().Select(x => x.Id).ToArray();
            }

            definition.PropertySchemas = additional == null ? schemas : schemas.Union(additional);
        }

        private void AddMitigations(List<Guid> mitigations, IEnumerable<IThreatType> threatTypes)
        {
            var tt = threatTypes?.ToArray();
            if (tt?.Any() ?? false)
            {
                foreach (var threatType in tt)
                {
                    var mm = threatType.Mitigations?.ToArray();
                    if (mm?.Any() ?? false)
                    {
                        foreach (var mitigation in mm)
                        {
                            if (!mitigations.Contains(mitigation.MitigationId))
                            {
                                mitigations.Add(mitigation.MitigationId);
                            }
                        }
                    }
                }
            }
        }

        private void AddSeverities([NotNull] List<int> severities, IEnumerable<IThreatType> threatTypes)
        {
            var tt = threatTypes?.ToArray();
            if (tt?.Any() ?? false)
            {
                var sevs = tt.Select(x => x.SeverityId);
                foreach (var sev in sevs)
                {
                    if (!severities.Contains(sev))
                        severities.Add(sev);
                }
            }
        }

        private void AddStrengths(List<int> strengths, IEnumerable<IThreatType> threatTypes)
        {
            var tt = threatTypes?.ToArray();
            if (tt?.Any() ?? false)
            {
                foreach (var threatType in tt)
                {
                    var mitigations = threatType.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            if (mitigation.Strength != null && !strengths.Contains(mitigation.StrengthId))
                            {
                                strengths.Add(mitigation.StrengthId);
                            }
                        }
                    }
                }
            }
        }

        private void AddSchemas([NotNull] List<Guid> schemas, IEnumerable<IEnumerable<IProperty>> listProps)
        {
            var listPropsArray = listProps?.ToArray();
            if (listPropsArray?.Any() ?? false)
            {
                foreach (var listProp in listPropsArray)
                {
                    var props = listProp?.ToArray();
                    if (props?.Any() ?? false)
                    {
                        foreach (var prop in props)
                        {
                            var propertyType = _model.GetPropertyType(prop.PropertyTypeId);
                            if (propertyType != null)
                            {
                                if (!schemas.Contains(propertyType.SchemaId) && !(_model.GetSchema(propertyType.SchemaId)?.NotExportable ?? true))
                                    schemas.Add(propertyType.SchemaId);

                                if ((_extractors?.Any() ?? false) && 
                                    prop is IPropertyJsonSerializableObject jsonProperty)
                                {
                                    foreach (var extractor in _extractors)
                                    {
                                        var extractedSchemas = extractor.GetPropertySchemas(jsonProperty)?.ToArray();
                                        if (extractedSchemas?.Any() ?? false)
                                        {
                                            foreach (var schema in extractedSchemas)
                                            {
                                                if (!schema.NotExportable && !schemas.Contains(schema.Id))
                                                    schemas.Add(schema.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Page management.
        private void _wizard_WizardPageChanging(object sender, WizardCancelPageChangeEventArgs e)
        {
            if (e.PageChangeSource == eWizardPageChangeSource.NextButton)
                e.NewPage = GetNextPage(e.OldPage);
            else if (e.PageChangeSource == eWizardPageChangeSource.BackButton)
                e.NewPage = GetPreviousPage(e.OldPage);
        }

        private WizardPage GetNextPage([NotNull] WizardPage current)
        {
            WizardPage result = null;

            if (current == _pageIntro)
                result = _pageTMProperties;
            else if (current == _pageTMProperties)
            {
                _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                    eWizardButtonState.True : eWizardButtonState.False;
                result = _pageFullyInclude;
            }
            else if (current == _pageFullyInclude)
            {
                if (_skipGranularSteps.Checked)
                    result = _pageFile;
                else
                {
                    result = !_fullySchemas.Checked ? _pageSchemas : GetNextPage(_pageSchemas);
                    if (GetNextPage(result) == _pageFile)
                        result.NextButtonEnabled = AnythingSelected(true) ?
                            eWizardButtonState.True : eWizardButtonState.False;
                }
            } else if (current == _pageSchemas)
            {
                result = !_fullyItemTemplates.Checked ? _pageItemTemplates : GetNextPage(_pageItemTemplates);
                if (GetNextPage(result) == _pageFile)
                    result.NextButtonEnabled =
                        AnythingSelected(true) ? eWizardButtonState.True : eWizardButtonState.False;
                else
                    result.NextButtonEnabled = eWizardButtonState.True;
            } else if (current == _pageItemTemplates)
            {
                result = !_fullyThreatTypes.Checked ? _pageThreatTypes : GetNextPage(_pageThreatTypes);
                if (GetNextPage(result) == _pageFile)
                    result.NextButtonEnabled = AnythingSelected(true) ?
                        eWizardButtonState.True : eWizardButtonState.False;
                else
                    result.NextButtonEnabled = eWizardButtonState.True;
            } else if (current == _pageThreatTypes)
            {
                result = !_fullyMitigations.Checked ? _pageMitigations : GetNextPage(_pageMitigations);
                if (GetNextPage(result) == _pageFile)
                    result.NextButtonEnabled = AnythingSelected(true) ?
                        eWizardButtonState.True : eWizardButtonState.False;
                else
                    result.NextButtonEnabled = eWizardButtonState.True;
            } else if (current == _pageMitigations)
            {
                result = !_fullyThreatActors.Checked ? _pageThreatActors : GetNextPage(_pageThreatActors);
                if (GetNextPage(result) == _pageFile)
                    result.NextButtonEnabled = AnythingSelected(true) ?
                        eWizardButtonState.True : eWizardButtonState.False;
                else
                    result.NextButtonEnabled = eWizardButtonState.True;
            } else if (current == _pageThreatActors)
            {
                result = _pageFile;
            } else if (current == _pageFile)
            {
                result = _pageFinish;
            }

            return result;
        }

        private WizardPage GetPreviousPage([NotNull] WizardPage current)
        {
            WizardPage result = null;

            if (current == _pageTMProperties)
                result = _pageIntro;
            else if (current == _pageFullyInclude)
            {
                result = _pageTMProperties;
            } else if (current == _pageSchemas)
            {
                _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                    eWizardButtonState.True : eWizardButtonState.False;
                result = _pageFullyInclude;
            } else if (current == _pageItemTemplates)
            {
                result = !_fullySchemas.Checked ? _pageSchemas : GetPreviousPage(_pageSchemas);
            } else if (current == _pageThreatTypes)
            {
                result = !_fullyItemTemplates.Checked ? _pageItemTemplates : GetPreviousPage(_pageItemTemplates);
            } else if (current == _pageMitigations)
            {
                result = !_fullyThreatTypes.Checked ? _pageThreatTypes : GetPreviousPage(_pageThreatTypes);
            } else if (current == _pageThreatActors)
            {
                result = !_fullyMitigations.Checked ? _pageMitigations : GetPreviousPage(_pageMitigations);
            } else if (current == _pageFile)
            {
                if (_skipGranularSteps.Checked)
                    result = _pageFullyInclude;
                else
                    result = !_fullyThreatActors.Checked ? _pageThreatActors : GetPreviousPage(_pageThreatActors);
            } else if (current == _pageFinish)
            {
                result = _pageFile;
            }

            return result;
        }

        private void _fullySchemas_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _fullyItemTemplates_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _fullyThreatTypes_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _fullyMitigations_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _fullyThreatActors_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _skipGranularSteps_CheckedChanged(object sender, EventArgs e)
        {
            _pageFullyInclude.NextButtonEnabled = AnythingSelected(false) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private bool AnythingSelected(bool full)
        {
            return (!full && !_skipGranularSteps.Checked) ||
                   _fullySchemas.CheckState == CheckState.Checked ||
                   _fullyItemTemplates.CheckState == CheckState.Checked ||
                   _fullyThreatTypes.CheckState == CheckState.Checked ||
                   _fullyMitigations.CheckState == CheckState.Checked || 
                   _fullyThreatActors.CheckState == CheckState.Checked || 
                   _schemas.CheckedItems.Count > 0 || _itemTemplates.CheckedItems.Count > 0 ||
                   _threatTypes.CheckedItems.Count > 0 || _mitigations.CheckedItems.Count > 0 ||
                   _threatActors.CheckedItems.Count > 0;
        }

        private void _schemas_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pageSchemas.NextButtonEnabled = (_wizard.GetNextPage() != _pageFinish ||
                                              _fullySchemas.CheckState == CheckState.Checked ||
                                              _fullyItemTemplates.CheckState == CheckState.Checked ||
                                              _fullyThreatTypes.CheckState == CheckState.Checked ||
                                              _fullyMitigations.CheckState == CheckState.Checked || 
                                              _fullyThreatActors.CheckState == CheckState.Checked || 
                                              (e.NewValue == CheckState.Checked || _schemas.CheckedItems.Count > 1) || 
                                              _itemTemplates.CheckedItems.Count > 0 ||
                                              _threatTypes.CheckedItems.Count > 0 || _mitigations.CheckedItems.Count > 0 ||
                                              _threatActors.CheckedItems.Count > 0) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _entityTemplates_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pageItemTemplates.NextButtonEnabled = (_wizard.GetNextPage() != _pageFinish ||
                                                      _fullySchemas.CheckState == CheckState.Checked ||
                                                      _fullyItemTemplates.CheckState == CheckState.Checked ||
                                                      _fullyThreatTypes.CheckState == CheckState.Checked ||
                                                      _fullyMitigations.CheckState == CheckState.Checked || 
                                                      _fullyThreatActors.CheckState == CheckState.Checked || 
                                                      _schemas.CheckedItems.Count > 0 || 
                                                      (e.NewValue == CheckState.Checked || _itemTemplates.CheckedItems.Count > 1) ||
                                                      _threatTypes.CheckedItems.Count > 0 || _mitigations.CheckedItems.Count > 0 ||
                                                      _threatActors.CheckedItems.Count > 0) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _threatActors_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pageThreatActors.NextButtonEnabled = (_wizard.GetNextPage() != _pageFinish ||
                                                   _fullySchemas.CheckState == CheckState.Checked ||
                                                   _fullyItemTemplates.CheckState == CheckState.Checked ||
                                                   _fullyThreatTypes.CheckState == CheckState.Checked ||
                                                   _fullyMitigations.CheckState == CheckState.Checked || 
                                                   _fullyThreatActors.CheckState == CheckState.Checked || 
                                                   _schemas.CheckedItems.Count > 0 || _itemTemplates.CheckedItems.Count > 0 ||
                                                   _threatTypes.CheckedItems.Count > 0 || _mitigations.CheckedItems.Count > 0 ||
                                                   (e.NewValue == CheckState.Checked || _threatActors.CheckedItems.Count > 1)) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _threatTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pageThreatTypes.NextButtonEnabled = (_wizard.GetNextPage() != _pageFinish ||
                                                  _fullySchemas.CheckState == CheckState.Checked ||
                                                  _fullyItemTemplates.CheckState == CheckState.Checked ||
                                                  _fullyThreatTypes.CheckState == CheckState.Checked ||
                                                  _fullyMitigations.CheckState == CheckState.Checked || 
                                                  _fullyThreatActors.CheckState == CheckState.Checked || 
                                                  _schemas.CheckedItems.Count > 0 || _itemTemplates.CheckedItems.Count > 0 ||
                                                  (e.NewValue == CheckState.Checked || _threatTypes.CheckedItems.Count > 1 ) || 
                                                  _mitigations.CheckedItems.Count > 0 || _threatActors.CheckedItems.Count > 0) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }

        private void _mitigations_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pageMitigations.NextButtonEnabled = (_wizard.GetNextPage() != _pageFinish ||
                                                  _fullySchemas.CheckState == CheckState.Checked ||
                                                  _fullyItemTemplates.CheckState == CheckState.Checked ||
                                                  _fullyThreatTypes.CheckState == CheckState.Checked ||
                                                  _fullyMitigations.CheckState == CheckState.Checked || 
                                                  _fullyThreatActors.CheckState == CheckState.Checked || 
                                                  _schemas.CheckedItems.Count > 0 || _itemTemplates.CheckedItems.Count > 0 ||
                                                  _threatTypes.CheckedItems.Count > 0 || _mitigations.CheckedItems.Count > 0 ||
                                                  _threatActors.CheckedItems.Count > 0) ?
                eWizardButtonState.True : eWizardButtonState.False;
        }
        #endregion

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _pageTMProperties.NextButtonEnabled = string.IsNullOrWhiteSpace(_name.Text)
                ? eWizardButtonState.False
                : eWizardButtonState.True;
        }

        private void _description_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.LinkText, 
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", 
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(e.LinkText);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                }
            }
            catch
            {
                // Ignore the error because the link is simply not trusted.
            }
        }
        
        private void AddSpellCheck([NotNull] TextBoxBase control)
        {
            try
            {
                if (control is RichTextBox richTextBox)
                {
                    _spellAsYouType.AddTextComponent(new RichTextBoxSpellAsYouTypeAdapter(richTextBox, 
                        _spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase));
                }
                else
                {
                    _spellAsYouType.AddTextBoxBase(control);
                }
            }
            catch
            {
            }
        }

        private void ExportTemplateDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
