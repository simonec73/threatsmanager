using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Engine.Properties;
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

        [JsonProperty("entities")]
        private List<IEntity> _entities;

        public IEnumerable<IEntity> Entities => _entities?.AsReadOnly();

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
            if (_entities == null)
                _entities = new List<IEntity>();

            _entities.Add(entity);
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
                result = new Process(this, name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };
            if (typeof(T) == typeof(IExternalInteractor))
                result = new ExternalInteractor(this, name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };
            if (typeof(T) == typeof(IDataStore))
                result = new DataStore(this, name)
                {
                    _templateId = template?.Id ?? Guid.Empty
                };

            if (result != null)
            {
                if (_entities == null)
                    _entities = new List<IEntity>();
                _entities.Add(result);
                RegisterEvents(result);
                SetDirty();
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
                RemoveRelated(entity);
                result = _entities?.Remove(entity) ?? false;
                if (result)
                {
                    UnregisterEvents(entity);
                    SetDirty();
                    ChildRemoved?.Invoke(entity);
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

            var dataFlows = _dataFlows?.Where(x => x.SourceId == entity.Id || x.TargetId == entity.Id).ToArray();
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