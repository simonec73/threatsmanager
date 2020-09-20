using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Engine.Properties;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private Action<IEntityShapesContainer, IEntityShape> _entityShapeAdded;
        public event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded
        {
            add
            {
                if (_entityShapeAdded == null || !_entityShapeAdded.GetInvocationList().Contains(value))
                {
                    _entityShapeAdded += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_entityShapeAdded != null) _entityShapeAdded -= value;
            }
        }

        private Action<IEntityShapesContainer, IEntity> _entityShapeRemoved;
        public event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved
        {
            add
            {
                if (_entityShapeRemoved == null || !_entityShapeRemoved.GetInvocationList().Contains(value))
                {
                    _entityShapeRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_entityShapeRemoved != null) _entityShapeRemoved -= value;
            }
        }

        private Action<IGroupShapesContainer, IGroupShape> _groupShapeAdded;
        public event Action<IGroupShapesContainer, IGroupShape> GroupShapeAdded
        {
            add
            {
                if (_groupShapeAdded == null || !_groupShapeAdded.GetInvocationList().Contains(value))
                {
                    _groupShapeAdded += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_groupShapeAdded != null) _groupShapeAdded -= value;
            }
        }

        private Action<IGroupShapesContainer, IGroup> _groupShapeRemoved;
        public event Action<IGroupShapesContainer, IGroup> GroupShapeRemoved
        {
            add
            {
                if (_groupShapeRemoved == null || !_groupShapeRemoved.GetInvocationList().Contains(value))
                {
                    _groupShapeRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_groupShapeRemoved != null) _groupShapeRemoved -= value;
            }
        }

        private Action<ILinksContainer, ILink> _linkAdded;
        public event Action<ILinksContainer, ILink> LinkAdded
        {
            add
            {
                if (_linkAdded == null || !_linkAdded.GetInvocationList().Contains(value))
                {
                    _linkAdded += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_linkAdded != null) _linkAdded -= value;
            }
        }

        private Action<ILinksContainer, IDataFlow> _linkRemoved;
        public event Action<ILinksContainer, IDataFlow> LinkRemoved
        {
            add
            {
                if (_linkRemoved == null || !_linkRemoved.GetInvocationList().Contains(value))
                {
                    _linkRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_linkRemoved != null) _linkRemoved -= value;
            }
        }

        private void OnEntityShapeAdded(IEntityShapesContainer arg1, IEntityShape arg2)
        {
            _entityShapeAdded?.Invoke(arg1, arg2);
        }

        private void OnEntityShapeRemoved(IEntityShapesContainer arg1, IEntity arg2)
        {
            _entityShapeRemoved?.Invoke(arg1, arg2);
        }

        private void OnGroupShapeAdded(IGroupShapesContainer arg1, IGroupShape arg2)
        {
            _groupShapeAdded?.Invoke(arg1, arg2);
        }

        private void OnGroupShapeRemoved(IGroupShapesContainer arg1, IGroup arg2)
        {
            _groupShapeRemoved?.Invoke(arg1, arg2);
        }

        private void OnLinkAdded(ILinksContainer arg1, ILink arg2)
        {
            _linkAdded?.Invoke(arg1, arg2);
        }

        private void OnLinkRemoved(ILinksContainer arg1, IDataFlow arg2)
        {
            _linkRemoved?.Invoke(arg1, arg2);
        }

        private static int _lastDiagram;

        [JsonProperty("diagrams")]
        private List<IDiagram> _diagrams;

        public IEnumerable<IDiagram> Diagrams => _diagrams?.AsReadOnly();

        [InitializationRequired]
        public IEnumerable<IDiagram> GetDiagrams([Required] string name)
        {
            return _diagrams?.Where(x => string.CompareOrdinal(name, x.Name) == 0);
        }

        [InitializationRequired]
        public IDiagram GetDiagram(Guid id)
        {
            return _diagrams?.FirstOrDefault(x => id == x.Id);
        }

        [InitializationRequired]
        public void Add([NotNull] IDiagram diagram)
        {
            if (_diagrams == null)
                _diagrams = new List<IDiagram>();

            _diagrams.Add(diagram);
        }

        [InitializationRequired]
        public IDiagram AddDiagram()
        {
            return AddDiagram(GetFirstAvailableDiagramName());
        }

        private string GetFirstAvailableDiagramName()
        {
            _lastDiagram += 1;

            string result = $"{GetIdentityTypeName(typeof(Diagram))} {_lastDiagram}";

            var diagrams = GetDiagrams(result);
            if (diagrams?.Any() ?? false)
            {
                result = GetFirstAvailableDiagramName();
            }

            return result;
        }

        [InitializationRequired]
        public IDiagram AddDiagram([Required] string name)
        {
            IDiagram result = null;

            var diagrams = GetDiagrams(name);
            if (!(diagrams?.Any() ?? false))
            {
                if (_diagrams == null)
                    _diagrams = new List<IDiagram>();
                result = new Diagram(this, name)
                {
                    Order = _diagrams.Any() ? _diagrams.Max(x => x.Order) + 1 : 1
                };
                _diagrams.Add(result);
                SetDirty();
                RegisterEvents(result);
                ChildCreated?.Invoke(result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveDiagram(Guid id)
        {
            bool result = false;

            var diagram = GetDiagram(id);
            if (diagram != null)
            {
                result = _diagrams.Remove(diagram);
                if (result)
                {
                    UnregisterEvents(diagram);
                    SetDirty();
                    ChildRemoved?.Invoke(diagram);
                }
            }

            return result;
        }
    }
}