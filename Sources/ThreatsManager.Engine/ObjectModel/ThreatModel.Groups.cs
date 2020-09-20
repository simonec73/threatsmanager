using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Engine.Properties;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private static int _lastGroup = 0;
        private static int _lastTrustBoundary = 0;

        [JsonProperty("groups")]
        private List<IGroup> _groups;

        public IEnumerable<IGroup> Groups => _groups?.AsReadOnly();

        [InitializationRequired]
        public IGroup GetGroup(Guid id)
        {
            return _groups?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public void Add(IGroup group)
        {
            if (group is IThreatModelChild child && child.Model != this)
                throw new ArgumentException();

            if (_groups == null)
                _groups = new List<IGroup>();

            _groups.Add(group);
        }

        [InitializationRequired]
        public T AddGroup<T>() where T : class, IGroup
        {
            return AddGroup<T>(GetFirstAvailableGroupName<T>());
        }

        [InitializationRequired]
        public T AddGroup<T>([Required] string name) where T : class, IGroup
        {
            T result = default(T);

            if (typeof(T) == typeof(ITrustBoundary))
            {
                result = new TrustBoundary(this, name) as T;
                if (_groups == null)
                    _groups = new List<IGroup>();
                _groups.Add(result);
                SetDirty();
                RegisterEvents(result);
                ChildCreated?.Invoke(result);
            }

            return result;
        }
        
        [InitializationRequired]
        public ITrustBoundary AddTrustBoundary([Required] string name, ITrustBoundaryTemplate template)
        {
            ITrustBoundary result = new TrustBoundary(this, name)
            {
                _templateId = template?.Id ?? Guid.Empty
            };
            
            if (_groups == null)
                _groups = new List<IGroup>();
            _groups.Add(result);
            RegisterEvents(result);
            SetDirty();
            ChildCreated?.Invoke(result);

            return result;
        }

        [InitializationRequired]
        public void AddGroup([NotNull] IGroup group)
        {
            if (group.Model != this)
                throw new ArgumentException();

            if (_groups == null)
                _groups = new List<IGroup>();
            _groups.Add(group);
            SetDirty();
            RegisterEvents(group);
            ChildCreated?.Invoke(group);
        }
        
        private string GetFirstAvailableGroupName<T>() where T : IGroup
        {
            string result = null;

            if (typeof(T) == typeof(ITrustBoundary))
                result = GetFirstAvailableTrustBoundaryName();
            else
                result = GetFirstAvailableGroupName();

            return result;
        }

        private string GetFirstAvailableGroupName()
        {
            _lastGroup += 1;

            string result = $"Group {_lastGroup}";

            var group = _groups?.FirstOrDefault(x => string.CompareOrdinal(result, x.Name) == 0);
            if (group != null)
            {
                result = GetFirstAvailableGroupName();
            }

            return result;
        }
        
        private string GetFirstAvailableTrustBoundaryName()
        {
            _lastTrustBoundary += 1;

            string result = $"{GetIdentityTypeName(typeof(TrustBoundary))} {_lastTrustBoundary}";

            var trustBoundary = _groups?.OfType<ITrustBoundary>().FirstOrDefault(x => string.CompareOrdinal(result, x.Name) == 0);
            if (trustBoundary != null)
            {
                result = GetFirstAvailableTrustBoundaryName();
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveGroup(Guid id)
        {
            bool result = false;

            var item = GetGroup(id);
            if (item != null)
            {
                var newParent = (item as IGroupElement)?.Parent;
                
                var entities = item.Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                        entity.SetParent(newParent);
                }

                var groups = item.Groups?.ToArray();
                if (groups?.Any() ?? false)
                {
                    foreach (var group in groups)
                    {
                        (group as IGroupElement)?.SetParent(newParent);
                    }
                }

                RemoveRelated(item);

                result = _groups.Remove(item);
                if (result)
                {
                    UnregisterEvents(item);
                    SetDirty();
                    ChildRemoved?.Invoke(item);
                }
            }

            return result;
        }

        public void RemoveRelated([NotNull] IGroup group)
        {
            var diagrams = _diagrams?.ToArray();
            if (diagrams != null)
            {
                foreach (var diagram in diagrams)
                {
                    diagram.RemoveGroupShape(group.Id);
                }
            }
        }
    }
}