using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Excel;
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
                            var threats = AnalyzeThreatModel(threatModel);
                            if (threats?.Any() ?? false)
                            {
                                if (CreateReport(dialog.FileName, threats))
                                    ShowMessage?.Invoke("Summary Excel Report generation succeeded.");
                            }
                            else
                            {
                                ShowWarning?.Invoke("Summary Excel Report failed because no Threat Events are defined.");
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

        private bool CreateReport([Required] string fileName, [NotNull] Dictionary<IThreatType, List<IThreatEvent>> threats)
        {
            var result = false;

            using (var engine = new ExcelReportEngine())
            {
                var page = engine.AddPage("Report");
                List<string> fields = new List<string> {"Name", "Severity", "Description", "Affected Objects"};
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

                try
                {
                    engine.Save(fileName);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    ShowWarning?.Invoke(e.Message);
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
                        builder.AppendLine($"[{model.GetIdentityTypeInitial(threat.Parent)}] {threat.Parent.Name} ({threat.Severity.ToString()})");
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
                            builder.AppendLine($"[{model.GetIdentityTypeInitial(m.ThreatEvent.Parent)}] {m.ThreatEvent.Parent}: {m.Mitigation.Name}");
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
