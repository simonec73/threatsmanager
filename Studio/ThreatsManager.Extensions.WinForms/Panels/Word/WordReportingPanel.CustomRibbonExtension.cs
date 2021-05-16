using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Extensions.Panels.Word.Engine;
using ThreatsManager.Extensions.Panels.Word.Engine.Fields;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Syncfusion.OfficeChart;
using ThreatsManager.Extensions.Panels.Configuration;
using ThreatsManager.Extensions.Panels.Diagram;
using ThreatsManager.Extensions.Panels.Roadmap;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Word
{
#pragma warning disable CS0067
    public partial class WordReportingPanel
    {
        private string _lastDocument;
        private ProgressDialog _progress;

        
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Word Report";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Full", "Generate Document",
                            Properties.Resources.docx_big,
                            Properties.Resources.docx),
                    }),
                    new CommandsBarDefinition("Open", "Open", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Open", "Open Last Document",
                            Properties.Resources.folder_open_document_big,
                            Properties.Resources.folder_open_document),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "Full":
                        if (SaveFull())
                            text = "Document Generation";
                        break;
                    case "Open":
                        if (!string.IsNullOrWhiteSpace(_lastDocument) && File.Exists(_lastDocument) &&
                            string.CompareOrdinal(Path.GetExtension(_lastDocument), ".docx") == 0)
                        {
                            text = "Document Opening";
                            Process.Start(_lastDocument);
                        }
                        break;
                    case "Refresh":
                        _docStructure.PrimaryGrid.Rows.Clear();
                        if (!string.IsNullOrWhiteSpace(_wordFile.Text))
                        {
                            var file = GetDocumentPath(_model, _wordFile.Text);
                            if (File.Exists(file))
                            {
                                LoadDocStructure(file);
                            }
                        }
                        else
                        {
                            ShowWarning?.Invoke($"No Reference Word file has been selected.");
                        }
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        #region Full Save.
        private bool SaveFull()
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(_wordFile.Text))
            {
                var originalPath = GetDocumentPath(_model, _wordFile.Text);
                var fileName = Path.Combine(Path.GetDirectoryName(originalPath),
                    $"{Path.GetFileNameWithoutExtension(originalPath)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.docx");

                var doc = new WordDocument();

                try
                {
                    ShowProgress();

                    try
                    {
                        doc.OpenReadOnly(originalPath, Syncfusion.DocIO.FormatType.Automatic);

                        UpdateModelPlaceholders(doc);
                        UpdateProgress(10);
                        UpdateCounterPlaceholders(doc);
                        UpdateProgress(20);
                        UpdateChartPlaceholders(doc);
                        UpdateProgress(30);
                        UpdateListPlaceholders(doc);
                        UpdateProgress(50);
                        UpdateTablePlaceholders(doc);
                        UpdateProgress(70);

                        try
                        {
                            doc.UpdateDocumentFields();
                        }
                        catch
                        {
                            ShowWarning?.Invoke(
                                "Automatic Fields Update for the newly generated file failed.<br/>We are sorry, but you to refresh it manually, by selecting all the content (CTRL+A) and refreshing it (F9).");
                        }

                        doc.Save(fileName);
                        UpdateProgress(90);

                        _lastDocument = fileName;
                    }
                    catch (IOException)
                    {
                        ShowWarning?.Invoke("Reference document may be in use in Word, please close it and try again.\nIf the problem persists, then the file may not be accessible.");
                    }
                    catch
                    {
                        ShowWarning?.Invoke("Reference document may be corrupted or not a Word document.");
                    }
                    finally
                    {
                        doc?.Close();
                    }

                    UpdateToC(fileName);

                    result = true;
                }
                finally
                {
                    CloseProgress();
                }
            }
            else
            {
                ShowWarning?.Invoke("Please select the reference Word File.");
            }

            return result;
        }

        private void ShowProgress()
        {
            _progress = new ProgressDialog();
            _progress.Show(Form.ActiveForm);
        }

        private void UpdateProgress(int percentage)
        {
            _progress.Value = percentage;
        }

        private void CloseProgress()
        {
            _progress.Close();
            _progress = null;
        }

        private void UpdateToC([Required] string fileName)
        {
            var doc = new WordDocument(fileName);
            doc.UpdateTableOfContents();
            doc.Save(fileName);
            doc.Close();
        }

        private void UpdateModelPlaceholders([NotNull] WordDocument doc)
        {
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:ModelName\]"), _model.Name ?? string.Empty);
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:ModelDescription\]"), _model.Description ?? string.Empty);
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:ModelOwner\]"), _model.Owner ?? string.Empty);
            ReplaceBullets(doc, "[ThreatsManagerPlatform:ModelContributors]", _model.Contributors);
            ReplaceBullets(doc, "[ThreatsManagerPlatform:ModelAssumptions]", _model.Assumptions);
            ReplaceBullets(doc, "[ThreatsManagerPlatform:ModelDependencies]", _model.ExternalDependencies);
        }

        private void UpdateCounterPlaceholders([NotNull] WordDocument doc)
        {
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterThreatTypes\]"), 
                _model.AssignedThreatTypes.ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterThreatEvents\]"), 
                _model.TotalThreatEvents.ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterMitigations\]"), 
                _model.UniqueMitigations.ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterCriticalThreatTypes\]"), 
                _model.CountThreatEventsByType((int) DefaultSeverity.Critical).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterHighThreatTypes\]"), 
                _model.CountThreatEventsByType((int) DefaultSeverity.High).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterMediumThreatTypes\]"), 
                _model.CountThreatEventsByType((int) DefaultSeverity.Medium).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterLowThreatTypes\]"), 
                _model.CountThreatEventsByType((int) DefaultSeverity.Low).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterInfoThreatTypes\]"), 
                _model.CountThreatEventsByType((int) DefaultSeverity.Info).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterProposedMitigations\]"), 
                _model.CountMitigationsByStatus(MitigationStatus.Proposed).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterExistingMitigations\]"), 
                _model.CountMitigationsByStatus(MitigationStatus.Existing).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterImplementedMitigations\]"), 
                _model.CountMitigationsByStatus(MitigationStatus.Implemented).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterExistingImplementedMitigations\]"), 
                (_model.CountMitigationsByStatus(MitigationStatus.Implemented) + _model.CountMitigationsByStatus(MitigationStatus.Existing)).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterApprovedMitigations\]"), 
                _model.CountMitigationsByStatus(MitigationStatus.Approved).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterPlannedMitigations\]"), 
                _model.CountMitigationsByStatus(MitigationStatus.Planned).ToString());

            var mitigations = _model.Mitigations?
                .Select(x => new KeyValuePair<Guid, RoadmapStatus>(x.Id, x.GetStatus()))
                .ToArray();

            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterShortTermMitigations\]"), 
                (mitigations?.Count(x => x.Value == RoadmapStatus.ShortTerm) ?? 0).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterMidTermMitigations\]"), 
                (mitigations?.Count(x => x.Value == RoadmapStatus.MidTerm) ?? 0).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterLongTermMitigations\]"), 
                (mitigations?.Count(x => x.Value == RoadmapStatus.LongTerm) ?? 0).ToString());
            doc.Replace(new Regex(@"\[ThreatsManagerPlatform:CounterNotRequiredMitigations\]"), 
                (mitigations?.Count(x => x.Value == RoadmapStatus.NoActionRequired) ?? 0).ToString());
        }

        private void UpdateChartPlaceholders([NotNull] WordDocument doc)
        {
            var severities = _model.Severities?
                .Where(x => x.Visible && x.Id > 0)
                .OrderByDescending(x => x.Id).ToArray();
            ReplacePieChart(doc, "[ThreatsManagerPlatform:ChartThreatTypes]", severities?.Select(x => x.Name),
                severities?.Select(x => _model.CountThreatEventsByType(x)), severities?.Select(x => x.BackColor));

            ReplacePieChart(doc, "[ThreatsManagerPlatform:ChartThreatEvents]", severities?.Select(x => x.Name),
                severities?.Select(x => _model.CountThreatEvents(x)), severities?.Select(x => x.BackColor));

            var states = EnumExtensions.GetEnumLabels<MitigationStatus>().ToArray();
            ReplacePieChart(doc, "[ThreatsManagerPlatform:ChartMitigations]", states,
                states.Select(x => _model.CountMitigationsByStatus(x.GetEnumValue<MitigationStatus>())),
                null);

            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(_model);
            var estimator = schemaManager.SelectedEstimator;
            if (estimator != null)
            {
                var mitigations = _model.Mitigations?.ToArray();
                if ((mitigations?.Any() ?? false))
                {
                    var selectedMitigations = new List<Guid>();

                    var currentRisk = estimator.Estimate(_model, null, out var currentMin, out var currentMax);
                    var current = 100f;

                    var shortTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.ShortTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (shortTermMitigations.Any())
                        selectedMitigations.AddRange(shortTermMitigations);
                    var shortTerm = estimator.Estimate(_model, selectedMitigations, out var shortTermMin, out var shortTermMax) * 100f / currentRisk;
                
                    var midTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.MidTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (midTermMitigations.Any())
                        selectedMitigations.AddRange(midTermMitigations);
                    var midTerm = estimator.Estimate(_model, selectedMitigations, out var midTermMin, out var midTermMax) * 100f / currentRisk;
                
                    var longTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.LongTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (longTermMitigations.Any())
                        selectedMitigations.AddRange(longTermMitigations);
                    var longTerm = estimator.Estimate(_model, selectedMitigations, out var longTermMin, out var longTermMax) * 100f / currentRisk;

                    float acceptableRisk;
                    var parameters = schemaManager.Parameters?.ToArray();
                    if (parameters?.Any() ?? false)
                    {
                        var infinite = schemaManager.Infinite;
                        if (infinite < 0)
                            infinite = estimator.DefaultInfinite;

                        var normalizationReference =
                            (new ExtensionConfigurationManager(_model, (new ConfigurationPanelFactory()).GetExtensionId())).NormalizationReference;

                        var p = parameters.ToDictionary(x => x.Name, x => x.Value);
                        acceptableRisk = estimator.GetAcceptableRisk(_model, p, infinite, normalizationReference) * 100f / currentRisk;
                    }
                    else
                    {
                        acceptableRisk = 0f;
                    }

                    ReplaceColumnChart(doc, "[ThreatsManagerPlatform:ChartRoadmap]",
                        new[]
                        {
                            "Current", RoadmapStatus.ShortTerm.GetEnumLabel(), RoadmapStatus.MidTerm.GetEnumLabel(),
                            RoadmapStatus.LongTerm.GetEnumLabel()
                        },
                        new [] { current, shortTerm, midTerm, longTerm },
                        new []
                        {
                            current <= acceptableRisk ? KnownColor.Green : KnownColor.Red,
                            shortTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red,
                            midTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red,
                            longTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red
                        }
                    );
                }
            }
        }

        private void UpdateListPlaceholders([NotNull] WordDocument doc)
        {
            var placeholders = GetPlaceholders(PlaceholderSection.List);

            var diagrams = _model.Diagrams?.OrderBy(x => x.Order).ToArray();
            ReplaceDiagram(doc, "[ThreatsManagerPlatform:ListDiagrams]", diagrams);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListExternalInteractors]",
                ItemType.ExternalInteractor, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListProcesses]",
                ItemType.Process, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListStorages]",
                ItemType.DataStore, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListFlows]",
                ItemType.DataFlow, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListTrustBoundaries]",
                ItemType.TrustBoundary, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListThreatTypes]",
                ItemType.ThreatType, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListThreatEvents]",
                ItemType.ThreatEvent, placeholders);

            ReplaceList(doc, "[ThreatsManagerPlatform:ListMitigations]",
                ItemType.Mitigation, placeholders);

            DoSecondPass(doc, ItemType.ThreatType, placeholders);
            DoSecondPass(doc, ItemType.ThreatEvent, placeholders);
            DoSecondPass(doc, ItemType.Mitigation, placeholders);
        }

        private void UpdateTablePlaceholders([NotNull] WordDocument doc)
        {
            var placeholders = GetPlaceholders(PlaceholderSection.Table)?.ToArray();

            var tableSeverities = new Table(new [] {"Severity", "Description"});
            var phSeverities = placeholders?.FirstOrDefault(x => x.TableType == TableType.Severities);
            tableSeverities.SetColumnWidth(0, GetColumnWidth(phSeverities, "Severity"), 100f);
            tableSeverities.SetColumnWidth(1, GetColumnWidth(phSeverities, "Description"), 600f);
            var severities = _model.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).ToArray();
            if (severities?.Any() ?? false)
            {
                foreach (var severity in severities)
                {
                    tableSeverities.AddRow(new Row(new [] {severity.Name, severity.Description}));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableSeverities]", tableSeverities);

            var tableActors = new Table(new [] { "Threat Actor", "Description" });
            var phActors = placeholders?.FirstOrDefault(x => x.TableType == TableType.ThreatActors);
            tableActors.SetColumnWidth(0, GetColumnWidth(phActors, "Threat Actor"), 100f);
            tableActors.SetColumnWidth(1, GetColumnWidth(phActors, "Description"), 600f);
            var actors = _model.ThreatActors?.OrderBy(x => x.Name).ToArray();
            if (actors?.Any() ?? false)
            {
                foreach (var actor in actors)
                {
                    tableActors.AddRow(new Row(new [] { actor.Name, actor.Description }));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableThreatActors]", tableActors);

            var tableStrengths = new Table(new [] { "Strength", "Description" });
            var phStrengths = placeholders?.FirstOrDefault(x => x.TableType == TableType.Strengths);
            tableStrengths.SetColumnWidth(0, GetColumnWidth(phStrengths, "Strength"), 100f);
            tableStrengths.SetColumnWidth(1, GetColumnWidth(phStrengths, "Description"), 600f);
            var strengths = _model.Strengths?.OrderByDescending(x => x.Id).ToArray();
            if (strengths?.Any() ?? false)
            {
                foreach (var strength in strengths)
                {
                    tableStrengths.AddRow(new Row(new [] { strength.Name, strength.Description }));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableStrengths]", tableStrengths);

            var tableControlTypes = new Table(new[] { "Control Type", "Description" });
            var phControlTypes = placeholders?.FirstOrDefault(x => x.TableType == TableType.ControlTypes);
            tableControlTypes.SetColumnWidth(0, GetColumnWidth(phControlTypes, "Control Type"), 100f);
            tableControlTypes.SetColumnWidth(1, GetColumnWidth(phControlTypes, "Description"), 600f);
            var controlTypes = EnumExtensions.GetEnumLabels<SecurityControlType>()
                .Select(x => x.GetEnumValue<SecurityControlType>())
                .ToArray();
            if (controlTypes?.Any() ?? false)
            {
                foreach (var controlType in controlTypes)
                {
                    tableControlTypes.AddRow(new Row(new[] { controlType.GetEnumLabel(), controlType.GetEnumDescription() }));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableControlTypes]", tableControlTypes);

            var tableMitigationStatus = new Table(new[] { "Status Name", "Description" });
            var phMitigationStatus = placeholders?.FirstOrDefault(x => x.TableType == TableType.MitigationStatus);
            tableMitigationStatus.SetColumnWidth(0, GetColumnWidth(phMitigationStatus, "Status Name"), 100f);
            tableMitigationStatus.SetColumnWidth(1, GetColumnWidth(phMitigationStatus, "Description"), 600f);
            var states = EnumExtensions.GetEnumLabels<MitigationStatus>()
                .Select(x => x.GetEnumValue<MitigationStatus>())
                .Where(x => _model.CountMitigationsByStatus(x) > 0)
                .ToArray();
            if (states?.Any() ?? false)
            {
                foreach (var state in states)
                {
                    tableMitigationStatus.AddRow(new Row(new[] { state.GetEnumLabel(), state.GetEnumDescription() }));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableMitigationStatus]", tableMitigationStatus);

            var tableSummaryThreatTypes = new Table(new[] { "Threat Type", "Severity", "Mitigations" });
            var phSummaryThreatTypes = placeholders?.FirstOrDefault(x => x.TableType == TableType.SummaryThreatTypes);
            tableSummaryThreatTypes.SetColumnWidth(0, GetColumnWidth(phSummaryThreatTypes, "Threat Type"), 200f);
            tableSummaryThreatTypes.SetColumnWidth(1, GetColumnWidth(phSummaryThreatTypes, "Severity"), 50f);
            tableSummaryThreatTypes.SetColumnWidth(2, GetColumnWidth(phSummaryThreatTypes, "Mitigations"), 300f);
            var threatTypes = GetItems(ItemType.ThreatType)?.OfType<IThreatType>().ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    tableSummaryThreatTypes.AddRow(new Row(threatType, GetTopSeverity(threatType), GetAssociatedMitigations(threatType)));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableSummaryThreatTypes]", tableSummaryThreatTypes);

            var tableSummaryThreatEvents = new Table(new[] { "Threat Event", "Severity", "Mitigations" });
            var phSummaryThreatEvents = placeholders?.FirstOrDefault(x => x.TableType == TableType.SummaryThreatEvents);
            tableSummaryThreatEvents.SetColumnWidth(0, GetColumnWidth(phSummaryThreatEvents, "Threat Event"), 200f);
            tableSummaryThreatEvents.SetColumnWidth(1, GetColumnWidth(phSummaryThreatEvents, "Severity"), 50f);
            tableSummaryThreatEvents.SetColumnWidth(2, GetColumnWidth(phSummaryThreatEvents, "Mitigations"), 300f);
            var threatEvents = GetItems(ItemType.ThreatEvent)?.OfType<IThreatEvent>().ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    tableSummaryThreatEvents.AddRow(new Row(threatEvent));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableSummaryThreatEvents]", tableSummaryThreatEvents);

            var tableSummaryMitigations = new Table(new[] { "Mitigation", "Related Threats" });
            var phSummaryMitigations = placeholders?.FirstOrDefault(x => x.TableType == TableType.SummaryMitigations);
            tableSummaryMitigations.SetColumnWidth(0, GetColumnWidth(phSummaryMitigations, "Mitigation"), 200f);
            tableSummaryMitigations.SetColumnWidth(1, GetColumnWidth(phSummaryMitigations, "Related Threats"), 500f);
            var mitigations = GetItems(ItemType.Mitigation)?.OfType<IMitigation>().ToArray();
            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    var sM = GetAssociatedThreats(mitigation);
                    if (sM?.Any() ?? false)
                        tableSummaryMitigations.AddRow(new Row(mitigation, sM));
                }
            }
            ReplaceTable(doc, "[ThreatsManagerPlatform:TableSummaryMitigations]", tableSummaryMitigations);

            var tableRoadmap = new Table(new[] {"Mitigation", "Related Threats"});
            var phRoadmap = placeholders?.FirstOrDefault(x => x.TableType == TableType.Roadmap);
            tableRoadmap.SetColumnWidth(0, GetColumnWidth(phRoadmap, "Mitigation"), 200f);
            tableRoadmap.SetColumnWidth(1, GetColumnWidth(phRoadmap, "Related Threats"), 500f);
            
            var roadmapShortTerm = GetItems(ItemType.Mitigation)?
                .OfType<IMitigation>()
                .Where(x => x.GetStatus() == RoadmapStatus.ShortTerm)
                .ToArray();
            if (roadmapShortTerm?.Any() ?? false)
            {
                tableRoadmap.AddRow(new Row("Short Term"));
                foreach (var mitigation in roadmapShortTerm)
                {
                    var rM= GetAssociatedThreats(mitigation, true);
                    if (rM?.Any() ?? false)
                        tableRoadmap.AddRow(new Row(mitigation, rM));
                }
            }
            
            var roadmapMidTerm = GetItems(ItemType.Mitigation)?
                .OfType<IMitigation>()
                .Where(x => x.GetStatus() == RoadmapStatus.MidTerm)
                .ToArray();
            if (roadmapMidTerm?.Any() ?? false)
            {
                tableRoadmap.AddRow(new Row("Mid Term"));
                foreach (var mitigation in roadmapMidTerm)
                {
                    var rM = GetAssociatedThreats(mitigation, true);
                    if (rM?.Any() ?? false)
                        tableRoadmap.AddRow(new Row(mitigation, rM));
                }
            }
            
            var roadmapLongTerm = GetItems(ItemType.Mitigation)?
                .OfType<IMitigation>()
                .Where(x => x.GetStatus() == RoadmapStatus.LongTerm)
                .ToArray();
            if (roadmapLongTerm?.Any() ?? false)
            {
                tableRoadmap.AddRow(new Row("Long Term"));
                foreach (var mitigation in roadmapLongTerm)
                {
                    var rM = GetAssociatedThreats(mitigation, true);
                    if (rM?.Any() ?? false)
                        tableRoadmap.AddRow(new Row(mitigation, rM));
                }
            }

            ReplaceTable(doc, "[ThreatsManagerPlatform:TableRoadmap]", tableRoadmap);
        }

        private IEnumerable<Placeholder> GetPlaceholders(PlaceholderSection section)
        {
            return _docStructure.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => !string.IsNullOrWhiteSpace(x.Cells[0].Value?.ToString()))
                .FirstOrDefault(x =>
                    x.Cells[0].Value.ToString().GetEnumValue<PlaceholderSection>() == section)?
                .Rows.OfType<GridPanel>().FirstOrDefault()?.Rows
                .Select(x => x.Tag as Placeholder);
        }

        private float GetColumnWidth(Placeholder placeholder, [Required] string propertyName)
        {
            float result = 0.0f;

            var width = placeholder?.PropertyWidths
                .Where(x => string.CompareOrdinal(x.Key, propertyName) == 0)
                .Select(x => x.Value).FirstOrDefault();
            if ((width ?? 0.0) > 0.0)
            {
                result = width.Value;
            }

            return result;
        }

        private void ReplaceBullets([NotNull] WordDocument doc, [Required] string filter, IEnumerable<string> items)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                foreach (var selection in selections)
                {
                    var range = selection.GetAsOneRange();
                    range.Text = string.Empty;

                    if (items?.Any() ?? false)
                    {
                        var paragraph = range.OwnerParagraph;
                        if (!paragraph.IsToc())
                        {
                            var section = GetSection(paragraph);
                            if (section != null)
                            {
                                var index = section.Body.ChildEntities.IndexOf(paragraph);
                                if (index >= 0)
                                {
                                    bool first = true;
                                    foreach (var item in items)
                                    {
                                        if (first)
                                        {
                                            first = false;
                                        }
                                        else
                                        {
                                            paragraph = new WParagraph(doc);
                                            section.Paragraphs.Insert(index + 1, paragraph);
                                            index++;
                                        }

                                        paragraph.ListFormat.ApplyDefBulletStyle();
                                        paragraph.AppendText(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ReplacePieChart([NotNull] WordDocument doc, [Required] string filter,
            IEnumerable<string> names, IEnumerable<int> counts, IEnumerable<KnownColor> colors)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                foreach (var selection in selections)
                {
                    var range = selection.GetAsOneRange();
                    range.Text = string.Empty;
                    var paragraph = range.OwnerParagraph;
                    if (!paragraph.IsToc())
                    {
                        var section = GetSection(paragraph);
                        if (section != null)
                        {
                            var width = section.PageSetup.PageSize.Width - section.PageSetup.Margins.Left -
                                        section.PageSetup.Margins.Right;
                            var height = width / 3.0f * 2.0f;

                            var chart = paragraph.AppendChart(width, height);
                            chart.ChartType = OfficeChartType.Pie;
                            chart.ChartData.SetValue(1, 1, "Name");
                            chart.ChartData.SetValue(1, 2, "Count");

                            if ((names?.Any() ?? false) && (counts?.Any() ?? false) && names.Count() == counts.Count())
                            {
                                var row = 1;
                                for (int i = 0; i < names.Count(); i++)
                                {
                                    row++;
                                    chart.ChartData.SetValue(row, 1, names.ElementAt(i));
                                    chart.ChartData.SetValue(row, 2, counts.ElementAt(i).ToString());
                                }

                                IOfficeChartSerie pieSeries = chart.Series.Add();
                                pieSeries.Values = chart.ChartData[2, 2, row, 2];
                                pieSeries.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                                pieSeries.DataPoints.DefaultDataPoint.DataLabels.Position =
                                    OfficeDataLabelPosition.Outside;
                                pieSeries.DataPoints.DefaultDataPoint.DataFormat.LineProperties.LineWeight =
                                    OfficeChartLineWeight.Hairline;

                                if (colors != null && names.Count() == colors.Count())
                                {
                                    for (int i = 0; i < row - 1; i++)
                                    {
                                        chart.Series[0].DataPoints[i].DataFormat.Fill.ForeColor =
                                            Color.FromKnownColor(colors.ElementAt(i));
                                    }
                                }

                                chart.PrimaryCategoryAxis.CategoryLabels = chart.ChartData[2, 1, row, 1];
                            }

                            chart.Legend.Position = OfficeLegendPosition.Bottom;
                            chart.ChartArea.Border.LineWeight = OfficeChartLineWeight.Hairline;
                            chart.OfficeChart.HasTitle = false;
                        }
                    }
                }
            }
        }

        private void ReplaceColumnChart([NotNull] WordDocument doc, [Required] string filter,
            IEnumerable<string> names, IEnumerable<float> values, IEnumerable<KnownColor> colors)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                foreach (var selection in selections)
                {
                    var range = selection.GetAsOneRange();
                    range.Text = string.Empty;
                    var paragraph = range.OwnerParagraph;
                    if (!paragraph.IsToc())
                    {
                        var section = GetSection(paragraph);
                        if (section != null)
                        {
                            var width = section.PageSetup.PageSize.Width - section.PageSetup.Margins.Left -
                                        section.PageSetup.Margins.Right;
                            var height = width / 3.0f * 2.0f;

                            var chart = paragraph.AppendChart(width, height);
                            chart.ChartType = OfficeChartType.Column_Clustered;
                            chart.ChartData.SetValue(1, 1, "Name");
                            chart.ChartData.SetValue(1, 2, "Value");

                            if ((names?.Any() ?? false) && (values?.Any() ?? false) && names.Count() == values.Count())
                            {
                                var row = 1;
                                for (int i = 0; i < names.Count(); i++)
                                {
                                    row++;
                                    chart.ChartData.SetValue(row, 1, names.ElementAt(i));
                                    chart.ChartData.SetValue(row, 2, values.ElementAt(i).ToString());
                                }

                                IOfficeChartSerie series = chart.Series.Add();
                                series.Values = chart.ChartData[2, 2, row, 2];
                                //series.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                                //series.DataPoints.DefaultDataPoint.DataLabels.Position =
                                //    OfficeDataLabelPosition.Outside;
                                series.DataPoints.DefaultDataPoint.DataFormat.LineProperties.LineWeight =
                                    OfficeChartLineWeight.Hairline;

                                if (colors != null && names.Count() == colors.Count())
                                {
                                    for (int i = 0; i < row - 1; i++)
                                    {
                                        chart.Series[0].DataPoints[i].DataFormat.Fill.ForeColor =
                                            Color.FromKnownColor(colors.ElementAt(i));
                                    }
                                }

                                chart.PrimaryCategoryAxis.CategoryLabels = chart.ChartData[2, 1, row, 1];
                            }

                            chart.HasLegend = false;
                            chart.ChartArea.Border.LineWeight = OfficeChartLineWeight.Hairline;
                            chart.OfficeChart.HasTitle = false;
                            chart.PrimaryValueAxis.Visible = false;
                            chart.PrimaryValueAxis.HasMajorGridLines = false;
                        }
                    }
                }
            }
        }

        private void ReplaceTable([NotNull] WordDocument doc, [Required] string filter, [NotNull] Table tableInfo)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                foreach (var selection in selections)
                {
                    var range = selection.GetAsOneRange();
                    range.Text = string.Empty;
                    var paragraph = range.OwnerParagraph;
                    if (!paragraph.IsToc())
                    {
                        var section = GetSection(paragraph);
                        if (section != null)
                        {
                            var pageWidth = section.PageSetup.PageSize.Width - section.PageSetup.Margins.Left -
                                            section.PageSetup.Margins.Right;
                            var oneHundredPercent = Math.Max(tableInfo.TotalWidth, 100f);

                            var table = new WTable(doc);
                            var index = section.Body.ChildEntities.IndexOf(paragraph);
                            if (index >= 0)
                            {
                                section.Body.ChildEntities.RemoveAt(index);
                                section.Body.ChildEntities.Insert(index, table);
                            }

                            int count = tableInfo.RowCount;
                            table.ResetCells(count + 1, tableInfo.ColumnCount);
                            table.ApplyStyleForBandedColumns = false;
                            table.ApplyStyleForBandedRows = false;
                            table.ApplyStyleForFirstColumn = false;
                            table.ApplyStyleForHeaderRow = true;
                            table.ApplyStyleForLastColumn = false;
                            table.ApplyStyleForLastRow = false;
                            table.TableFormat.IsAutoResized = true;

                            var header = table.Rows[0];
                            for (int i = 0; i < tableInfo.ColumnCount; i++)
                            {
                                header.Cells[i].AddParagraph().AppendText(tableInfo.Header.ElementAt(i));
                                var width = tableInfo.GetColumnWidth(i);
                                if (width > 0)
                                    header.Cells[i].Width = width / oneHundredPercent * pageWidth;
                            }

                            for (int i = 0; i < tableInfo.RowCount; i++)
                            {
                                var row = table.Rows[i + 1];
                                var tableRow = tableInfo.Rows.ElementAt(i);

                                for (int j = 0; j < tableInfo.ColumnCount; j++)
                                {
                                    switch (tableRow.RowType)
                                    {
                                        case RowType.Normal:
                                            row.Cells[j].AddParagraph().AppendText(tableRow.Values.ElementAt(j));
                                            break;
                                        case RowType.Merged:
                                            if (j == 0)
                                            {
                                                var mergeText = row.Cells[0].AddParagraph()
                                                    .AppendText(tableRow.Values.FirstOrDefault());
                                                mergeText.CharacterFormat.Bold = true;
                                                row.Cells[0].CellFormat.HorizontalMerge = CellMerge.Start;
                                            }
                                            else
                                            {
                                                row.Cells[j].CellFormat.HorizontalMerge = CellMerge.Continue;
                                            }
                                            break;
                                        case RowType.ThreatType:
                                            CreateThreatTypeRow(tableRow, row, j);
                                            break;
                                        case RowType.ThreatEvent:
                                            CreateThreatEventRow(tableRow, row, j);
                                            break;
                                        case RowType.Mitigation:
                                            CreateMitigationRow(tableRow, row, j);
                                            break;
                                    }

                                    var width = tableInfo.GetColumnWidth(j);
                                    if (width > 0)
                                        row.Cells[j].Width = width / oneHundredPercent * pageWidth;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateThreatTypeRow(Row tableRow, WTableRow row, int col)
        {
            IWParagraph par = null;

            switch (col)
            {
                case 0:
                    par = row.Cells[col].AddParagraph();
                    var bookmarkTt = row.Document.Bookmarks.FindByName(tableRow.Identity.Id.ToString("N"));
                    if (bookmarkTt != null)
                    {
                        par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText, bookmarkTt.BookmarkStart,
                                true, false, false, null);
                    }
                    else
                    {
                        par.AppendText(tableRow.Values.ElementAt(col));
                    }

                    break;
                case 1:
                    par = row.Cells[col].AddParagraph();
                    var text = par.AppendText(tableRow.Values.ElementAt(col));
                    text.CharacterFormat.TextColor =
                        Color.FromKnownColor(tableRow.SeverityTextColor);
                    row.Cells[col].CellFormat.BackColor =
                        Color.FromKnownColor(tableRow.SeverityBackColor);
                    row.Cells[col].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    break;
                case 2:
                    var mitigations = tableRow.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        var states = Enum.GetValues(typeof(MitigationStatus));
                        foreach (MitigationStatus status in states)
                        {
                            var filteredList = mitigations.Where(x => x.Status == status)
                                .ToArray();
                            if (filteredList.Any())
                            {
                                par = row.Cells[col].AddParagraph();
                                par.AppendText(status.ToString());
                                foreach (var item in filteredList)
                                {
                                    par = row.Cells[col].AddParagraph();
                                    var bookmarkM = row.Document.Bookmarks.FindByName(item.MitigationId.ToString("N"));
                                    if (bookmarkM != null)
                                    {
                                        par.AppendText(
                                            $"[{_model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] {item.ThreatEvent.Parent.Name}: ");
                                        par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText,
                                            bookmarkM.BookmarkStart,
                                            true, false, false, null);
                                    }
                                    else
                                    {
                                        par.AppendText(
                                            $"[{_model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] {item.ThreatEvent.Parent.Name}: {item.Mitigation.Name}");
                                    }

                                    par.ListFormat.ApplyDefBulletStyle();
                                }
                            }
                        }
                    }

                    break;
            }
        }

        private void CreateThreatEventRow(Row tableRow, WTableRow row, int col)
        {
            if (tableRow.Identity is IThreatEvent threatEvent)
            {
                IWParagraph par = null;

                switch (col)
                {
                    case 0:
                        par = row.Cells[col].AddParagraph();
                        var bookmarkTt = row.Document.Bookmarks.FindByName(threatEvent.Id.ToString("N"));
                        if (bookmarkTt != null)
                        {
                            par.AppendText(
                                $"[{_model.GetIdentityTypeInitial(threatEvent.Parent)}] {threatEvent.Parent.Name}: ");
                            par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText,
                                bookmarkTt.BookmarkStart,
                                true, false, false, null);
                        }
                        else
                        {
                            par.AppendText(
                                $"[{_model.GetIdentityTypeInitial(threatEvent.Parent)}] {threatEvent.Parent.Name}: {threatEvent.Name}");
                        }

                        break;
                    case 1:
                        par = row.Cells[col].AddParagraph();
                        var text = par.AppendText(tableRow.Values.ElementAt(col));
                        text.CharacterFormat.TextColor =
                            Color.FromKnownColor(tableRow.SeverityTextColor);
                        row.Cells[col].CellFormat.BackColor =
                            Color.FromKnownColor(tableRow.SeverityBackColor);
                        row.Cells[col].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        break;
                    case 2:
                        var mitigations = tableRow.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            var states = Enum.GetValues(typeof(MitigationStatus));
                            foreach (MitigationStatus status in states)
                            {
                                var filteredList = mitigations.Where(x => x.Status == status)
                                    .ToArray();
                                if (filteredList.Any())
                                {
                                    par = row.Cells[col].AddParagraph();
                                    par.AppendText(status.ToString());
                                    foreach (var item in filteredList)
                                    {
                                        par = row.Cells[col].AddParagraph();
                                        var bookmarkM =
                                            row.Document.Bookmarks.FindByName(item.MitigationId.ToString("N"));
                                        if (bookmarkM != null)
                                        {
                                            par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText,
                                                bookmarkM.BookmarkStart,
                                                true, false, false, null);
                                        }
                                        else
                                        {
                                            par.AppendText(item.Mitigation.Name);
                                        }

                                        par.ListFormat.ApplyDefBulletStyle();
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }

        private void CreateMitigationRow(Row tableRow, WTableRow row, int col)
        {
            if (tableRow.Identity is IMitigation mitigation)
            {
                IWParagraph par = null;

                switch (col)
                {
                    case 0:
                        par = row.Cells[col].AddParagraph();
                        var bookmarkM = row.Document.Bookmarks.FindByName(mitigation.Id.ToString("N"));
                        if (bookmarkM != null)
                        {
                            par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText,
                                bookmarkM.BookmarkStart,
                                true, false, false, null);
                        }
                        else
                        {
                            par.AppendText(mitigation.Name);
                        }

                        break;
                    case 1:
                        var mitigations = tableRow.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            var states = Enum.GetValues(typeof(MitigationStatus));
                            foreach (MitigationStatus status in states)
                            {
                                var filteredList = mitigations.Where(x => x.Status == status)
                                    .ToArray();
                                if (filteredList.Any())
                                {
                                    par = row.Cells[col].AddParagraph();
                                    par.AppendText(status.ToString());
                                    foreach (var item in filteredList)
                                    {
                                        par = row.Cells[col].AddParagraph();
                                        var bookmarkT = row.Document.Bookmarks.FindByName(item.ThreatEvent.Id.ToString("N"));
                                        if (bookmarkT == null)
                                            bookmarkT = row.Document.Bookmarks.FindByName(item.ThreatEvent.ThreatTypeId.ToString("N"));
                                        if (bookmarkT != null)
                                        {
                                            par.AppendText(
                                                $"[{_model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] {item.ThreatEvent.Parent.Name}: ");
                                            par.AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText,
                                                bookmarkT.BookmarkStart,
                                                true, false, false, null);
                                        }
                                        else
                                        {
                                            par.AppendText(
                                                $"[{_model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] {item.ThreatEvent.Parent.Name}: {item.ThreatEvent.ThreatType.Name}");
                                        }

                                        par.ListFormat.ApplyDefBulletStyle();
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }

        private void ReplaceList([NotNull] WordDocument doc, [Required] string filter, 
            ItemType itemType, IEnumerable<Placeholder> placeholders)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                if (itemType != ItemType.Undefined)
                {
                    var items = GetItems(itemType);
                    var placeholder = placeholders?.FirstOrDefault(x => x.ItemType == itemType);
                    var fields = Field.GetFields(itemType, _model)?
                        .Where(x => !(placeholder?.Ignored.Any(y => string.CompareOrdinal(y.Label, x.Label) == 0) ?? false))
                        .ToArray();

                    if (items?.Any() ?? false)
                    {
                        foreach (var selection in selections)
                        {
                            var range = selection.GetAsOneRange();
                            var paragraph = range.OwnerParagraph;
                            if (!paragraph.IsToc())
                            {
                                var section = GetSection(paragraph);
                                if (section != null)
                                {
                                    var style = paragraph.StyleName;
                                    TextBodyItem current = paragraph;

                                    foreach (var item in items)
                                    {
                                        current = AddItem(doc, section, current, style, item, fields);
                                    }

                                    section.Body.ChildEntities.Remove(paragraph);
                                }
                            }
                        }
                    }
                }
            }
        }

        private TextBodyItem AddItem([NotNull] WordDocument doc, [NotNull] WSection wSection,
            TextBodyItem preceding, [Required] string headerStyle, [NotNull] IIdentity identity,
            [NotNull] IEnumerable<Field> fields)
        {
            TextBodyItem result = null;

            WParagraph header = null;
            if (preceding != null)
            {
                var index = wSection.Body.ChildEntities.IndexOf(preceding);
                if (index >= 0)
                {
                    header = new WParagraph(doc);
                    wSection.Paragraphs.Insert(index + 1, header);
                }
            }

            if (header == null)
            {
                header = wSection.AddParagraph() as WParagraph;
            }

            header.ApplyStyle(headerStyle);
            header.AppendBookmarkStart(identity.Id.ToString("N"));
            header.AppendText(identity.Name);
            header.AppendBookmarkEnd(identity.Id.ToString("N"));

            var selected = fields.Where(x => x.IsVisible(identity)).ToArray();

            var count = selected.Count();
            if (count > 0)
            {
                int headerIndex = wSection.Body.ChildEntities.IndexOf(header);
                var table = new WTable(doc);
                result = table;
                wSection.Body.ChildEntities.Insert(headerIndex + 1, table);
                table.ResetCells(count, 2);
                table.ApplyStyle(BuiltinTableStyle.LightList);
                table.ApplyStyleForBandedColumns = false;
                table.ApplyStyleForBandedRows = false;
                table.ApplyStyleForFirstColumn = true;
                table.ApplyStyleForHeaderRow = false;
                table.ApplyStyleForLastColumn = false;
                table.ApplyStyleForLastRow = false;
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
                table.TableFormat.IsAutoResized = true;

                for (int i = 0; i < count; i++)
                {
                    var row = table.Rows[i];

                    var field = selected.ElementAt(i);
                    row.Cells[0].AddParagraph().AppendText(field.Label);
                    row.Cells[0].Width = 100;
                    if (!field.SecondPass)
                        field.InsertContent(row.Cells[1], identity);
                    row.Cells[1].Width = 400;
                }
            }
            else
            {
                result = header;
            }

            return result;
        }

        private void DoSecondPass([NotNull] WordDocument doc, ItemType itemType, IEnumerable<Placeholder> placeholders)
        {
            var items = GetItems(itemType)?.ToArray();
            var placeholder = placeholders?.FirstOrDefault(x => x.ItemType == itemType);
            var fields = Field.GetFields(itemType, _model)?
                .Where(x => !(placeholder?.Ignored.Any(y => string.CompareOrdinal(y.Label, x.Label) == 0) ?? false))
                .ToArray();

            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    DoSecondPass(doc, item, fields);
                }
            }
        }

        private void DoSecondPass([NotNull] WordDocument doc, [NotNull] IIdentity identity,
            [NotNull] IEnumerable<Field> fields)
        {
            var selected = fields.Where(x => x.IsVisible(identity)).ToArray();
            var count = selected.Count();
            if (count > 0)
            {
                var bookmark = doc.Bookmarks.FindByName(identity.Id.ToString("N"));
                WParagraph header = bookmark?.BookmarkStart?.OwnerParagraph;
                if (header?.NextSibling is WTable table)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (i < table.Rows.Count)
                        {
                            var row = table.Rows[i];

                            if (selected[i].SecondPass && row.Cells.Count > 1)
                                selected[i].InsertContent(row.Cells[1], identity);
                        }
                    }
                }
            }
        }

        private void ReplaceDiagram([NotNull] WordDocument doc, [Required] string filter, IEnumerable<IDiagram> diagrams)
        {
            var selections = doc.FindAll(filter, true, true);
            if (selections?.Any() ?? false)
            {
                foreach (var selection in selections)
                {
                    var range = selection.GetAsOneRange();
                    range.Text = string.Empty;
                    var paragraph = range.OwnerParagraph;
                    if (!paragraph.IsToc())
                    {
                        var section = GetSection(paragraph);
                        if (section != null)
                        {
                            var index = section.Body.ChildEntities.IndexOf(paragraph);
                            var style = paragraph.StyleName;
                            section.Body.ChildEntities.RemoveAt(index);
                            if (diagrams?.Any() ?? false)
                            {
                                var width = section.PageSetup.PageSize.Width - section.PageSetup.Margins.Left -
                                            section.PageSetup.Margins.Right;

                                var currentMarkerStatus = MarkerStatusTrigger.CurrentStatus;
                                MarkerStatusTrigger.RaiseMarkerStatusUpdated(MarkerStatus.Hidden);

                                try
                                {
                                    foreach (var diagram in diagrams)
                                    {
                                        var panel = new ModelPanel();
                                        panel.SetDiagram(diagram);
                                        var bitmap = panel.GetBitmap();

                                        var newParagraph = new WParagraph(doc);
                                        newParagraph.Text = diagram.Name;
                                        newParagraph.ApplyStyle(style);
                                        section.Body.ChildEntities.Insert(index, newParagraph);
                                        index++;

                                        var imageParagraph = new WParagraph(doc);
                                        var picture = imageParagraph.AppendPicture(bitmap);
                                        var height = picture.Height * width / picture.Width;
                                        picture.Width = width;
                                        picture.Height = height;
                                        section.Body.ChildEntities.Insert(index, imageParagraph);
                                        index++;
                                        var caption = new WParagraph(doc);
                                        section.Body.ChildEntities.Insert(index, caption);
                                        caption.ApplyStyle(BuiltinStyle.Caption);
                                        caption.AppendText("Figure ");
                                        var field = caption.AppendField("Figure", FieldType.FieldSequence);
                                        field.Text = "X";
                                        caption.AppendText($" - The '{diagram.Name}' scenario.");
                                        index++;

                                        if (!string.IsNullOrWhiteSpace(diagram.Description))
                                        {
                                            var descParagraph = new WParagraph(doc);
                                            descParagraph.Text = diagram.Description;
                                            section.Body.ChildEntities.Insert(index, descParagraph);
                                            index++;
                                        }
                                    }
                                }
                                finally
                                {
                                    MarkerStatusTrigger.RaiseMarkerStatusUpdated(currentMarkerStatus);
                                }
                            }
                        }
                    }
                }
            }
        }

        private WSection GetSection([NotNull] Entity entity)
        {
            WSection result = null;

            if (entity is WSection section)
                result = section;
            else if (entity is TextBodyItem textBodyItem)
                result = GetSection(textBodyItem.OwnerTextBody.Owner);
            else
                result = GetSection(entity.Owner);

            return result;
        }
        #endregion

        #region Get information out of the Threat Model.
        private IEnumerable<IIdentity> GetItems(ItemType type)
        {
            IEnumerable<IIdentity> result = null;

            switch (type)
            {
                case ItemType.ExternalInteractor:
                    result = _model.Entities?.Where(x => x is IExternalInteractor).OrderBy(x => x.Name).ToArray();
                    break;
                case ItemType.Process:
                    result = _model.Entities?.Where(x => x is IProcess).OrderBy(x => x.Name).ToArray();
                    break;
                case ItemType.DataStore:
                    result = _model.Entities?.Where(x => x is IDataStore).OrderBy(x => x.Name).ToArray();
                    break;
                case ItemType.DataFlow:
                    result = _model.DataFlows?.OrderBy(x => x.Name).ToArray();
                    break;
                case ItemType.TrustBoundary:
                    result = _model.Groups?.OfType<ITrustBoundary>().OrderBy(x => x.Name).ToArray();
                    break;
                case ItemType.ThreatType:
                    result = GetThreatTypes();
                    break;
                case ItemType.ThreatEvent:
                    result = GetThreatEvents();
                    break;
                case ItemType.Mitigation:
                    result = _model.Mitigations?.Where(HasThreatEvents).OrderBy(x => x.Name).ToArray();
                    break;
            }

            return result;
        }

        private IEnumerable<IThreatType> GetThreatTypes()
        {
            return _model?.ThreatTypes?
                        .Where(x => (_model?.Entities?.Any(y =>
                                         y.ThreatEvents?.Any(z => z.ThreatTypeId == x.Id) ?? false) ?? false) ||
                                    (_model?.DataFlows?.Any(y =>
                                         y.ThreatEvents?.Any(z => z.ThreatTypeId == x.Id) ?? false) ?? false) ||
                                    (_model?.ThreatEvents?.Any(z => z.ThreatTypeId == x.Id) ?? false))
                        .OrderByDescending(GetTopSeverity, new SeverityComparer()).ToArray();;
        }

        private IEnumerable<IThreatEventMitigation> GetAssociatedMitigations([NotNull] IThreatType threatType)
        {
            var result = new List<IThreatEventMitigation>();

            var em = GetAssociatedMitigations(_model?.Entities?
                .Select(x => x.ThreatEvents?.FirstOrDefault(y => y.ThreatTypeId == threatType.Id))
                .Where(x => x != null))?.ToArray();
            if (em?.Any() ?? false)
                result.AddRange(em);

            var fm = GetAssociatedMitigations(_model?.DataFlows?
                .Select(x => x.ThreatEvents?.FirstOrDefault(y => y.ThreatTypeId == threatType.Id))
                .Where(x => x != null))?.ToArray();
            if (fm?.Any() ?? false)
                result.AddRange(fm);

            var tm = GetAssociatedMitigations(_model?.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id))?
                .ToArray();
            if (tm?.Any() ?? false)
                result.AddRange(tm);

            return result;
        }

        private IEnumerable<IThreatEventMitigation> GetAssociatedMitigations(IEnumerable<IThreatEvent> threatEvents)
        {
            IEnumerable<IThreatEventMitigation> result = null;

            if (threatEvents?.Any() ?? false)
            {
                var list = new List<IThreatEventMitigation>();

                foreach (var item in threatEvents)
                {
                    var m = item.Mitigations?.ToArray();
                    if (m?.Any() ?? false)
                    {
                        list.AddRange(m);
                    }
                }

                if (list.Count > 0)
                    result = list.ToArray();
            }

            return result;
        }

        private IEnumerable<IThreatEventMitigation> GetAssociatedThreats([NotNull] IMitigation mitigation, bool onlyActive = false)
        {
            List<IThreatEventMitigation> result = new List<IThreatEventMitigation>();

            var entities = _model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    AddThreatEventMitigationsToList(mitigation, entity, result, onlyActive);
                }
            }

            var dataFlows = _model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    AddThreatEventMitigationsToList(mitigation, dataFlow, result, onlyActive);
                }
            }

            AddThreatEventMitigationsToList(mitigation, _model, result, onlyActive);

            return result;
        }

        private static void AddThreatEventMitigationsToList(IMitigation referenceMitigation, 
            IThreatEventsContainer container, List<IThreatEventMitigation> list, bool onlyActive)
        {
            var threatEvents = container.ThreatEvents?
                .Where(x => x.Mitigations?
                    .Any(y => y.MitigationId == referenceMitigation.Id && 
                              (!onlyActive || y.Status == MitigationStatus.Approved || y.Status == MitigationStatus.Planned || y.Status == MitigationStatus.Proposed)) ?? false)
                .ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    list.AddRange(threatEvent.Mitigations.Where(x => x.MitigationId == referenceMitigation.Id));
                }
            }
        }

        private ISeverity GetTopSeverity([NotNull] IThreatType threatType)
        {
            var comparer = new SeverityComparer();

            var entitySeverity = _model?.Entities?
                .Where(x => x.ThreatEvents?.Any() ?? false)
                .Select(x => x.ThreatEvents?.Where(y => y.ThreatTypeId == threatType.Id)
                    .OrderByDescending(z => z.Severity, comparer).FirstOrDefault())
                .Where(x => x != null)
                .OrderByDescending(x => x.Severity, comparer)
                .FirstOrDefault()?.Severity;
            var dataFlowSeverity = _model?.DataFlows?
                .Where(x => x.ThreatEvents?.Any() ?? false)
                .Select(x => x.ThreatEvents?.Where(y => y.ThreatTypeId == threatType.Id)
                    .OrderByDescending(z => z.Severity, comparer).FirstOrDefault())
                .Where(x => x != null)
                .OrderByDescending(x => x.Severity, comparer)
                .FirstOrDefault()?.Severity;
            var threatModelSeverity = _model?.ThreatEvents?
                .Where(x => x.ThreatTypeId == threatType.Id)
                .OrderByDescending(y => y.Severity, comparer)
                .FirstOrDefault()?.Severity;

            ISeverity result = null;

            if (comparer.Compare(entitySeverity, dataFlowSeverity) > 0)
            {
                if (comparer.Compare(entitySeverity, threatModelSeverity) > 0)
                    result = entitySeverity;
                else
                    result = threatModelSeverity;
            } else if (comparer.Compare(dataFlowSeverity, threatModelSeverity) > 0)
                result = dataFlowSeverity;
            else
                result = threatModelSeverity;

            return result;
        }

        private IEnumerable<IThreatEvent> GetThreatEvents()
        {
            var list = new List<IThreatEvent>();
            if (_model.Entities?.Any() ?? false)
            {
                foreach (var entity in _model.Entities)
                {
                    var eTe = entity.ThreatEvents?.ToArray(); 
                    if (eTe?.Any() ?? false)
                        list.AddRange(eTe);
                }
            }

            if (_model.DataFlows?.Any() ?? false)
            {
                foreach (var flow in _model.DataFlows)
                {
                    var eF = flow.ThreatEvents?.ToArray();
                    if (eF?.Any() ?? false)
                        list.AddRange(eF);
                }
            }

            var modelThreatEvents = _model.ThreatEvents?.ToArray();
            if (modelThreatEvents?.Any() ?? false)
            {
                list.AddRange(modelThreatEvents);
            }

            return list.OrderByDescending(x => x.Severity, new SeverityComparer()).ToArray();
        }

        private bool HasThreatEvents([NotNull] IMitigation mitigation)
        {
            bool result = false;

            var entities = _model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    result = entity.ThreatEvents?
                        .Any(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false) ?? false;
                    if (result)
                        break;
                }
            }

            if (!result)
            {
                var dataFlows = _model.DataFlows?.ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        result = dataFlow.ThreatEvents?
                                     .Any(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false) ?? false;
                        if (result)
                            break;
                    }
                }
            }

            if (!result)
            {
                result = _model.ThreatEvents?
                             .Any(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false) ?? false;
            }

            return result;
        }
        #endregion
    }
}