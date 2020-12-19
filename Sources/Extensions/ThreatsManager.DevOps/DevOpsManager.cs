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
        private static readonly Dictionary<Guid, IDevOpsConnector> _connectors = new Dictionary<Guid, IDevOpsConnector>();

        public static void Register([NotNull] IDevOpsConnector connector, [NotNull] IThreatModel model)
        {
            Unregister(model);

            var config = new DevOpsConfigPropertySchemaManager(model);
            config.RegisterConnection(model, connector);

            _connectors.Add(model.Id, connector);
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

        public static async Task UpdateAsync([NotNull] IThreatModel model)
        {
            if (_connectors.TryGetValue(model.Id, out var connector) && (connector?.IsConnected() ?? false))
            {
                await UpdateMitigationsAsync(model, connector);
            }
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

        private static async Task UpdateMitigationsAsync([NotNull] IThreatModel model, [NotNull] IDevOpsConnector connector)
        {
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

                        if (mitigation != null && info is WorkItemInfo workItemInfo)
                        {
                            schemaManager.SetDevOpsStatus(mitigation, connector, info.Id, workItemInfo.Status);
                        }
                    }
                }
            }
        }
    }
}
