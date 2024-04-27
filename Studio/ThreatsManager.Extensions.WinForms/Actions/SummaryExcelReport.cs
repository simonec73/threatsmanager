using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Excel;
using ThreatsManager.Extensions.Reporting;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
#pragma warning disable CS0067
    [Extension("998BE29E-C537-471C-8734-B1654D71BB1A", "Summary Excel Report Action", 26, ExecutionMode.Business)]
    public class SummaryExcelReport : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IPanelFactory> ClosePanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Export;
        public string Bar => "Reporting";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "SummaryExcel", "Summary Excel Report", Properties.Resources.xlsx_big,
                Properties.Resources.xlsx)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "SummaryExcel":
                        var dialog = new SaveFileDialog();
                        dialog.DefaultExt = "xlsx";
                        dialog.Filter = "Excel 2016 files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        dialog.Title = "Save Summary Excel Report";
                        dialog.RestoreDirectory = true;
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            if (CreateReport(dialog.FileName, threatModel))
                            {
                                ShowMessage?.Invoke("Summary Excel Report generation succeeded.");
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                ShowWarning?.Invoke("Summary Excel Report failed.");
            }
        }

        private void AddThreatEvents(IEnumerable<IThreatEvent> threatEvents,
            ref Dictionary<IThreatType, List<IThreatEvent>> dict)
        {
            var tes = threatEvents?.ToArray();
            if (tes?.Any() ?? false)
            {
                foreach (var te in tes)
                {
                    if (dict == null)
                        dict = new Dictionary<IThreatType, List<IThreatEvent>>();

                    if (!dict.TryGetValue(te.ThreatType, out var teList))
                    {
                        teList = new List<IThreatEvent>();
                        dict.Add(te.ThreatType, teList);
                    }

                    teList.Add(te);
                }
            }
        }

        private bool CreateReport([Required] string fileName, [NotNull] IThreatModel model)
        {
            var result = false;

            var threats = AnalyzeThreatModel(model);

            using (var engine = new ExcelReportEngine())
            {
                if (threats?.Any() ?? false)
                {
                    AddStandardSheets(engine, threats);
                    result = true;
                }

                var providers = ExtensionUtils.GetExtensions<ISummarySheetProvider>()?.ToArray();
                if (providers?.Any() ?? false)
                {
                    foreach (var provider in providers)
                    {
                        result |= AddProviderSheet(engine, model, provider);
                    }
                }

                if (result)
                {
                    try
                    {
                        engine.Save(fileName);
                    }
                    catch (System.IO.IOException e)
                    {
                        result = false;
                        ShowWarning?.Invoke(e.Message);
                    }
                }
                else
                {
                    ShowWarning?.Invoke("Summary Excel Report has not been generated because it is empty.");
                }
            }

            return result;
        }

        private Dictionary<IThreatType, List<IThreatEvent>> AnalyzeThreatModel([NotNull] IThreatModel model)
        {
            Dictionary<IThreatType, List<IThreatEvent>> result = null;

            AddThreatEvents(model.ThreatEvents, ref result);

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    AddThreatEvents(entity.ThreatEvents, ref result);
                }
            }

            var flows = model.DataFlows?.ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    AddThreatEvents(flow.ThreatEvents, ref result);
                }
            }

            return result;
        }

        private void AddStandardSheets([NotNull] ExcelReportEngine engine, 
            [NotNull] Dictionary<IThreatType, List<IThreatEvent>> threats)
        {
            var page = engine.AddPage("Report");
            List<string> fields = new List<string> { "Name", "Severity", "Description", "Affected Objects" };
            var existing = HasMitigations(threats, MitigationStatus.Existing);
            if (existing)
                fields.Add("Existing Mitigations");
            var approved = HasMitigations(threats, MitigationStatus.Approved);
            if (approved)
                fields.Add("Approved Mitigations");
            var planned = HasMitigations(threats, MitigationStatus.Planned);
            if (planned)
                fields.Add("Planned Mitigations");
            var implemented = HasMitigations(threats, MitigationStatus.Implemented);
            if (implemented)
                fields.Add("Implemented Mitigations");
            var proposed = HasMitigations(threats, MitigationStatus.Proposed);
            if (proposed)
                fields.Add("Proposed Mitigations");
            engine.AddHeader(page, fields.ToArray());

            var sorted = threats.OrderByDescending(x => GetTopSeverity(x.Value), new SeverityComparer())
                .ThenBy(x => x.Key.Name);
            List<object> values = new List<object>();
            foreach (var threat in sorted)
            {
                values.Clear();
                values.Add(threat.Key.Name);
                var severity = GetTopSeverity(threat.Value);
                values.Add(severity?.ToString() ?? string.Empty);
                values.Add(threat.Key.Description);
                values.Add(GetAffectedObjects(threat.Value));
                if (existing && threat.Value.Any())
                {
                    values.Add(ConcatenateMitigations(threat, values, MitigationStatus.Existing) ?? string.Empty);
                }

                if (approved && threat.Value.Any())
                {
                    values.Add(ConcatenateMitigations(threat, values, MitigationStatus.Approved) ?? string.Empty);
                }

                if (planned && threat.Value.Any())
                {
                    values.Add(ConcatenateMitigations(threat, values, MitigationStatus.Planned) ?? string.Empty);
                }

                if (implemented && threat.Value.Any())
                {
                    values.Add(ConcatenateMitigations(threat, values, MitigationStatus.Implemented) ??
                               string.Empty);
                }

                if (proposed && threat.Value.Any())
                {
                    values.Add(ConcatenateMitigations(threat, values, MitigationStatus.Proposed) ?? string.Empty);
                }

                var row = engine.AddRow(page, values.ToArray());
                engine.ColorCell(page, row, 2, Color.FromKnownColor(severity.TextColor),
                    Color.FromKnownColor(severity.BackColor));
            }
        }

        private bool AddProviderSheet([NotNull] ExcelReportEngine engine, [NotNull] IThreatModel model,
            [NotNull] ISummarySheetProvider provider)
        {
            bool result = false;

            var rows = provider.GetRows(model)?.ToArray();
            if (CheckRows(rows))
            {
                result = true;

                var page = engine.AddPage(provider.Name);
                bool first = true;

                foreach (var row in rows)
                {
                    if (first)
                    {
                        engine.AddHeader(page, row.ToArray());
                        first = false;
                    }
                    else
                    {
                        engine.AddRow(page, row.ToArray());
                    }
                }
            }

            return result;
        }
        
        private bool CheckRows(IEnumerable<string>[] rows)
        {
            var result = false;

            if ((rows?.Count() ?? 0) > 1)
            {
                int count = 0;
                result = true;

                foreach (var row in rows)
                {
                    if (count == 0)
                        count = row.Count();
                    else if (count != row.Count())
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private string GetAffectedObjects(List<IThreatEvent> threats)
        {
            string result = null;

            if (threats?.Any() ?? false)
            {
                var builder = new StringBuilder();
                var model = threats[0].Model;

                if (model != null)
                {
                    foreach (var threat in threats)
                    {
                        if (threat.Parent != null)
                            builder.AppendLine($"[{threat.Parent.GetIdentityTypeInitial()}] {threat.Parent.Name} ({threat.Severity?.ToString() ?? ThreatModelManager.Unknown})");
                    }

                    result = builder.ToString();
                }
            }

            return result;
        }

        private ISeverity GetTopSeverity(List<IThreatEvent> threats)
        {
            ISeverity result = null;

            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    if (result == null)
                        result = threat.Severity;
                    else if (threat.SeverityId > result.Id)
                    {
                        result = threat.Severity;
                    }
                }
            }

            return result;
        }

        private static string ConcatenateMitigations(KeyValuePair<IThreatType, List<IThreatEvent>> threat, 
            List<object> values, MitigationStatus status)
        {
            string result = null;

            var model = threat.Key?.Model;
            if (model != null && (threat.Value?.Count ?? 0) > 0)
            {
                var builder = new StringBuilder();
                foreach (var te in threat.Value)
                {
                    var ms = te.Mitigations?.Where(x => x.Status == status).ToArray();
                    if (ms?.Any() ?? false)
                    {
                        foreach (var m in ms)
                        {
                            if (m.ThreatEvent?.Parent != null)
                                builder.AppendLine($"[{m.ThreatEvent.Parent.GetIdentityTypeInitial()}] {m.ThreatEvent.Parent}: {m.Mitigation.Name}");
                        }
                    }
                }

                result = builder.ToString();
            }

            return result;
        }

        private bool HasMitigations([NotNull] Dictionary<IThreatType, List<IThreatEvent>> threats,
            MitigationStatus status)
        {
            bool result = false;

            if (threats.Any())
            {
                foreach (var threat in threats)
                {
                    var tes = threat.Value;
                    if (tes.Any())
                    {
                        foreach (var te in tes)
                        {
                            var mitigations = te.Mitigations?.ToArray();
                            if (mitigations?.Any() ?? false)
                            {
                                foreach (var mitigation in mitigations)
                                {
                                    if (mitigation.Status == status)
                                    {
                                        result = true;
                                        break;
                                    }
                                }

                                if (result)
                                    break;
                            }
                        }

                        if (result)
                            break;
                    }
                }
            }

            return result;
        }
    }
}
