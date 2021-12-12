using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;
using PostSharp.Patterns.Threading;

namespace ThreatsManager.DevOps.Panels.MitigationsKanban
{
#pragma warning disable CS0067
    public partial class MitigationsKanbanPanel
    {
        private readonly Guid _id = Guid.NewGuid();
        public event Action<string, bool> ChangeCustomActionStatus;

        public Guid Id => _id;
        public string TabLabel => "DevOps";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("DevOps", "DevOps", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Sync", "Synchronize",
                            Properties.Resources.cloud_updown_big,
                            Properties.Resources.cloud_updown),
                        new ActionDefinition(Id, "Auto", "Automatic Retrieval",
                            Properties.Resources.cloud_download_big,
                            Properties.Resources.cloud_download),
                    }),
                    new CommandsBarDefinition("Filter", "Filter", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ShortTerm", "Short Term\nMitigations",
                            Properties.Resources.short_term_big,
                            Properties.Resources.short_term),
                        new ActionDefinition(Id, "MidTerm", "Mid Term\nMitigations",
                            Properties.Resources.mid_term_big,
                            Properties.Resources.mid_term),
                        new ActionDefinition(Id, "LongTerm", "Long Term\nMitigations",
                            Properties.Resources.long_term_big,
                            Properties.Resources.long_term),
                        new ActionDefinition(Id, "All", "All Mitigations",
                            Resources.mitigations_big,
                            Resources.mitigations),
                    }),new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
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

        private CountdownEvent _countdown;

        [InitializationRequired]
        public async void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            //string text = null;
            //bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "Sync":
                        if ((await DevOpsManager.UpdateAsync(_model)) > 0)
                        {
                            LoadModel();
                        }
                        break;
                    case "Auto":
                        var summaries = DevOpsManager.GetMitigationsSummary(_model);
                        var schemaManager = new RoadmapPropertySchemaManager(_model);
                        var mitigations = _model?.GetUniqueMitigations()?
                            .Where(x => (schemaManager.GetStatus(x) != RoadmapStatus.NoActionRequired) &&
                                        !(summaries?.ContainsKey(x) ?? false))
                            .OrderBy(x => x.Name).ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            var connector = DevOpsManager.GetConnector(_model);
                            var devOpsSchemaManager = new DevOpsPropertySchemaManager(_model);
                            _countdown = new CountdownEvent(mitigations.Length);
                            foreach (var mitigation in mitigations)
                                AutoLoad(mitigation, connector, devOpsSchemaManager);

                            ShowMessage?.Invoke("Automatic Load in progress...");
                            AutomaticLoadCompletion();
                        }
                        else
                        {
                            ShowWarning?.Invoke("Automatic Load has not identified any action to do.");
                        }
                        break;
                    case "ShortTerm":
                        _filter = RoadmapStatus.ShortTerm;
                        LoadModel();
                        break;
                    case "MidTerm":
                        _filter = RoadmapStatus.MidTerm;
                        LoadModel();
                        break;
                    case "LongTerm":
                        _filter = RoadmapStatus.LongTerm;
                        LoadModel();
                        break;
                    case "All":
                        _filter = RoadmapStatus.NoActionRequired;
                        LoadModel();
                        break;
                    case "Refresh":
                        LoadModel();
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

        private async void AutoLoad([NotNull] IMitigation mitigation, [NotNull] IDevOpsConnector connector, 
            [NotNull] DevOpsPropertySchemaManager schemaManager)
        {
            try
            {
                var devOpsItemInfos = (await connector.GetItemsAsync(mitigation.Name))?.ToArray();
                if ((devOpsItemInfos?.Length ?? 0) == 1)
                {
                    var info = devOpsItemInfos.First();
                    if (info?.Id >= 0)
                    {
                        var workItemInfo = await connector.GetWorkItemInfoAsync(info.Id);
                        if (workItemInfo != null)
                        {
                            schemaManager.SetDevOpsStatus(mitigation, connector, info.Id, 
                                info.Url, info.AssignedTo, workItemInfo.Status);
                        }
                    }
                }
            }
            finally
            {
                _countdown.Signal();
            }
        }

        [Background]
        private void AutomaticLoadCompletion()
        {
            if (_countdown.Wait(10000))
                ShowMessage?.Invoke("Automatic Load done successfully.");
            else
                ShowMessage?.Invoke("Automatic Load is taking more than expected.");

            LoadModel();
        }
    }
}