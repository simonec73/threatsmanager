using PostSharp.Patterns.Contracts;
using System;
using System.ComponentModel;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        public event Action<IIdentity> ChildCreated;
        public event Action<IIdentity> ChildRemoved;
        public event Action<IIdentity, string> ChildChanged;
        public event Action<IIdentity, IPropertyType, IProperty> ChildPropertyAdded;
        public event Action<IIdentity, IPropertyType, IProperty> ChildPropertyRemoved;
        public event Action<IIdentity, IPropertyType, IProperty> ChildPropertyValueChanged;
        public event Action<string> ContributorAdded;
        public event Action<string> ContributorRemoved;
        public event Action<string, string> ContributorChanged;
        public event Action<string> AssumptionAdded;
        public event Action<string> AssumptionRemoved;
        public event Action<string, string> AssumptionChanged;
        public event Action<string> DependencyAdded;
        public event Action<string> DependencyRemoved;
        public event Action<string, string> DependencyChanged;

        internal void RegisterEvents()
        {
            PropertyAdded += OnPropertyAdded;
            PropertyRemoved += OnPropertyRemoved;
            PropertyValueChanged += OnPropertyValueChanged;

            if (_actors?.Any() ?? false)
            {
                foreach (var actor in _actors)
                {
                    RegisterEvents(actor);
                }
            }

            if (_flows?.Any() ?? false)
            {
                foreach (var dataFlow in _flows)
                {
                    RegisterEvents(dataFlow);
                }
            }

            if (_diagrams?.Any() ?? false)
            {
                foreach (var diagram in _diagrams)
                {
                    RegisterEvents(diagram);
                }
            }

            if (_entities?.Any() ?? false)
            {
                foreach (var entity in _entities)
                {
                    RegisterEvents(entity);
                }
            }

            if (_groups?.Any() ?? false)
            {
                foreach (var group in _groups)
                {
                    RegisterEvents(group);
                }
            }

            if (_mitigations?.Any() ?? false)
            {
                foreach (var mitigation in _mitigations)
                {
                    RegisterEvents(mitigation);
                }
            }

            if (_schemas?.Any() ?? false)
            {
                foreach (var schema in _schemas)
                {
                    RegisterEvents(schema);
                }
            }

            if (_severities?.Any() ?? false)
            {
                foreach (var severity in _severities)
                {
                    RegisterEvents(severity);
                }
            }

            if (_threatTypes?.Any() ?? false)
            {
                foreach (var threatType in _threatTypes)
                {
                    RegisterEvents(threatType);
                }
            }

            if (_weaknesses?.Any() ?? false)
            {
                foreach (var weakness in _weaknesses)
                {
                    RegisterEvents(weakness);
                }
            }
        }

        private void UnregisterEvents()
        {
            PropertyAdded -= OnPropertyAdded;
            PropertyRemoved -= OnPropertyRemoved;
            PropertyValueChanged -= OnPropertyValueChanged;

            if (_actors?.Any() ?? false)
            {
                foreach (var actor in _actors)
                {
                    UnregisterEvents(actor);
                }
            }

            if (_flows?.Any() ?? false)
            {
                foreach (var dataFlow in _flows)
                {
                    UnregisterEvents(dataFlow);
                }
            }

            if (_diagrams?.Any() ?? false)
            {
                foreach (var diagram in _diagrams)
                {
                    UnregisterEvents(diagram);
                }
            }

            if (_entities?.Any() ?? false)
            {
                foreach (var entity in _entities)
                {
                    UnregisterEvents(entity);
                }
            }

            if (_groups?.Any() ?? false)
            {
                foreach (var group in _groups)
                {
                    UnregisterEvents(group);
                }
            }

            if (_mitigations?.Any() ?? false)
            {
                foreach (var mitigation in _mitigations)
                {
                    UnregisterEvents(mitigation);
                }
            }

            if (_schemas?.Any() ?? false)
            {
                foreach (var schema in _schemas)
                {
                    UnregisterEvents(schema);
                }
            }

            if (_severities?.Any() ?? false)
            {
                foreach (var severity in _severities)
                {
                    UnregisterEvents(severity);
                }
            }

            if (_threatTypes?.Any() ?? false)
            {
                foreach (var threatType in _threatTypes)
                {
                    UnregisterEvents(threatType);
                }
            }

            if (_weaknesses?.Any() ?? false)
            {
                foreach (var weakness in _weaknesses)
                {
                    UnregisterEvents(weakness);
                }
            }
        }

        private void RegisterEvents([NotNull] IEntity entity)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)entity).PropertyChanged += OnPropertyChanged;
            entity.PropertyAdded += OnPropertyAdded;
            entity.PropertyRemoved += OnPropertyRemoved;
            entity.PropertyValueChanged += OnPropertyValueChanged;
            entity.ThreatEventAdded += OnThreatEventAddedToEntity;
            entity.ThreatEventRemoved += OnThreatEventRemovedFromEntity;
            entity.VulnerabilityAdded += OnVulnerabilityAddedToEntity;
            entity.VulnerabilityRemoved += OnVulnerabilityRemovedFromEntity;
        }

        private void UnregisterEvents([NotNull] IEntity entity)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)entity).PropertyChanged -= OnPropertyChanged;
            entity.PropertyAdded -= OnPropertyAdded;
            entity.PropertyRemoved -= OnPropertyRemoved;
            entity.PropertyValueChanged -= OnPropertyValueChanged;
            entity.ThreatEventAdded -= OnThreatEventAddedToEntity;
            entity.ThreatEventRemoved -= OnThreatEventRemovedFromEntity;
            entity.VulnerabilityAdded -= OnVulnerabilityAddedToEntity;
            entity.VulnerabilityRemoved -= OnVulnerabilityRemovedFromEntity;
        }

        private void RegisterEvents([NotNull] IGroup group)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)group).PropertyChanged += OnPropertyChanged;
            group.PropertyAdded += OnPropertyAdded;
            group.PropertyRemoved += OnPropertyRemoved;
            group.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] IGroup group)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)group).PropertyChanged -= OnPropertyChanged;
            group.PropertyAdded -= OnPropertyAdded;
            group.PropertyRemoved -= OnPropertyRemoved;
            group.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void RegisterEvents([NotNull] IDataFlow dataFlow)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)dataFlow).PropertyChanged += OnPropertyChanged;
            dataFlow.PropertyAdded += OnPropertyAdded;
            dataFlow.PropertyRemoved += OnPropertyRemoved;
            dataFlow.PropertyValueChanged += OnPropertyValueChanged;
            dataFlow.ThreatEventAdded += OnThreatEventAddedToDataFlow;
            dataFlow.ThreatEventRemoved += OnThreatEventRemovedFromDataFlow;
            dataFlow.VulnerabilityAdded += OnVulnerabilityAddedToDataFlow;
            dataFlow.VulnerabilityRemoved += OnVulnerabilityRemovedFromDataFlow;
        }

        private void UnregisterEvents([NotNull] IDataFlow dataFlow)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)dataFlow).PropertyChanged -= OnPropertyChanged;
            dataFlow.PropertyAdded -= OnPropertyAdded;
            dataFlow.PropertyRemoved -= OnPropertyRemoved;
            dataFlow.PropertyValueChanged -= OnPropertyValueChanged;
            dataFlow.ThreatEventAdded -= OnThreatEventAddedToDataFlow;
            dataFlow.ThreatEventRemoved -= OnThreatEventRemovedFromDataFlow;
            dataFlow.VulnerabilityAdded -= OnVulnerabilityAddedToDataFlow;
            dataFlow.VulnerabilityRemoved -= OnVulnerabilityRemovedFromDataFlow;
        }

        private void RegisterEvents([NotNull] IPropertySchema schema)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)schema).PropertyChanged += OnPropertyChanged;
        }

        private void UnregisterEvents([NotNull] IPropertySchema schema)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)schema).PropertyChanged -= OnPropertyChanged;
        }

        private void RegisterEvents([NotNull] IDiagram diagram)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)diagram).PropertyChanged += OnPropertyChanged;
            diagram.PropertyAdded += OnPropertyAdded;
            diagram.PropertyRemoved += OnPropertyRemoved;
            diagram.PropertyValueChanged += OnPropertyValueChanged;
            diagram.EntityShapeAdded += OnEntityShapeAdded;
            diagram.EntityShapeRemoved += OnEntityShapeRemoved;
            diagram.GroupShapeAdded += OnGroupShapeAdded;
            diagram.GroupShapeRemoved += OnGroupShapeRemoved;
            diagram.LinkAdded += OnLinkAdded;
            diagram.LinkRemoved += OnLinkRemoved;
        }

        private void UnregisterEvents([NotNull] IDiagram diagram)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)diagram).PropertyChanged -= OnPropertyChanged;
            diagram.PropertyAdded -= OnPropertyAdded;
            diagram.PropertyRemoved -= OnPropertyRemoved;
            diagram.PropertyValueChanged -= OnPropertyValueChanged;
            diagram.EntityShapeAdded -= OnEntityShapeAdded;
            diagram.EntityShapeRemoved -= OnEntityShapeRemoved;
            diagram.GroupShapeAdded -= OnGroupShapeAdded;
            diagram.GroupShapeRemoved -= OnGroupShapeRemoved;
            diagram.LinkAdded -= OnLinkAdded;
            diagram.LinkRemoved -= OnLinkRemoved;
        }

        private void RegisterEvents([NotNull] ISeverity severity)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)severity).PropertyChanged += OnPropertyChanged;
            severity.PropertyAdded += OnPropertyAdded;
            severity.PropertyRemoved += OnPropertyRemoved;
            severity.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] ISeverity severity)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)severity).PropertyChanged -= OnPropertyChanged;
            severity.PropertyAdded -= OnPropertyAdded;
            severity.PropertyRemoved -= OnPropertyRemoved;
            severity.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void RegisterEvents([NotNull] IThreatType threatType)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)threatType).PropertyChanged += OnPropertyChanged;
            threatType.PropertyAdded += OnPropertyAdded;
            threatType.PropertyRemoved += OnPropertyRemoved;
            threatType.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] IThreatType threatType)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)threatType).PropertyChanged -= OnPropertyChanged;
            threatType.PropertyAdded -= OnPropertyAdded;
            threatType.PropertyRemoved -= OnPropertyRemoved;
            threatType.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void RegisterEvents([NotNull] IWeakness weakness)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)weakness).PropertyChanged += OnPropertyChanged;
            weakness.PropertyAdded += OnPropertyAdded;
            weakness.PropertyRemoved += OnPropertyRemoved;
            weakness.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] IWeakness weakness)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)weakness).PropertyChanged -= OnPropertyChanged;
            weakness.PropertyAdded -= OnPropertyAdded;
            weakness.PropertyRemoved -= OnPropertyRemoved;
            weakness.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void RegisterEvents([NotNull] IMitigation mitigation)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)mitigation).PropertyChanged += OnPropertyChanged;
            mitigation.PropertyAdded += OnPropertyAdded;
            mitigation.PropertyRemoved += OnPropertyRemoved;
            mitigation.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] IMitigation mitigation)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)mitigation).PropertyChanged -= OnPropertyChanged;
            mitigation.PropertyAdded -= OnPropertyAdded;
            mitigation.PropertyRemoved -= OnPropertyRemoved;
            mitigation.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void RegisterEvents([NotNull] IThreatActor actor)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)actor).PropertyChanged += OnPropertyChanged;
            actor.PropertyAdded += OnPropertyAdded;
            actor.PropertyRemoved += OnPropertyRemoved;
            actor.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void UnregisterEvents([NotNull] IThreatActor actor)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)actor).PropertyChanged -= OnPropertyChanged;
            actor.PropertyAdded -= OnPropertyAdded;
            actor.PropertyRemoved -= OnPropertyRemoved;
            actor.PropertyValueChanged -= OnPropertyValueChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IIdentity identity)
            {
                ChildChanged?.Invoke(identity, e.PropertyName);
            }
        }

        private void OnPropertyAdded([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            if (container is IIdentity identity)
            {
                ChildPropertyAdded?.Invoke(identity, property.PropertyType, property);
            }
        }

        private void OnPropertyRemoved([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            if (container is IIdentity identity)
            {
                ChildPropertyRemoved?.Invoke(identity, property.PropertyType, property);
            }
        }

        private void OnPropertyValueChanged([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            if (container is IIdentity identity)
            {
                ChildPropertyValueChanged?.Invoke(identity, property.PropertyType, property);
            }
        }
    }
}
