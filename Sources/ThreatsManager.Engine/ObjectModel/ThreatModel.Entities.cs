using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private static int _lastProcess;
        private static int _lastExternalInteractor;
        private static int _lastDataStore;

        private Action<IThreatEventsContainer, IThreatEvent> _threatEventAddedToEntity;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAddedToEntity
        {
            add
            {
                if (_threatEventAddedToEntity == null || !_threatEventAddedToEntity.GetInvocationList().Contains(value))
                {
                    _threatEventAddedToEntity += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventAddedToEntity != null) _threatEventAddedToEntity -= value;
            }
        }

        private Action<IThreatEventsContainer, IThreatEvent> _threatEventRemovedFromEntity;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemovedFromEntity
        {
            add
            {
                if (_threatEventRemovedFromEntity == null || !_threatEventRemovedFromEntity.GetInvocationList().Contains(value))
                {
                    _threatEventRemovedFromEntity += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventRemovedFromEntity != null) _threatEventRemovedFromEntity -= value;
            }
        }

        private Action<IVulnerabilitiesContainer, IVulnerability> _vulnerabilityAddedToEntity;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAddedToEntity
        {
            add
            {
                if (_vulnerabilityAddedToEntity == null || !_vulnerabilityAddedToEntity.GetInvocationList().Contains(value))
                {
                    _vulnerabilityAddedToEntity += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_vulnerabilityAddedToEntity != null) _vulnerabilityAddedToEntity -= value;
            }
        }

        private Action<IVulnerabilitiesContainer, IVulnerability> _vulnerabilityRemovedFromEntity;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemovedFromEntity
        {
            add
            {
                if (_vulnerabilityRemovedFromEntity == null || !_vulnerabilityRemovedFromEntity.GetInvocationList().Contains(value))
                {
                    _vulnerabilityRemovedFromEntity += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_vulnerabilityRemovedFromEntity != null) _vulnerabilityRemovedFromEntity -= value;
            }
        }

        [Child]
        [JsonProperty("entities")]
        private IList<IEntity> _entities;

        [IgnoreAutoChangeNotification]
        public IEnumerable<IEntity> Entities => _entities?.AsEnumerable();

        [InitializationRequired]
        public IEntity GetEntity(Guid id)
        {
            return _entities?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IEnumerable<IEntity> GetEntities([Required] string name)
        {
            return _entities?.Where(x => name.IsEqual(x.Name));
        }

        [InitializationRequired]
        public IEnumerable<IEntity> SearchEntities([Required] string filter)
        {
            var rx = new Regex(filter, RegexOptions.IgnoreCase);

            return _entities?.Where(x => (x.Name != null && rx.IsMatch(x.Name)));
        }

        [InitializationRequired]
        public void Add([NotNull] IEntity entity)
        {
            using (var scope = UndoRedoManager.OpenScope("Add Entity"))
            {
                if (_entities == null)
                    _entities = new AdvisableCollection<IEntity>();

                _entities.Add(entity);
                UndoRedoManager.Attach(entity);
                scope.Complete();
            }
        }

        [InitializationRequired]
        public IEntity AddEntity<T>() where T : IEntity
        {
            return AddEntity<T>(GetFirstAvailableEntityName<T>());
        }

        [InitializationRequired]
        public IEntity AddEntity<T>([Required] string name) where T : IEntity
        {
            return AddEntity<T>(name, null);
        }

        [InitializationRequired]
        public IEntity AddEntity<T>([Required] string name, IEntityTemplate template) where T : IEntity
        {
            IEntity result = null;

            if (typeof(T) == typeof(IProcess))
                result = new Process(name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };
            if (typeof(T) == typeof(IExternalInteractor))
                result = new ExternalInteractor(name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };
            if (typeof(T) == typeof(IDataStore))
                result = new DataStore(name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };

            if (result != null)
            {
                Add(result);
                RegisterEvents(result);
                ChildCreated?.Invoke(result);
            }

            return result;
        }

        private void OnThreatEventRemovedFromEntity([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            _threatEventRemovedFromEntity?.Invoke(container, threatEvent);
        }

        private void OnThreatEventAddedToEntity([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            _threatEventAddedToEntity?.Invoke(container, threatEvent);
        }

        private void OnVulnerabilityRemovedFromEntity([NotNull] IVulnerabilitiesContainer container, [NotNull] IVulnerability vulnerability)
        {
            _vulnerabilityRemovedFromEntity?.Invoke(container, vulnerability);
        }

        private void OnVulnerabilityAddedToEntity([NotNull] IVulnerabilitiesContainer container, [NotNull] IVulnerability vulnerability)
        {
            _vulnerabilityAddedToEntity?.Invoke(container, vulnerability);
        }

        private string GetFirstAvailableEntityName<T>() where T : IEntity
        {
            string result = null;

            if (typeof(T) == typeof(IProcess))
                result = GetFirstAvailableProcessName();
            if (typeof(T) == typeof(IExternalInteractor))
                result = GetFirstAvailableExternalInteractor();
            if (typeof(T) == typeof(IDataStore))
                result = GetFirstAvailableDataStore();

            return result;
        }

        private string GetFirstAvailableProcessName()
        {
            _lastProcess += 1;

            string result = $"{GetIdentityTypeName(typeof(Process))} {_lastProcess}";

            var process = _entities?.OfType<IProcess>().FirstOrDefault(x => string.CompareOrdinal(result, x.Name) == 0);
            if (process != null)
            {
                result = GetFirstAvailableProcessName();
            }

            return result;
        }

        private string GetFirstAvailableExternalInteractor()
        {
            _lastExternalInteractor += 1;

            string result = $"{GetIdentityTypeName(typeof(ExternalInteractor))} {_lastExternalInteractor}";

            var externalInteractor = _entities?.OfType<IExternalInteractor>().FirstOrDefault(x => string.CompareOrdinal(result, x.Name) == 0);
            if (externalInteractor != null)
            {
                result = GetFirstAvailableExternalInteractor();
            }

            return result;
        }
        
        private string GetFirstAvailableDataStore()
        {
            _lastDataStore += 1;

            string result = $"{GetIdentityTypeName(typeof(DataStore))} {_lastDataStore}";

            var dataStore = _entities?.OfType<IDataStore>().FirstOrDefault(x => string.CompareOrdinal(result, x.Name) == 0);
            if (dataStore != null)
            {
                result = GetFirstAvailableDataStore();
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveEntity(Guid id)
        {
            bool result = false;

            var entity = GetEntity(id);
            if (entity != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Entity"))
                {
                    RemoveRelated(entity);
                    result = _entities?.Remove(entity) ?? false;
                    if (result)
                    {
                        UndoRedoManager.Detach(entity);

                        UnregisterEvents(entity);
                        ChildRemoved?.Invoke(entity);
                    }
                        
                    scope.Complete();
                }
            }

            return result;
        }

        public void RemoveRelated([NotNull] IEntity entity)
        {
            var diagrams = _diagrams?.ToArray();
            if (diagrams != null)
            {
                foreach (var diagram in diagrams)
                {
                    diagram.RemoveEntityShape(entity.Id);
                }
            }

            var dataFlows = _flows?.Where(x => x.SourceId == entity.Id || x.TargetId == entity.Id).ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var flow in dataFlows)
                {
                    RemoveDataFlow(flow.Id);
                }
            }

        }
    }
}