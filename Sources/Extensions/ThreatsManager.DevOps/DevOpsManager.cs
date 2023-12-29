using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps
{
    public static class DevOpsManager
    {
        #region Private member variables.
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

        public static int Update([NotNull] IThreatModel model)
        {
            int result = 0;

            if (_connectors.TryGetValue(model.Id, out var connector) && (connector?.IsConnected() ?? false))
            {
                result = UpdateMitigations(model, connector);
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

        public static bool SetMitigationsStatus([NotNull] IMitigation mitigation, WorkItemStatus status)
        {
            bool result = false;

            var model = mitigation.Model;

            if (model != null)
            {
                var connector = GetConnector(model);
                if (connector?.IsConnected() ?? false)
                {
                    var workItemInfo = connector.GetWorkItemInfoAsync(mitigation).Result;
                    int id;
                    if (workItemInfo == null)
                    {
                        id = connector.CreateWorkItemAsync(mitigation).Result;
                        if (id >= 0)
                            workItemInfo = connector.GetWorkItemInfoAsync(id).Result;
                    }
                    else
                    {
                        id = workItemInfo.Id;
                    }

                    if (id >= 0)
                    {
                        if (connector.SetWorkItemStateAsync(id, status).Result)
                        {
                            using (var scope = UndoRedoManager.OpenScope("Set DevOps Mitigation status"))
                            {
                                var schemaManager = new DevOpsPropertySchemaManager(model);
                                schemaManager.SetDevOpsStatus(mitigation, connector, id, workItemInfo?.Url, status);
                                scope?.Complete();
                            }
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

        private static int UpdateMitigations([NotNull] IThreatModel model, [NotNull] IDevOpsConnector connector)
        {
            int result = 0;

            var schemaManager = new DevOpsPropertySchemaManager(model);

            var mitigations = model.GetUniqueMitigations()?
                .ToDictionary(x => x, x => schemaManager.GetDevOpsInfo(x, connector))
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                var infos = connector
                    .GetWorkItemsInfoAsync(mitigations
                        .Select(x => x.Value?.Id ?? -1)
                        .Where(x => x >= 0)
                    ).Result;

                using (var scope = UndoRedoManager.OpenScope("Update Mitigations from DevOps"))
                {
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
                                    schemaManager.SetDevOpsStatus(pair.Key, connector, info.Id, info.Url, info.Status, info.AssignedTo);
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


                    scope?.Complete();
                }
            }

            return result;
        }
    }
}
