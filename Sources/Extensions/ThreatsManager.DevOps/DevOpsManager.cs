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
        private static int _intervalMins;
        private static bool _stopUpdater = false;
        private static bool _updaterExecuting = false;
        private static NotificationType _notificationType;
        private static readonly Dictionary<Guid, IDevOpsConnector> _connectors = new Dictionary<Guid, IDevOpsConnector>();

        public static event Action<int> RefreshDone;
        public static event Action<IThreatModel, IDevOpsConnector> ConnectorAdded;
        public static event Action<IThreatModel, IDevOpsConnector> ConnectorRemoved;

        public static void Register([NotNull] IDevOpsConnector connector, [NotNull] IThreatModel model)
        {
            Unregister(model);

            var config = new DevOpsConfigPropertySchemaManager(model);
            config.RegisterConnection(model, connector);

            _connectors.Add(model.Id, connector);

            ConnectorAdded?.Invoke(model, connector);
        }

        public static void UpdateConfig([NotNull] IThreatModel model)
        {
            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                var config = new DevOpsConfigPropertySchemaManager(model);
                config.RegisterConnection(model, connector);
            }
        }

        public static void Unregister([NotNull] IThreatModel model)
        {
            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                connector.Disconnect();

                var config = new DevOpsConfigPropertySchemaManager(model);
                config.UnregisterConnection(model);

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

        public static IDictionary<IMitigation, WorkItemStatus> GetMitigationsStatus([NotNull] IThreatModel model)
        {
            IDictionary<IMitigation, WorkItemStatus> result = null;

            if (_connectors.TryGetValue(model.Id, out var connector))
            {
                var schemaManager = new DevOpsPropertySchemaManager(model);

                var mitigations = model.GetUniqueMitigations()?.OrderBy(x => x.Name).ToArray();
                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        var status = schemaManager.GetDevOpsStatus(mitigation, connector);

                        if (result == null)
                            result = new Dictionary<IMitigation, WorkItemStatus>();
                        result.Add(mitigation, status);
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
                    }
                    else
                    {
                        id = workItemInfo.Id;
                    }

                    if (id >= 0 && await connector.SetWorkItemStateAsync(id, status).ConfigureAwait(false))
                    {
                        var schemaManager = new DevOpsPropertySchemaManager(model);
                        schemaManager.SetDevOpsStatus(mitigation, connector, id, status);
                        result = true;
                    }
                    else
                    {
                        
                    }
                }
            }

            return result;
        }

        public static async Task StartAutomaticUpdaterAsync([NotNull] IThreatModel model, [Range(1, 120)] int intervalMins, NotificationType notificationType)
        {
            _stopUpdater = false;
            _intervalMins = intervalMins;
            _notificationType = notificationType;

            if (!_updaterExecuting)
            {
                await AutomaticUpdater(model);
            }
        }

        public static void StopAutomaticUpdater()
        {
            _stopUpdater = true;
        }

        private static async Task<int> UpdateMitigationsAsync([NotNull] IThreatModel model, [NotNull] IDevOpsConnector connector)
        {
            int result = 0;

            var schemaManager = new DevOpsPropertySchemaManager(model);

            var mitigations = model.GetUniqueMitigations()?
                .Where(x => schemaManager.GetDevOpsId(x, connector) >= 0)
                .ToDictionary(x => schemaManager.GetDevOpsId(x, connector), x => x)
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                var infos = await connector.GetWorkItemsInfoAsync(mitigations.Select(x => x.Key));

                if (infos != null)
                {
                    foreach (var info in infos)
                    {
                        var mitigation = mitigations
                            .Where(x => x.Key == info.Id)
                            .Select(x => x.Value)
                            .FirstOrDefault();

                        if (mitigation != null && info != null)
                        {
                            var oldStatus = schemaManager.GetDevOpsStatus(mitigation, connector);
                            if (oldStatus != info.Status)
                            {
                                schemaManager.SetDevOpsStatus(mitigation, connector, info.Id, info.Status);
                                result++;
                            }
                        }
                    }
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
                    var changes = await UpdateAsync(model);
                    if (_notificationType == NotificationType.Full || 
                        (_notificationType == NotificationType.SuccessOnly && changes > 0))
                        RefreshDone?.Invoke(changes);

                    await Task.Delay(_intervalMins * 60000);
                } while (!_stopUpdater);

                _updaterExecuting = false;
            }
        }
    }
}
