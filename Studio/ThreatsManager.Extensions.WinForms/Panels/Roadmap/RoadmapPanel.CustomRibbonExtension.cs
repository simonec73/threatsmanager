using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
#pragma warning disable CS0067
    public partial class RoadmapPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Roadmap";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_executionMode != ExecutionMode.Business)
                {
                    result.Add(new CommandsBarDefinition("ResidualRisk", "Risk", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ConfigureEstimator", "Configure Risk Estimator",
                            Properties.Resources.money_bill_fire_big,
                            Properties.Resources.money_bill_fire),
                    }));
                };

                result.Add(new CommandsBarDefinition("Filter", "Filter", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Filter", "Filter Mitigations",
                        Properties.Resources.funnel_big,
                        Properties.Resources.funnel),
                }));

                result.Add(new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "ExportCsv", "Export for Azure DevOps",
                        Properties.Resources.Azure_DevOps_big,
                        Properties.Resources.Azure_DevOps),
                    new ActionDefinition(Id, "ExportShortCsv", "Export Short Term for Azure DevOps",
                        Properties.Resources.Azure_DevOps_big_short_term,
                        Properties.Resources.Azure_DevOps_short_term),
                    new ActionDefinition(Id, "ExportMidCsv", "Export Medium Term for Azure DevOps",
                        Properties.Resources.Azure_DevOps_big_mid_term,
                        Properties.Resources.Azure_DevOps_mid_term),
                    new ActionDefinition(Id, "ExportLongCsv", "Export Long Term for Azure DevOps",
                        Properties.Resources.Azure_DevOps_big_long_term,
                        Properties.Resources.Azure_DevOps_long_term),
                }));

                if (_commandsBarContextAwareActions?.Any() ?? false)
                {
                    foreach (var definitions in _commandsBarContextAwareActions.Values)
                    {
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        result.Add(new CommandsBarDefinition(definitions[0].Name, definitions[0].Label, actions));
                    }
                }

                result.Add(new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Refresh", "Refresh List",
                        Resources.refresh_big,
                        Resources.refresh,
                        true, Shortcut.F5),
                }));


                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            //string text = null;
            //bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "ConfigureEstimator":
                        var dialog = new ResidualRiskEstimatorConfigurationDialog();
                        dialog.Initialize(_model);
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            LoadModel();
                        }
                        break;
                    case "Filter":
                        var dialogFilter = new RoadmapFilterDialog(_model);
                        dialogFilter.Filter = _filter;
                        if (dialogFilter.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            _filter = dialogFilter.Filter;
                            LoadModel();
                        }
                        break;
                    case "ExportCsv":
                        var saveFileDialog = new SaveFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = false,
                            CheckPathExists = true,
                            RestoreDirectory = true,
                            DefaultExt = "csv",
                            Filter = "CSV file (*.csv)|*.csv",
                            Title = "Create CSV file for Azure DevOps",
                            ValidateNames = true
                        };
                        if (saveFileDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            ExportCsv(saveFileDialog.FileName);
                        }
                        break;
                    case "ExportShortCsv":
                        var saveFileDialogShort = new SaveFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = false,
                            CheckPathExists = true,
                            RestoreDirectory = true,
                            DefaultExt = "csv",
                            Filter = "CSV file (*.csv)|*.csv",
                            Title = "Create CSV file for Azure DevOps",
                            ValidateNames = true
                        };
                        if (saveFileDialogShort.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            ExportCsv(saveFileDialogShort.FileName, RoadmapStatus.ShortTerm);
                        }
                        break;
                    case "ExportMidCsv":
                        var saveFileDialogMid = new SaveFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = false,
                            CheckPathExists = true,
                            RestoreDirectory = true,
                            DefaultExt = "csv",
                            Filter = "CSV file (*.csv)|*.csv",
                            Title = "Create CSV file for Azure DevOps",
                            ValidateNames = true
                        };
                        if (saveFileDialogMid.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            ExportCsv(saveFileDialogMid.FileName, RoadmapStatus.MidTerm);
                        }
                        break;
                    case "ExportLongCsv":
                        var saveFileDialogLong = new SaveFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = false,
                            CheckPathExists = true,
                            RestoreDirectory = true,
                            DefaultExt = "csv",
                            Filter = "CSV file (*.csv)|*.csv",
                            Title = "Create CSV file for Azure DevOps",
                            ValidateNames = true
                        };
                        if (saveFileDialogLong.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            ExportCsv(saveFileDialogLong.FileName, RoadmapStatus.LongTerm);
                        }
                        break;
                    case "Refresh":
                        LoadModel();
                        break;
                    default:
                        if (action.Tag is IIdentityContextAwareAction identityContextAwareAction)
                        {
                            if ((identityContextAwareAction.Scope & Scope.ThreatModel) != 0)
                            {
                                identityContextAwareAction.Execute(_model);
                            }
                        }
                        break;
                }

                //if (warning)
                //    ShowWarning?.Invoke(text);
                //else if (text != null)
                //    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                //ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        private void ExportCsv([Required] string fileName, RoadmapStatus requiredStatus = RoadmapStatus.NotAssessed)
        {
            var mitigations = _model?.GetUniqueMitigations()?.OrderBy(x => x.Name).ToArray();

            if (mitigations?.Any() ?? false)
            {
                using (var engine = new ExcelReportEngine())
                {
                    var page = engine.AddPage("Report");
                    List<string> fields = new List<string> {"Work Item Type", "Title", "Description", "State", "Priority"};
                    engine.AddHeader(page, fields.ToArray());

                    var schema = new RoadmapPropertySchemaManager(_model);

                    foreach (var mitigation in mitigations)
                    {
                        var status = mitigation.GetStatus();

                        switch (requiredStatus)
                        {
                            case RoadmapStatus.ShortTerm:
                                if (status == RoadmapStatus.ShortTerm)
                                {
                                    engine.AddRow(page, new[]
                                    {
                                        "Task", mitigation.Name, TextToHtml(mitigation.Description), "To Do", ((int)status).ToString()
                                    });
                                }
                                break;
                            case RoadmapStatus.MidTerm:
                                if (status == RoadmapStatus.MidTerm)
                                {
                                    engine.AddRow(page, new[]
                                    {
                                        "Task", mitigation.Name, TextToHtml(mitigation.Description), "To Do", ((int)status).ToString()
                                    });
                                }
                                break;
                            case RoadmapStatus.LongTerm:
                                if (status == RoadmapStatus.LongTerm)
                                {
                                    engine.AddRow(page, new[]
                                    {
                                        "Task", mitigation.Name, TextToHtml(mitigation.Description), "To Do", ((int)status).ToString()
                                    });
                                }
                                break;
                            default:
                                if (status == RoadmapStatus.ShortTerm || status == RoadmapStatus.MidTerm || status == RoadmapStatus.LongTerm)
                                {
                                    engine.AddRow(page, new[]
                                    {
                                        "Task", mitigation.Name, TextToHtml(mitigation.Description), "To Do", ((int)status).ToString()
                                    });
                                }
                                break;
                        }
                    }

                    try
                    {
                        engine.Save(fileName);
                        ShowMessage?.Invoke("CSV for ADO created successfully.");
                    }
                    catch (Exception exc)
                    {
                        ShowWarning?.Invoke(exc.Message);
                    }
                }
            }

        }

        private string TextToHtml(string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text))
                result = HttpUtility.HtmlEncode(text).Replace("\r\n", "\r").Replace("\n", "\r")
                    .Replace("\r", "<br>").Replace("  ", " &nbsp;");

            return result;
        }
    }
}