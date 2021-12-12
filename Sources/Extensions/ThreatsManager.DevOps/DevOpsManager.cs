using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps
{
    public static class DevOpsManager
    {
        #region Private member variables.
        private static int _intervalMins;
        private static Task _updater;
        private static bool _stopUpdater;
        private static NotificationType _notificationType;
        private static readonly Dictionary<Guid, IDevOpsConnector> _connectors = new Dictionary<Guid, IDevOpsConnector>();
        #endregion

        public static event Action<IThreatModel, int> RefreshDone;
        public static event Action<IThreatModel, IDevOpsConnector> ConnectorAdded;
        public static event Action<IThreatModel, IDevOpsConnector> ConnectorRemoved;

        public static void Register([NotNull] IDevOpsConnector connector, [NotNull] IThreatModel model)
        {
            Unregister(model);

            var config = new DevOpsConfigPropertySchemaManager(model);
            config.RegisterConnection(connector);

            _connectors.Add(model.Id, connector);

            ConnectorAdded?.Invoke(model, connector);
        }

        public static void UpdateConfig([NotNull] IThreatModel model)
        {
            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                var config = new DevOpsConfigPropertySchemaManager(model);
                config.RegisterConnection(connector);
            }
        }

        public static void Unregister([NotNull] IThreatModel model)
        {
            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                connector.Disconnect();

                var config = new DevOpsConfigPropertySchemaManager(model);
                config.UnregisterConnection();

                _connectors.Remove(model.Id);

                ConnectorRemoved?.Invoke(model, connector);
            }
        }

        public static IDevOpsConnector GetConnector([NotNull] IThreatModel model)
        {
            IDevOpsConnector result = null;

            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                result = connector;
            }

            return result;
        }

        public static async Task<int> UpdateAsync([NotNull] IThreatModel model)
        {
            int result = 0;

            if (_connectors.TryGetValue(model.Id, out var connector) && (connector?.IsConnected() ?? false))
            {
                result = await UpdateMitigationsAsync(model, connector);
            }

            return result;
        }

        public static IDictionary<IMitigation, WorkItemSummary> GetMitigationsSummary([NotNull] IThreatModel model)
        {
            IDictionary<IMitigation, WorkItemSummary> result = null;

            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                var schemaManager = new DevOpsPropertySchemaManager(model);

                var mitigations = model.GetUniqueMitigations()?.OrderBy(x => x.Name).ToArray();
                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        var info = schemaManager.GetDevOpsInfo(mitigation, connector);

                        if (info != null)
                        {
                            if (result == null)
                                result = new Dictionary<IMitigation, WorkItemSummary>();
                            result.Add(mitigation, new WorkItemSummary(info.Id, info.Status, info.AssignedTo));
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<bool> SetMitigationsStatusAsync([NotNull] IMitigation mitigation, WorkItemStatus status)
        {
            bool result = false;

            var model = mitigation.Model;

            if (model != null)
            {
                var connector = GetConnector(model);
                if (connector?.IsConnected() ?? false)
                {
                    var workItemInfo = await connector.GetWorkItemInfoAsync(mitigation).ConfigureAwait(false);
                    int id;
                    if (workItemInfo == null)
                    {
                        id = await connector.CreateWorkItemAsync(mitigation).ConfigureAwait(false);
                        if (id >= 0)
                            workItemInfo = await connector.GetWorkItemInfoAsync(id).ConfigureAwait(false);
                    }
                    else
                    {
                        id = workItemInfo.Id;
                    }

                    if (id >= 0)
                    {
                        if (await connector.SetWorkItemStateAsync(id, status).ConfigureAwait(false))
                        {
                            var schemaManager = new DevOpsPropertySchemaManager(model);
                            schemaManager.SetDevOpsStatus(mitigation, connector, id, workItemInfo?.Url, status);
                            result = true;
                        }
                        else
                        {
                            throw new WorkItemStateChangeException(mitigation,
                                workItemInfo?.Status ?? WorkItemStatus.Created, status);
                        }
                    }
                    else
                    {
                        throw new WorkItemCreationException(mitigation);
                    }
                }
            }

            return result;
        }

        public static void StartAutomaticUpdater([NotNull] IThreatModel model, [Range(1, 120)] int intervalMins, NotificationType notificationType)
        {
            _stopUpdater = false;
            _intervalMins = intervalMins;
            _notificationType = notificationType;

            if (_updater == null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _updater = AutomaticUpdater(model);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public static bool StopAutomaticUpdater()
        {
            _stopUpdater = true;
            return _updater?.Wait(3000) ?? true;
        }

        private static async Task<int> UpdateMitigationsAsync([NotNull] IThreatModel model, [NotNull] IDevOpsConnector connector)
        {
            int result = 0;

            var schemaManager = new DevOpsPropertySchemaManager(model);

            var mitigations = model.GetUniqueMitigations()?
                .ToDictionary(x => x, x => schemaManager.GetDevOpsInfo(x, connector))
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                var infos = await connector
                    .GetWorkItemsInfoAsync(mitigations
                        .Select(x => x.Value?.Id ?? -1)
                        .Where(x => x >= 0)
                    );

                if (infos != null)
                {
                    foreach (var info in infos)
                    {
                        var pairs = mitigations
                            .Where(x => x.Value != null && x.Value.Id == info.Id)
                            .ToArray();

                        if (pairs.Any())
                        {
                            var pair = pairs.FirstOrDefault();

                            if (pair.Value.Status != info.Status || 
                                string.CompareOrdinal(pair.Value.AssignedTo, info.AssignedTo) != 0)
                            {
                                schemaManager.SetDevOpsStatus(pair.Key, connector, info.Id, info.Url, info.AssignedTo, info.Status);
                                result++;
                            }

                            MitigationStatus status; 
                            switch (info.Status)
                            {
                                case WorkItemStatus.Created:
                                    status = MitigationStatus.Approved;
                                    break;
                                case WorkItemStatus.Planned:
                                    status = MitigationStatus.Planned;
                                    break;
                                case WorkItemStatus.InProgress:
                                    status = MitigationStatus.Planned;
                                    break;
                                case WorkItemStatus.Done:
                                    status = MitigationStatus.Implemented;
                                    break;
                                default:
                                    status = MitigationStatus.Proposed;
                                    break;
                            }

                            var tems = model.GetThreatEventMitigations(pair.Key);
                            if (tems?.Any() ?? false)
                            {
                                foreach (var tem in tems)
                                {
                                    if (tem.Status != status)
                                        tem.Status = status;
                                }
                            }
                        }
                    }
                }

                var missing = mitigations.Where(x => x.Value != null && (infos?.All(y => y.Id != x.Value.Id) ?? true)).ToArray();
                if (missing.Any())
                {
                    foreach (var m in missing)
                        schemaManager.RemoveDevOpsInfos(m.Key);
                }
            }

            return result;
        }

        private static async Task AutomaticUpdater([NotNull] IThreatModel model)
        {
            if (_intervalMins > 0 && _intervalMins <= 120)
            {
                do
                {
                    var connector = GetConnector(model);
                    if (connector != null)
                    {
                        var changes = await UpdateAsync(model);
                        if (_notificationType == NotificationType.Full ||
                            (_notificationType == NotificationType.SuccessOnly && changes > 0))
                            RefreshDone?.Invoke(model, changes);
                    }

                    await Task.Delay(_intervalMins * 60000);
                } while (!_stopUpdater);

                _updater = null;
            }
        }
    }
}
