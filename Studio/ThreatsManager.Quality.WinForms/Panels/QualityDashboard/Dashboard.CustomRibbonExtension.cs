using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Dialogs;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
#pragma warning disable CS0067
    public partial class Dashboard
    {
        private readonly Guid _id = Guid.NewGuid();
        private float _minTextHeight;
        
        public event Action<string, bool> ChangeCustomActionStatus;

        public Guid Id => _id;
        public string TabLabel => "Quality Dashboard";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Reports", "Quality Reports", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Pdf", "Generate PDF Report",
                            Properties.Resources.pdf_big,
                            Properties.Resources.pdf,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "Excel", "Generate Excel Report",
                            Properties.Resources.xlsx_big,
                            Properties.Resources.xlsx,
                            true, Shortcut.None),
                    }),
                    new CommandsBarDefinition("FalsePositives", "False Positives", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "FalsePositives", "False Positive\nList",
                            Properties.Resources.hand_thumb_up_big,
                            Properties.Resources.hand_thumb_up,
                            true, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh List",
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
                    case "Pdf":
                        var dialog = new SaveFileDialog();
                        dialog.DefaultExt = "pdf";
                        dialog.Filter = "Acrobat file (*.pdf)|*.pdf|All files (*.*)|*.*";
                        dialog.Title = "Save Quality Report";
                        dialog.RestoreDirectory = true;
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            if (GeneratePdf(dialog.FileName))
                            {
                                text = "PDF Quality Report creation";
                                Process.Start(dialog.FileName);
                            }                            
                            else
                            {
                                text = "PDF Quality Report creation has failed.";
                                warning = true;
                            }
                        }
                        break;
                    case "Excel":
                        var dialog2 = new SaveFileDialog();
                        dialog2.DefaultExt = "xlsx";
                        dialog2.Filter = "Excel 2016 files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        dialog2.Title = "Save Quality Report";
                        dialog2.RestoreDirectory = true;
                        if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            if (GenerateXlsx(dialog2.FileName))
                            {
                                text = "Excel Quality Report creation";
                                Process.Start(dialog2.FileName);
                            }                            
                            else
                            {
                                text = "Excel Quality Report creation has failed.";
                                warning = true;
                            }
                        }
                        break;
                    case "FalsePositives":
                        var dialog3 = new FalsePositivesListDialog(_model);
                        dialog3.ShowDialog();
                        if (dialog3.StatusChanged)
                            Analyze();
                        break;
                    case "Refresh":
                        Analyze();
                        text = "Quality Analysis";
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

        #region PDF
        private bool GeneratePdf([Required] string fileName)
        {
            using (var doc = new PdfDocument(PdfConformanceLevel.None))
            {
                doc.PageSettings.Orientation = PdfPageOrientation.Portrait;
                var page = doc.Pages.Add();
                var y = AddPdfHeader(page);

                var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                var minSize = font.MeasureString("Ag");
                _minTextHeight = minSize.Height;
                var healthIndex = _analyzersManager.Analyze(_model, 
                    QualityPropertySchemaManager.IsFalsePositive,
                    out var outcomes);
                AddSummary(doc, page, y + 10, font, healthIndex, outcomes);

                var analyzers = QualityAnalyzersManager.QualityAnalyzers?.ToArray();
                if (analyzers?.Any() ?? false)
                {
                    foreach (var analyzer in analyzers)
                    {
                        var outcome = outcomes?.FirstOrDefault(x => x.Id == analyzer.GetExtensionId());
                        if (outcome != null)
                            AddOutcomePage(doc, font, analyzer, outcome);
                    }
                }

                var schemaManager = new AnnotationsPropertySchemaManager(_model);
                var propertyType = schemaManager.GetAnnotationsPropertyType();
                var containers = GetContainers(schemaManager, propertyType)?.ToArray();
                if (containers?.Any() ?? false)
                {
                    AddReviewNotesPage(doc, font, containers, schemaManager, propertyType);
                }

                AddFooters(doc, font);

                doc.Save(fileName);
            }

            return true;
        }

        private float AddPdfHeader([NotNull] PdfPage page)
        {
            var g = page.Graphics;
            var header1 = WriteText(page, "Threats Manager Platform",
                new PdfStandardFont(PdfFontFamily.Helvetica, 12), 0, 0);
            var header2 = WriteText(page, "Threat Model Quality Report",
                new PdfStandardFont(PdfFontFamily.Helvetica, 16), 0, header1.Height + 2);

            var img = PdfImage.FromImage(Properties.Resources.logo);
            g.DrawImage(img, new RectangleF(g.ClientSize.Width - 64, 0, 64, 64));
            var subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
            var y = header1.Height + header2.Height;
            g.DrawRectangle(new PdfSolidBrush(new PdfColor(0x01, 0x73, 0xC7)), 
                new RectangleF(0, y + 40, g.ClientSize.Width, 30));

            WriteText(page, _model.Name, subHeadingFont, 
                10, y + 48, PdfBrushes.White, false, (g.ClientSize.Width - 20) * 0.75f, false);
            WriteText(page, $"Date: {DateTime.Now.ToString("d")}", subHeadingFont,
                10, y + 48, PdfBrushes.White, true, (g.ClientSize.Width - 20) * 0.25f);

            return y + 70;
        }

        private void AddSummary([NotNull] PdfDocument doc, [NotNull] PdfPage page, float top, 
            PdfStandardFont font, double healthIndex, 
            IEnumerable<QualityAnalyzerResult> outcomes)
        {
            WriteText(page, "Threat Model Health Index",
                new PdfStandardFont(font, font.Size, PdfFontStyle.Bold), 0, top);
            var textSize = WriteText(page,
                $"{QualityAnalyzersManager.GetHealthIndexDescription(healthIndex)} ({healthIndex.ToString("F1")}/30)",
                new PdfStandardFont(font, font.Size, PdfFontStyle.Bold), 0, top, null, true);

            var y = textSize.Height + top + 10;

            page.Graphics.DrawLine(PdfPens.Black, 40, y, page.Graphics.ClientSize.Width - 40, y);

            y += 10;

            var analyzers = QualityAnalyzersManager.QualityAnalyzers?.ToArray();
            if (analyzers?.Any() ?? false)
            {
                foreach (var analyzer in analyzers)
                {
                    if (y > page.Graphics.ClientSize.Height - textSize.Height * 4)
                    {
                        y = 0;
                        page = doc.Pages.Add();
                    }

                    var outcome = outcomes?.FirstOrDefault(x => x.Id == analyzer.GetExtensionId());
                    if (outcome != null)
                    {
                        WriteText(page, analyzer.Label, font, 0, y);
                        textSize = WriteText(page, outcome.Assessment.GetEnumLabel(),
                            font, 0, y, null, true);
                        y += textSize.Height + 10;
                    }
                }
            }
        }

        private void AddOutcomePage([NotNull] PdfDocument doc, [NotNull] PdfStandardFont font, 
            [NotNull] IQualityAnalyzer analyzer, [NotNull] QualityAnalyzerResult outcome)
        {
            var page = doc.Pages.Add();

            var bitmap = GetBitmap(analyzer);

            var textSize = WriteText(page, analyzer.Label, new PdfStandardFont(font, font.Size, PdfFontStyle.Bold), 0, 0);
            textSize = WriteText(page, $"{outcome.Assessment.GetEnumLabel()}", font, 0, 0, null, true);
            var gaugeImage = PdfImage.FromImage(bitmap);
            page.Graphics.DrawImage(gaugeImage, new RectangleF(page.Graphics.ClientSize.Width - 150 - textSize.Width, -2, 150, 18));
            var y = textSize.Height + 2;

            textSize = WriteText(page, analyzer.Description, font, 0, y);
            y += textSize.Height + 10;
            page.Graphics.DrawLine(PdfPens.Black, 0, y, page.Graphics.ClientSize.Width, y);
            y += 10;

            var findings = outcome.Findings?.ToArray();
            if (findings?.Any() ?? false)
            {
                PdfImage img;
                string text;
                foreach (var finding in findings)
                {
                    if (y > page.Graphics.ClientSize.Height - textSize.Height * 5)
                    {
                        y = 0;
                        page = doc.Pages.Add();
                    }

                    text = null;
                    if (finding is IIdentity identity)
                    {
                        img = PdfImage.FromImage(identity.GetImage(ImageSize.Medium));
                        if (identity is IDataFlow flow)
                        {
                            text =
                                $"Flow from {_model.GetIdentityTypeName(flow.Source)} '{flow.Source.Name}' to {_model.GetIdentityTypeName(flow.Target)} '{flow.Target.Name}'.";
                        }
                        else if (identity is IThreatEvent threatEvent)
                        {
                            text =
                                $"Threat Event applied to {_model.GetIdentityTypeName(threatEvent.Parent)} '{threatEvent.Parent.Name}'.";
                        }
                    }
                    else
                    {
                        img = PdfImage.FromImage(Resources.undefined);
                    }

                    page.Graphics.DrawImage(img, new RectangleF(0, y, 16, 16));
                    textSize = WriteText(page, finding.ToString(), font, 18, y + 2);
                    if (text != null)
                    {
                        y += textSize.Height + 5;
                        textSize = WriteText(page, text, font, 18, y);
                    }

                    y += textSize.Height + 10;
                }
            }
            else
            {
                WriteText(page, "--- The Analyzer has not identified any problem.", font, 0, y);
            }
        }

        private void AddReviewNotesPage([NotNull] PdfDocument doc, [NotNull] PdfStandardFont font, 
            [NotNull] IEnumerable<IPropertiesContainer> containers, [NotNull] AnnotationsPropertySchemaManager schemaManager, 
            [NotNull] IPropertyType propertyType)
        {
            string objectType = null;
            string name = null;
            string text = null;
            Bitmap image = null;
            var y = 0f;
            PdfPage page = null;
            var textSize = SizeF.Empty;

            foreach (var container in containers)
            {
                if (container is IDataFlow flow)
                {
                    objectType = _model.GetIdentityTypeName(flow);
                    name = flow.Name;
                    text = $"From {_model.GetIdentityTypeName(flow.Source)} '{flow.Source.Name}' to {_model.GetIdentityTypeName(flow.Target)} '{flow.Target.Name}'";
                    image = flow.GetImage(ImageSize.Medium);
                }
                else if (container is IThreatEvent threatEvent)
                {
                    objectType = _model.GetIdentityTypeName(threatEvent);
                    name = threatEvent.Name;
                    text = $"Applied to {_model.GetIdentityTypeName(threatEvent.Parent)} '{threatEvent.Parent.Name}'";
                    image = threatEvent.GetImage(ImageSize.Medium);
                }
                else if (container is IIdentity identity)
                {
                    objectType = _model.GetIdentityTypeName(identity);
                    name = identity.Name;
                    text = null;
                    image = identity.GetImage(ImageSize.Medium);
                }
                else if (container is IThreatEventMitigation threatEventMitigation)
                {
                    objectType = "Threat Event Mitigation";
                    name = threatEventMitigation.Mitigation.Name;
                    text = $"Associated to Threat Event '{threatEventMitigation.ThreatEvent.Name}' on '{threatEventMitigation.ThreatEvent.Parent.Name}'";
                    image = Icons.Resources.mitigations;
                }
                else if (container is IThreatTypeMitigation threatTypeMitigation)
                {
                    objectType = "Threat Type Mitigation";
                    name = threatTypeMitigation.Mitigation.Name;
                    text = $"Associated to Threat Type '{threatTypeMitigation.ThreatType.Name}'";
                    image = Icons.Resources.standard_mitigations;
                }
                else
                {
                    objectType = null;
                    name = null;
                    text = null;
                    image = null;
                }

                var reviewNotes = schemaManager.GetAnnotations(container)?.OfType<ReviewNote>().ToArray();
                if (reviewNotes?.Any() ?? false)
                {
                    PdfImage img;
                    foreach (var reviewNote in reviewNotes)
                    { 
                        if (y == 0 || (y > page.Graphics.ClientSize.Height - textSize.Height * 10))
                        {
                            y = 0;
                            page = doc.Pages.Add();

                            textSize = WriteText(page, "Review Notes", new PdfStandardFont(font, font.Size, PdfFontStyle.Bold), 0, 0);
                            y = textSize.Height + 10;
                            page.Graphics.DrawLine(PdfPens.Black, 0, y, page.Graphics.ClientSize.Width, y);
                            y += 10;

                        }

                        if (image != null && objectType != null && name != null)
                        {
                            img = PdfImage.FromImage(image);
                            page.Graphics.DrawImage(img, new RectangleF(0, y, 16, 16));
                            textSize = WriteText(page, $"{objectType}: {name}", font, 18, y + 2);
                            if (text != null)
                            {
                                y += textSize.Height + 5;
                                textSize = WriteText(page, text, font, 18, y);
                            }
                            y += textSize.Height + 5;
                            textSize = WriteText(page, reviewNote.Text, font, 18, y);
                            y += textSize.Height + 5;
                            textSize = WriteText(page, $"Note created by {reviewNote.CreatedBy} on {reviewNote.CreatedOn.ToShortDateString()} and modified by {reviewNote.ModifiedBy} on {reviewNote.ModifiedOn.ToShortDateString()}", font, 18, y);

                            y += textSize.Height + 10;
                        }
                    }
                }
            }
        }

        private void AddFooters([NotNull] PdfDocument doc, [NotNull] PdfStandardFont font)
        {
            var pages = doc.Pages.OfType<PdfPage>().ToArray();

            var textSize = font.MeasureString(_model.Name);

            int i = 1;
            foreach (var page in pages)
            {
                var y = page.Graphics.ClientSize.Height - textSize.Height - 5;
                page.Graphics.DrawLine(PdfPens.Black, 0, y, page.Graphics.ClientSize.Width, y);
                WriteText(page, _model.Name, font, 0, y + 5);
                WriteText(page, $"Page {i}", font, 0, y + 5, null, true);
                i++;
            }
        }

        private SizeF WriteText([NotNull] PdfPage page, [Required] string text, 
            PdfStandardFont font, float x, float y, PdfBrush brush = null, 
            bool alignRight = false, float maxWidth = 0, bool wordWrap = true)
        {
            if (maxWidth == 0)
                maxWidth = page.Graphics.ClientSize.Width;
            var format = new PdfStringFormat();
            if (wordWrap)
                format.WordWrap = PdfWordWrapType.Word;
            else
                format.WordWrap = PdfWordWrapType.None;

            var result = font.MeasureString(text, maxWidth, format);

            float newX;
            if (alignRight)
                newX = page.Graphics.ClientSize.Width - result.Width - x;
            else
                newX = x;

            page.Graphics.DrawString(text, font, brush ?? PdfBrushes.Black, 
                new RectangleF(newX, y, result.Width, result.Height), format);

            if (result.Height < _minTextHeight)
                result.Height = _minTextHeight;

            return result;
        }

        private Bitmap GetBitmap([NotNull] IQualityAnalyzer analyzer)
        {
            Bitmap result = null;

            var controls = _container.Controls.OfType<CheckPanel>().ToArray();
            if (controls.Any())
            {
                foreach (var control in controls)
                {
                    if (control.Id == analyzer.GetExtensionId())
                    {
                        result = control.GetGaugeBitmap();
                    }
                }
            }

            return result;
        }

        private IEnumerable<IPropertiesContainer> GetContainers([NotNull] AnnotationsPropertySchemaManager schemaManager, 
            [NotNull] IPropertyType propertyType)
        {
            var list = new List<IPropertiesContainer>();
            Add(list, _model.GetExternalInteractors(schemaManager, propertyType, null));
            Add(list, _model.GetProcesses(schemaManager, propertyType, null));
            Add(list, _model.GetDataStores(schemaManager, propertyType, null));
            Add(list, _model.GetFlows(schemaManager, propertyType, null));
            Add(list, _model.GetTrustBoundaries(schemaManager, propertyType, null));
            Add(list, _model.GetThreatEvents(schemaManager, propertyType, null));
            Add(list, _model.GetThreatEventMitigations(schemaManager, propertyType, null));
            Add(list, _model.GetThreatTypes(schemaManager, propertyType, null));
            Add(list, _model.GetKnownMitigations(schemaManager, propertyType, null));
            Add(list, _model.GetStandardMitigations(schemaManager, propertyType, null));
            Add(list, _model.GetEntityTemplates(schemaManager, propertyType, null));
            Add(list, _model.GetFlowTemplates(schemaManager, propertyType, null));
            Add(list, _model.GetTrustBoundaryTemplates(schemaManager, propertyType, null));
            Add(list, _model.GetDiagrams(schemaManager, propertyType, null));

            return list;
        }

        private void Add([NotNull] List<IPropertiesContainer> list, IEnumerable<IPropertiesContainer> containers)
        {
            var items = containers?.ToArray();
            if (items?.Any() ?? false)
            {
                list.AddRange(items);
            }
        }
        #endregion

        #region Excel

        private bool GenerateXlsx([Required] string fileName)
        {
            bool result = false;

            var healthIndex = _analyzersManager.Analyze(_model, 
                QualityPropertySchemaManager.IsFalsePositive,
                out var outcomes);

            using (var engine = new ExcelReportEngine())
            {
                CreateSummary(engine, healthIndex, outcomes);

                var analyzers = QualityAnalyzersManager.QualityAnalyzers?.ToArray();
                if (analyzers?.Any() ?? false)
                {
                    foreach (var analyzer in analyzers)
                    {
                        var outcome = outcomes?.FirstOrDefault(x => x.Id == analyzer.GetExtensionId());
                        if (outcome != null)
                            CreateOutcomePage(engine, analyzer, outcome);
                    }
                }

                engine.Save(fileName);
                result = true;
            }

            return result;
        }

        private void CreateSummary([NotNull] ExcelReportEngine engine, 
            double healthIndex, 
            IEnumerable<QualityAnalyzerResult> outcomes)
        {
            var p = engine.AddPage("Summary");
            engine.AddHeader(p, "Assessment", "Evaluation");
            
            engine.AddRow(p, "Threat Model Health Index", QualityAnalyzersManager.GetHealthIndexDescription(healthIndex));
            
            var analyzers = QualityAnalyzersManager.QualityAnalyzers?.ToArray();
            if (analyzers?.Any() ?? false)
            {
                foreach (var analyzer in analyzers)
                {
                    var outcome = outcomes?.FirstOrDefault(x => x.Id == analyzer.GetExtensionId());
                    if (outcome != null)
                    {
                        engine.AddRow(p, analyzer.Label, outcome.Assessment.GetEnumLabel());
                    }
                }
            }
        }
 
        private void CreateOutcomePage([NotNull] ExcelReportEngine engine,
            [NotNull] IQualityAnalyzer analyzer, [NotNull] QualityAnalyzerResult outcome)
        {
            var p = engine.AddPage(analyzer.Label);

            engine.AddHeader(p, "Finding Type", "Finding Name", "Description");

            var findings = outcome.Findings?.ToArray();
            if (findings?.Any() ?? false)
            {
                string text;
                foreach (var finding in findings)
                {
                    if (finding is IIdentity identity)
                    {
                        if (identity is IDataFlow flow)
                        {
                            text = $"Flow from {_model.GetIdentityTypeName(flow.Source)} '{flow.Source.Name}' to {_model.GetIdentityTypeName(flow.Target)} '{flow.Target.Name}'.";
                        }
                        else if (identity is IThreatEvent threatEvent)
                        {
                            text = $"Threat Event applied to {_model.GetIdentityTypeName(threatEvent.Parent)} '{threatEvent.Parent.Name}'.";
                        }
                        else
                        {
                            text = null;
                        }

                        engine.AddRow(p, 
                            _model.GetIdentityTypeName(identity),
                            identity.Name, text);
                    }
                    else
                    {
                        engine.AddRow(p, finding.GetType().Name, finding.ToString());
                    }
                }
            }
        }
       #endregion
    }
}