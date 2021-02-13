using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal abstract class Field
    {
        #region Private member variables.
        private static IThreatModel _model;
        private static IEnumerable<IPropertyType> _eiProperties;
        private static IEnumerable<IPropertyType> _pProperties;
        private static IEnumerable<IPropertyType> _dsProperties;
        private static IEnumerable<IPropertyType> _dfProperties;
        private static IEnumerable<IPropertyType> _tbProperties;
        private static IEnumerable<IPropertyType> _teProperties;
        private static IEnumerable<IPropertyType> _mProperties;
        private static IEnumerable<IPropertyType> _ttProperties;
        #endregion

        #region Public members.
        public abstract string Tooltip { get; }

        public virtual bool SecondPass => false;

        public virtual string Label => ToString();

        public abstract void InsertContent(WTableCell cell, IIdentity identity);

        public virtual bool IsVisible(IIdentity identity)
        {
            return true;
        }

        public static Field Severity => new SeverityField();
        public static Field Description => new DescriptionField();
        public static Field AssociatedTo => new AssociatedToField();
        public static Field Strength => new ControlTypeField();
        public static Field AffectedObjects => new AssociatedObjectsField();
        public static Field ApprovedMitigations => new ApprovedMitigationsField();
        public static Field ExistingMitigations => new ExistingMitigationsField();
        public static Field ImplementedMitigations => new ImplementedMitigationsField();
        public static Field PlannedMitigations => new PlannedMitigationsField();
        public static Field ProposedMitigations => new ProposedMitigationsField();
        public static Field AffectedThreats => new AffectedThreatsField();
        public static Field Directives => new DirectivesField();
        public static Field Source => new SourceField();
        public static Field Target => new TargetField();
        public static Field FlowType => new FlowTypeField();

        public static IEnumerable<Field> GetFields(ItemType type, [NotNull] IThreatModel model)
        {
            List<Field> result = null;

            if (model != null && model != _model)
            {
                _model = model;
                LoadModel(model);
            }

            switch (type)
            {
                case ItemType.ExternalInteractor:
                    result = new List<Field>();
                    result.Add(Field.Description);
                    if (_eiProperties?.Any() ?? false)
                    {
                        foreach (var p in _eiProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.Process:
                    result = new List<Field>();
                    result.Add(Field.Description);
                    if (_pProperties?.Any() ?? false)
                    {
                        foreach (var p in _pProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.DataStore:
                    result = new List<Field>();
                    result.Add(Field.Description);
                    if (_dsProperties?.Any() ?? false)
                    {
                        foreach (var p in _dsProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.DataFlow:
                    result = new List<Field>();
                    result.Add(Field.Description);
                    result.Add(Field.Source);
                    result.Add(Field.Target);
                    result.Add(Field.FlowType);
                    if (_dfProperties?.Any() ?? false)
                    {
                        foreach (var p in _dfProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.TrustBoundary:
                    result = new List<Field>();
                    result.Add(Field.Description);
                    if (_tbProperties?.Any() ?? false)
                    {
                        foreach (var p in _tbProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.ThreatType:
                    result = new List<Field>();
                    result.Add(Field.Severity);
                    result.Add(Field.Description);
                    result.Add(Field.AffectedObjects);
                    result.Add(Field.ApprovedMitigations);
                    result.Add(Field.ExistingMitigations);
                    result.Add(Field.ImplementedMitigations);
                    result.Add(Field.PlannedMitigations);
                    result.Add(Field.ProposedMitigations);
                    if (_ttProperties?.Any() ?? false)
                    {
                        foreach (var p in _ttProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    if (_teProperties?.Any() ?? false)
                    {
                        foreach (var p in _teProperties)
                        {
                            if (p.Visible)
                                result.Add(new TablePropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.ThreatEvent:
                    result = new List<Field>();
                    result.Add(Field.Severity);
                    result.Add(Field.Description);
                    result.Add(Field.AssociatedTo);
                    result.Add(Field.ApprovedMitigations);
                    result.Add(Field.ExistingMitigations);
                    result.Add(Field.ImplementedMitigations);
                    result.Add(Field.PlannedMitigations);
                    result.Add(Field.ProposedMitigations);
                    if (_teProperties?.Any() ?? false)
                    {
                        foreach (var p in _teProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
                case ItemType.Mitigation:
                    result = new List<Field>();
                    result.Add(Field.Strength);
                    result.Add(Field.Description);
                    result.Add(Field.AffectedThreats);
                    result.Add(Field.Directives);
                    if (_mProperties?.Any() ?? false)
                    {
                        foreach (var p in _mProperties)
                        {
                            if (p.Visible)
                                result.Add(new PropertyTypeField(p));
                        }
                    }
                    break;
            }

            return result;
        }

        public static ItemType GetItemType([NotNull] IIdentity identity)
        {
            var result = ItemType.Undefined;

            if (identity is IExternalInteractor)
                result = ItemType.ExternalInteractor;
            else if (identity is IProcess)
                result = ItemType.Process;
            else if (identity is IDataStore)
                result = ItemType.DataStore;
            else if (identity is IDataFlow)
                result = ItemType.DataFlow;
            else if (identity is ITrustBoundary)
                result = ItemType.TrustBoundary;
            else if (identity is IThreatType)
                result = ItemType.ThreatType;
            else if (identity is IThreatEvent)
                result = ItemType.ThreatEvent;
            else if (identity is IMitigation)
                result = ItemType.Mitigation;

            return result;
        }
        #endregion

        #region Load Threat Model.
        private static void LoadModel([NotNull] IThreatModel model)
        {
            List<IThreatEvent> threatEvents = new List<IThreatEvent>();
            List<IThreatEventMitigation> mitigations = new List<IThreatEventMitigation>();

            var externalInteractors = model.Entities?.OfType<IExternalInteractor>().ToArray();
            if (externalInteractors?.Any() ?? false)
            {
                _eiProperties = LoadItems(externalInteractors, threatEvents, mitigations);
            }

            var processes = model.Entities?.OfType<IProcess>().ToArray();
            if (processes?.Any() ?? false)
            {
                _pProperties = LoadItems(processes, threatEvents, mitigations);
            }

            var dataStores = model.Entities?.OfType<IDataStore>().ToArray();
            if (dataStores?.Any() ?? false)
            {
                _dsProperties = LoadItems(dataStores, threatEvents, mitigations);
            }

            var dataFlows = model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                _dfProperties = LoadItems(dataFlows, threatEvents, mitigations);
            }

            var trustBoundaries = model.Groups?.OfType<ITrustBoundary>().ToArray();
            if (trustBoundaries?.Any() ?? false)
            {
                _tbProperties = LoadItems(trustBoundaries, threatEvents, mitigations);
            }

            var modelTe = model.ThreatEvents?.ToArray();
            if (modelTe?.Any() ?? false)
            {
                threatEvents.AddRange(modelTe);
                foreach (var threatEvent in modelTe)
                {
                    var tem = threatEvent.Mitigations?.ToArray();
                    if (tem?.Any() ?? false)
                        mitigations.AddRange(tem);
                }
            }

            if (threatEvents.Any())
            {
                _teProperties = LoadItems(threatEvents);
            }

            if (mitigations.Any())
            {
                _mProperties = LoadItems(mitigations);
            }

            var threatTypes = model.ThreatTypes?.ToArray();
            if (threatTypes?.Any() ?? false)
            {
                _ttProperties = LoadItems(threatTypes);
            }
        }

        private static void GetPropertyTypes([NotNull] IPropertiesContainer container,
            ref IEnumerable<IPropertyType> properties)
        {
            var temp = container.Properties?
                .Where(x => x.PropertyType != null && x.PropertyType.Visible &&
                            (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false))
                .Select(x => x.PropertyType)
                .Distinct().ToArray();
            if (temp?.Any() ?? false)
            {
                var props = properties?.ToArray();
                if (props?.Any() ?? false)
                    properties = props.Union(temp).Distinct();
                else
                    properties = temp;
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IExternalInteractor> entities,
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }

                    GetPropertyTypes(entity, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IProcess> entities,
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }

                    GetPropertyTypes(entity, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IDataStore> entities,
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }

                    GetPropertyTypes(entity, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IDataFlow> dataFlows,
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (dataFlows.Any())
            {
                foreach (var dataFlow in dataFlows)
                {
                    var teArray = dataFlow.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }

                    GetPropertyTypes(dataFlow, ref result);
                }
            }

            return result;
        }
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<ITrustBoundary> trustBoundaries,
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (trustBoundaries.Any())
            {
                foreach (var boundary in trustBoundaries)
                {
                    //var teArray = boundary.ThreatEvents?.ToArray();
                    //if (teArray?.Any() ?? false)
                    //{
                    //    threatEvents.AddRange(teArray);

                    //    foreach (var teItem in teArray)
                    //    {
                    //        var mArray = teItem.Mitigations?.ToArray();
                    //        if (mArray?.Any() ?? false)
                    //        {
                    //            mitigations.AddRange(mArray);
                    //        }
                    //    }
                    //}

                    GetPropertyTypes(boundary, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            IEnumerable<IPropertyType> result = null;

            if (threatEvents.Any())
            {
                foreach (var threatEvent in threatEvents)
                {
                    GetPropertyTypes(threatEvent, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IThreatEventMitigation> mitigations)
        {
            IEnumerable<IPropertyType> result = null;

            if (mitigations.Any())
            {
                foreach (var mitigation in mitigations)
                {
                    GetPropertyTypes(mitigation.Mitigation, ref result);
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static IEnumerable<IPropertyType> LoadItems([NotNull] IEnumerable<IThreatType> threatTypes)
        {
            IEnumerable<IPropertyType> result = null;

            if (threatTypes.Any())
            {
                foreach (var threatType in threatTypes)
                {
                    GetPropertyTypes(threatType, ref result);
                }
            }

            return result;
        }
        #endregion

        #region Auxiliary functions for derived classes.
        protected static IEnumerable<IThreatEvent> GetAssociatedThreatEvents([NotNull] IThreatType threatType)
        {
            List<IThreatEvent> result = new List<IThreatEvent>();

            var entities = threatType.Model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var entityTe = entity.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
                    if (entityTe?.Any() ?? false)
                        result.AddRange(entityTe);
                }
            }

            var dataFlows = threatType.Model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var dataFlowTe = dataFlow.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
                    if (dataFlowTe?.Any() ?? false)
                        result.AddRange(dataFlowTe);
                }
            }

            var threatModelTe = threatType.Model.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
            if (threatModelTe?.Any() ?? false)
                result.AddRange(threatModelTe);

            return result;
        }

        protected static ISeverity GetMaximumSeverity(IEnumerable<IThreatEvent> threatEvents)
        {
            ISeverity result = null;

            var array = threatEvents?.ToArray();
            if (array?.Any() ?? false)
            {
                foreach (var item in array)
                {
                    if (result == null || result.Id < (item.Severity?.Id ?? 0))
                    {
                        result = item.Severity;
                    }
                }
            }

            return result;
        }

        protected static ISeverity GetMaximumSeverity([NotNull] IThreatType threatType)
        {
            return GetMaximumSeverity(GetAssociatedThreatEvents(threatType));
        }

        protected static IEnumerable<IThreatEvent> GetAllThreatEvents([NotNull] IThreatModel model)
        {
            List<IThreatEvent> result = new List<IThreatEvent>();

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var entityTe = entity.ThreatEvents?.ToArray();
                    if (entityTe?.Any() ?? false)
                        result.AddRange(entityTe);
                }
            }

            var dataFlows = model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var dataFlowTe = dataFlow.ThreatEvents?.ToArray();
                    if (dataFlowTe?.Any() ?? false)
                        result.AddRange(dataFlowTe);
                }
            }

            var threatModelTe = model.ThreatEvents?.ToArray();
            if (threatModelTe?.Any() ?? false)
                result.AddRange(threatModelTe);

            return result;
        }

        protected static IEnumerable<IThreatEventMitigation> GetAssociatedThreatEvents([NotNull] IMitigation mitigation)
        {
            var result = new List<IThreatEventMitigation>();

            var entities = mitigation?.Model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var entityTem = entity.ThreatEvents?
                        .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                        .Select(x => x.Mitigations.Where(y => y.MitigationId == mitigation.Id))
                        .ToArray();
                    if (entityTem?.Any() ?? false)
                    {
                        foreach (var list in entityTem)
                            result.AddRange(list);
                    }
                }
            }

            var dataFlows = mitigation?.Model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var dataFlowTem = dataFlow.ThreatEvents?
                        .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                        .Select(x => x.Mitigations.Where(y => y.MitigationId == mitigation.Id))
                        .ToArray();
                    if (dataFlowTem?.Any() ?? false)
                    {
                        foreach (var list in dataFlowTem)
                            result.AddRange(list);
                    }
                }
            }

            var threatModelTem = mitigation?.Model.ThreatEvents?
                .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                .Select(x => x.Mitigations.Where(y => y.MitigationId == mitigation.Id))
                .ToArray();
            if (threatModelTem?.Any() ?? false)
            {
                foreach (var list in threatModelTem)
                    result.AddRange(list);
            }

            return result;

        }
        #endregion
    }
}