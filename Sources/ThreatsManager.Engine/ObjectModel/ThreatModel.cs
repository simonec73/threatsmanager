using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [NotifyPropertyChanged]
    [Serializable]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatEventsContainerAspect]
    [VulnerabilitiesContainerAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Threat Model")]
    [TypeInitial("M")]
    public partial class ThreatModel : IThreatModel, IInitializableObject, IDisposable
    {
        #region Constructors.
        public ThreatModel()
        {
            _lastMitigation = 0;
            _lastDataStore = 0;
            _lastDiagram = 0;
            _lastExternalInteractor = 0;
            _lastGroup = 0;
            _lastProcess = 0;
            _lastTrustBoundary = 0;
        }

        public ThreatModel([Required] string name) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
        }
        #endregion

        public bool IsInitialized => Id != Guid.Empty;

        public void Dispose()
        {
            UnregisterEvents();
        }

        #region General properties and methods.
        public Scope PropertiesScope => Scope.ThreatModel;

        [JsonProperty("owner", Order = 4)]
        public string Owner { get; set; }

        #region Contributors.
        [Reference]
        [JsonProperty("contributors")]
        [field: NotRecorded]
        private List<string> _legacyContributors { get; set; }

        [Child]
        [JsonProperty("contrib", Order = 5)]
        private AdvisableCollection<RecordableString> _contributors { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<string> Contributors => _contributors?.Select(x => x.Value).AsEnumerable();

        public bool AddContributor([Required] string name)
        {
            bool result = false;

            if (!(_contributors?.Any(x => string.CompareOrdinal(name, x.Value) == 0) ?? false))
            {
                if (_contributors == null)
                    _contributors = new AdvisableCollection<RecordableString>();

                _contributors.Add(new RecordableString(name));
                result = true;
                ContributorAdded?.Invoke(name);
            }

            return result;
        }

        public bool RemoveContributor([Required] string name)
        {
            bool result = false;

            var contributor = _contributors?.FirstOrDefault(x => string.CompareOrdinal(name, x.Value) == 0);
            if (contributor != null)
            {
                result = _contributors.Remove(contributor);
                if (result)
                {
                    ContributorRemoved?.Invoke(name);
                }
            }

            return result;
        }

        public bool ChangeContributor([Required] string oldName, [Required] string newName)
        {
            bool result = false;

            var contributor = _contributors?.FirstOrDefault(x => string.CompareOrdinal(oldName, x.Value) == 0);
            if (contributor != null)
            {
                // ReSharper disable once PossibleNullReferenceException
                contributor.Value = newName;
                result = true;
                ContributorChanged?.Invoke(oldName, newName);
            }

            return result;
        }
        #endregion

        #region Assumptions.
        [Reference]
        [JsonProperty("assumptions")]
        [field:NotRecorded]
        private List<string> _legacyAssumptions { get; set; }

        [Child]
        [JsonProperty("assump", Order = 6)]
        [field:NotRecorded]
        private AdvisableCollection<RecordableString> _assumptions { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<string> Assumptions => _assumptions?.Select(x => x.Value).AsEnumerable();

        public bool AddAssumption([Required] string text)
        {
            bool result = false;

            if (!(_assumptions?.Any(x => string.CompareOrdinal(text, x.Value) == 0) ?? false))
            {
                if (_assumptions == null)
                    _assumptions = new AdvisableCollection<RecordableString>();

                _assumptions.Add(new RecordableString(text));
                result = true;
                AssumptionAdded?.Invoke(text);
            }

            return result;
        }

        public bool RemoveAssumption([Required] string text)
        {
            bool result = false;

            var assumption = _assumptions?.FirstOrDefault(x => string.CompareOrdinal(text, x.Value) == 0);
            if (assumption != null)
            {
                result = _assumptions.Remove(assumption);
                if (result)
                {
                    AssumptionRemoved?.Invoke(text);
                }
            }

            return result;
        }

        public bool ChangeAssumption([Required] string oldText, [Required] string newText)
        {
            bool result = false;

            var assumption = _assumptions?.FirstOrDefault(x => string.CompareOrdinal(oldText, x.Value) == 0);
            if (assumption != null)
            {
                assumption.Value = newText;
                result = true;
                AssumptionChanged?.Invoke(oldText, newText);
            }

            return result;
        }
        #endregion

        #region Dependencies.
        [Reference]
        [JsonProperty("dependencies")]
        [field:NotRecorded]
        private List<string> _legacyDependencies { get; set; }

        [Child]
        [JsonProperty("depend", Order = 7)]
        [field:NotRecorded]
        private AdvisableCollection<RecordableString> _dependencies { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<string> ExternalDependencies => _dependencies?.Select(x => x.Value).AsEnumerable();

        public bool AddDependency([Required] string text)
        {
            bool result = false;

            if (!(_dependencies?.Any(x => string.CompareOrdinal(x.Value, text) == 0) ?? false))
            {
                if (_dependencies == null)
                    _dependencies = new AdvisableCollection<RecordableString>();

                _dependencies.Add(new RecordableString(text));
                result = true;
                DependencyAdded?.Invoke(text);
            }

            return result;
        }

        public bool RemoveDependency([Required] string text)
        {
            bool result = false;

            var dependency = _dependencies?.FirstOrDefault(x => string.CompareOrdinal(text, x.Value) == 0);
            if (dependency != null)
            {
                result = _dependencies.Remove(dependency);
                if (result)
                {
                    DependencyRemoved?.Invoke(text);
                }
            }

            return result;
        }

        public bool ChangeDependency([Required] string oldText, [Required] string newText)
        {
            bool result = false;

            var dependency = _dependencies?.FirstOrDefault(x => string.CompareOrdinal(oldText, x.Value) == 0);
            if (dependency != null)
            {
                dependency.Value = newText;
                result = true;
                DependencyChanged?.Invoke(oldText, newText);
            }

            return result;
        }
        #endregion

        public void ExecutePostDeserialization()
        {
            if (_legacyContributors?.Any() ?? false)
            {
                if (_contributors == null)
                    _contributors = new AdvisableCollection<RecordableString>();

                foreach (var contrib in _legacyContributors)
                {
                    var r = new RecordableString(contrib);
                    UndoRedoManager.Attach(r, this);
                    _contributors.Add(r);
                }

                _legacyContributors.Clear();
            }

            if (_legacyAssumptions?.Any() ?? false)
            {
                if (_assumptions == null)
                    _assumptions = new AdvisableCollection<RecordableString>();

                foreach (var assump in _legacyAssumptions)
                {
                    var r = new RecordableString(assump);
                    UndoRedoManager.Attach(r, this);
                    _assumptions.Add(r);
                }

                _legacyAssumptions.Clear();
            }

            if (_legacyDependencies?.Any() ?? false)
            {
                if (_dependencies == null)
                    _dependencies = new AdvisableCollection<RecordableString>();

                foreach (var depend in _legacyDependencies)
                {
                    var r = new RecordableString(depend);
                    UndoRedoManager.Attach(r, this);
                    _dependencies.Add(r);
                }

                _legacyDependencies.Clear();
            }
        }

        public string GetIdentityTypeName([NotNull] IIdentity identity)
        {
            return GetIdentityTypeName(identity.GetType());
        }

        private string GetIdentityTypeName(Type type)
        {
            TypeLabelAttribute[] attribs = type.GetCustomAttributes(
                typeof(TypeLabelAttribute), false) as TypeLabelAttribute[];

            return attribs?.Length > 0 ? attribs[0].Label : type.Name;
        }

        public string GetIdentityTypeInitial([NotNull] IIdentity identity)
        {
            return GetIdentityTypeInitial(identity.GetType());
        }

        private string GetIdentityTypeInitial(Type type)
        {
            TypeInitialAttribute[] attribs = type.GetCustomAttributes(
                typeof(TypeInitialAttribute), false) as TypeInitialAttribute[];

            return attribs?.Length > 0 ? attribs[0].Initial : null;
        }

        public IIdentity GetIdentity(Guid id)
        {
            IIdentity result = null;

            if (Id == id)
                result = this;
            if (result == null)
                result = GetEntity(id);
            if (result == null)
                result = GetDataFlow(id);
            if (result == null)
                result = GetGroup(id);
            if (result == null)
                result = GetDiagram(id);
            if (result == null)
                result = GetSchema(id);
            if (result == null)
                result = GetThreatType(id);
            if (result == null)
                result = GetWeakness(id);
            if (result == null)
                result = GetMitigation(id);
            if (result == null)
                result = GetThreatActor(id);
            if (result == null)
                result = GetEntityTemplate(id);
            if (result == null)
                result = GetTrustBoundaryTemplate(id);
            if (result == null)
                result = GetFlowTemplate(id);
            if (result == null)
                result = GetThreatEvent(id);
            if (result == null)
                result = _entities?.FirstOrDefault(x => x.ThreatEvents?.Any(y => y.Id == id) ?? false)?.GetThreatEvent(id);
            if (result == null)
                result = _flows?.FirstOrDefault(x => x.ThreatEvents?.Any(y => y.Id == id) ?? false)?.GetThreatEvent(id);
            if (result == null)
                result = GetVulnerability(id);
            if (result == null)
                result = _entities?.FirstOrDefault(x => x.Vulnerabilities?.Any(y => y.Id == id) ?? false)?.GetVulnerability(id);
            if (result == null)
                result = _flows?.FirstOrDefault(x => x.Vulnerabilities?.Any(y => y.Id == id) ?? false)?.GetVulnerability(id);

            return result;
        }
        
        public override string ToString()
        {
            return Name;
        }

        public IVulnerability FindVulnerability(Guid id)
        {
            IVulnerability result = null;

            result = _vulnerabilities?.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                var entities = _entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        result = entity.Vulnerabilities?.FirstOrDefault(x => x.Id == id);
                        if (result != null)
                            break;
                    }
                }
            }

            if (result == null)
            {
                var dataFlows = _flows?.ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        result = dataFlow.Vulnerabilities?.FirstOrDefault(x => x.Id == id);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        public IThreatEvent FindThreatEvent(Guid id)
        {
            IThreatEvent result = null;

            result = _threatEvents?.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                var entities = _entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        result = entity.ThreatEvents?.FirstOrDefault(x => x.Id == id);
                        if (result != null)
                            break;
                    }
                }
            }

            if (result == null)
            {
                var dataFlows = _flows?.ToArray();
                if (dataFlows?.Any() ?? false)
                {
                    foreach (var dataFlow in dataFlows)
                    {
                        result = dataFlow.ThreatEvents?.FirstOrDefault(x => x.Id == id);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Advanced Threats & Mitigations properties and functions.
        [IgnoreAutoChangeNotification]
        public int AssignedThreatTypes => CountThreatTypes();

        [IgnoreAutoChangeNotification]
        public int FullyMitigatedThreatTypes => _threatTypes?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) >= 100).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int PartiallyMitigatedThreatTypes
        {
            get
            {
                var result = 0;

                var threatTypes = _threatTypes?.ToArray();
                if (threatTypes?.Any() ?? false)
                {
                    foreach (var threatType in threatTypes)
                    {
                        var totalStrength = threatType.Mitigations?.Sum(x => x.StrengthId) ?? 0;
                        if (totalStrength > 0 && totalStrength < 100)
                            result++;
                    }
                }

                return result;
            }
        }

        [IgnoreAutoChangeNotification]
        public int NotMitigatedThreatTypes => _threatTypes?
                                                  .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) == 0).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int TotalThreatEvents => CountThreatEvents();

        [IgnoreAutoChangeNotification]
        public int UniqueMitigations => GetUniqueMitigations()?.Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int FullyMitigatedThreatEvents => GetThreatEvents()?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) >= 100).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int PartiallyMitigatedThreatEvents
        {
            get
            {
                var result = 0;

                var threatEvents = GetThreatEvents()?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var threatEvent in threatEvents)
                    {
                        var totalStrength = threatEvent.Mitigations?.Sum(x => x.StrengthId) ?? 0;
                        if (totalStrength > 0 && totalStrength < 100)
                            result++;
                    }
                }

                return result;
            }
        }

        [IgnoreAutoChangeNotification]
        public int NotMitigatedThreatEvents => GetThreatEvents()?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) == 0).Count() ?? 0;

        public int CountThreatEvents([NotNull] ISeverity severity)
        {
            return CountThreatEvents(severity.Id);
        }

        public int CountThreatEvents([Positive] int severityId)
        {
            return (_threatEvents?
                        .Where(x => x.Severity != null && x.SeverityId == severityId).Count() ?? 0) +
                   (_entities?.Sum(x => x.ThreatEvents?
                                            .Where(y => y.Severity != null && y.SeverityId == severityId)
                                            .Count() ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.ThreatEvents?
                                             .Where(y => y.Severity != null && y.SeverityId == severityId)
                                             .Count() ?? 0) ?? 0);
        }

        public int CountThreatEventsByType([NotNull] ISeverity severity)
        {
            return CountThreatEventsByType(severity.Id);
        }

        public int CountThreatEventsByType([Positive] int severityId)
        {
            int result = 0;

            var types = _threatTypes?.ToArray();

            if (types?.Any() ?? false)
            {
                foreach (var type in types)
                {
                    var severity = type.GetTopSeverity();
                    if (severity != null && severity.Id == severityId)
                        result++;
                }
            }

            return result;
        }

        public int CountMitigationsByStatus(MitigationStatus status)
        {
            return (_threatEvents?.Sum(x => x.Mitigations?.Where(y => y.Status == status).Count() ?? 0) ?? 0) +
                   (_vulnerabilities?.Sum(x => x.Mitigations?.Where(y => y.Status == status).Count() ?? 0) ?? 0) +
                   (_entities?.Sum(x => x.ThreatEvents?.Sum(y => y.Mitigations?.Where(z => z.Status == status).Count() ?? 0) ?? 0) ?? 0) +
                   (_entities?.Sum(x => x.Vulnerabilities?.Sum(y => y.Mitigations?.Where(z => z.Status == status).Count() ?? 0) ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.ThreatEvents?.Sum(y => y.Mitigations?.Where(z => z.Status == status).Count() ?? 0) ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.Vulnerabilities?.Sum(y => y.Mitigations?.Where(z => z.Status == status).Count() ?? 0) ?? 0) ?? 0);
        }

        public IEnumerable<IThreatEvent> GetThreatEvents()
        {
            return GetThreatEvents(null);
        }

        public IEnumerable<IThreatEvent> GetThreatEvents(IThreatType threatType)
        {
            var threatEvents = new List<IThreatEvent>();

            GetThreatEvents(this, threatType, threatEvents);
            GetThreatEvents(_entities, threatType, threatEvents);
            GetThreatEvents(_flows, threatType, threatEvents);

            return threatEvents;
        }

        public IEnumerable<IMitigation> GetUniqueMitigations()
        {
            var mitigations = new List<IMitigation>();

            GetMitigations(_threatEvents?.Select(x => x.Mitigations), mitigations);
            GetMitigations(_vulnerabilities?.Select(x => x.Mitigations), mitigations);
            GetMitigations(_entities?.Select(x => x.ThreatEvents?.Select(y => y.Mitigations)), mitigations);
            GetMitigations(_entities?.Select(x => x.Vulnerabilities?.Select(y => y.Mitigations)), mitigations);
            GetMitigations(_flows?.Select(x => x.ThreatEvents?.Select(y => y.Mitigations)), mitigations);
            GetMitigations(_flows?.Select(x => x.Vulnerabilities?.Select(y => y.Mitigations)), mitigations);

            return mitigations;
        }

        public IEnumerable<IThreatTypeMitigation> GetThreatTypeMitigations()
        {
            IEnumerable<IThreatTypeMitigation> result = null;

            var threatTypes = _threatTypes?.OrderBy(x => x.Name).ToArray();
            if (threatTypes?.Any() ?? false)
            {
                var list = new List<IThreatTypeMitigation>();

                foreach (var threatType in threatTypes)
                {
                    var mitigations = threatType.Mitigations?.OrderBy(x => x.Mitigation.Name).ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        list.AddRange(mitigations);
                    }
                }

                if (list.Any())
                    result = list.AsReadOnly();
            }

            return result;
        }

        public IEnumerable<IThreatTypeMitigation> GetThreatTypeMitigations([NotNull] IMitigation mitigation)
        {
            IEnumerable<IThreatTypeMitigation> result = null;

            var threatTypes = _threatTypes?
                .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                .OrderBy(x => x.Name).ToArray();
            if (threatTypes?.Any() ?? false)
            {
                var list = new List<IThreatTypeMitigation>();

                foreach (var threatType in threatTypes)
                {
                    var threatTypeMitigation = threatType.Mitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    if (threatTypeMitigation != null)
                    {
                        list.Add(threatTypeMitigation);
                    }
                }

                if (list.Any())
                    result = list.AsReadOnly();
            }

            return result;
        }

        public IEnumerable<IThreatEventMitigation> GetThreatEventMitigations()
        {
            return GetThreatEventMitigations(null);
        }

        public IEnumerable<IThreatEventMitigation> GetThreatEventMitigations(IMitigation mitigation)
        {
            var result = new List<IThreatEventMitigation>();

            GetMitigations(this, mitigation, result);
            GetMitigations(_entities, mitigation, result);
            GetMitigations(_flows, mitigation, result);

            return result;
        }

        private int CountThreatTypes()
        {
            List<Guid> threatTypes = new List<Guid>();

            GetThreatTypes(_threatEvents, threatTypes);
            GetThreatTypes(_entities?.Select(x => x.ThreatEvents), threatTypes);
            GetThreatTypes(_flows?.Select(x => x.ThreatEvents), threatTypes);

            return threatTypes.Count();
        }

        private static void GetThreatTypes(
            IEnumerable<IEnumerable<IThreatEvent>> arraysOfEnumerables, List<Guid> threatTypes)
        {
            if (arraysOfEnumerables?.Any() ?? false)
            {
                foreach (var enumerables in arraysOfEnumerables)
                {
                    GetThreatTypes(enumerables, threatTypes);
                }
            }
        }

        private static void GetThreatTypes(IEnumerable<IThreatEvent> enumerables, List<Guid> threatTypes)
        {
            if (enumerables?.Any() ?? false)
            {
                foreach (var enumerable in enumerables)
                {
                    if (!threatTypes.Contains(enumerable.ThreatTypeId))
                        threatTypes.Add(enumerable.ThreatTypeId);
                }
            }
        }

        private int CountThreatEvents()
        {
            return (_threatEvents?.Count() ?? 0) +
                   (_entities?.Sum(x => x.ThreatEvents?.Count() ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.ThreatEvents?.Count() ?? 0) ?? 0);
        }

        private void GetThreatEvents(IEnumerable<IThreatEventsContainer> containers,
            IThreatType reference,
            [NotNull] List<IThreatEvent> list)
        {
            var cs = containers?.ToArray();
            if (cs?.Any() ?? false)
            {
                foreach (var container in cs)
                {
                    GetThreatEvents(container, reference, list);
                }
            }
        }

        private void GetThreatEvents(IThreatEventsContainer container,
            IThreatType reference,
            [NotNull] List<IThreatEvent> list)
        {
            var threats = container?.ThreatEvents?
                .Where(x => (reference == null) || (x.ThreatTypeId == reference.Id)).ToArray();
            if (threats?.Any() ?? false)
            {
                list.AddRange(threats);
            }
        }

        private static void GetMitigations(
            IEnumerable<IEnumerable<IEnumerable<IThreatEventMitigation>>> arrayOfArraysOfEnumerables,
            [NotNull] List<IMitigation> mitigations)
        {
            if (arrayOfArraysOfEnumerables?.Any() ?? false)
            {
                foreach (var arrayOfEnumerables in arrayOfArraysOfEnumerables)
                {
                    GetMitigations(arrayOfEnumerables, mitigations);
                }
            }
        }

        private static void GetMitigations(IEnumerable<IEnumerable<IThreatEventMitigation>> arrayOfEnumerables,
            [NotNull] List<IMitigation> mitigations)
        {
            if (arrayOfEnumerables?.Any() ?? false)
            {
                foreach (var enumerable in arrayOfEnumerables)
                {
                    if (enumerable?.Any() ?? false)
                    {
                        foreach (var mitigation in enumerable)
                        {
                            if (!mitigations.Contains(mitigation.Mitigation))
                                mitigations.Add(mitigation.Mitigation);
                        }
                    }
                }
            }
        }

        private void GetMitigations(IEnumerable<IThreatEventsContainer> containers, IMitigation reference,
            [NotNull] List<IThreatEventMitigation> list)
        {
            var cs = containers?.ToArray();
            if (cs?.Any() ?? false)
            {
                foreach (var container in cs)
                {
                    GetMitigations(container, reference, list);
                }
            }
        }

        private void GetMitigations(IThreatEventsContainer container, IMitigation reference,
            [NotNull] List<IThreatEventMitigation> list)
        {
            var threats = container?.ThreatEvents?.ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    var mitigations = threat.Mitigations?
                        .Where(x => (reference == null) || x.MitigationId == reference.Id).ToArray();
                    if (mitigations?.Any() ?? false)
                        list.AddRange(mitigations);
                }
            }
        }
        #endregion

        #region Advanced Weaknesses & Mitigations properties and functions.
        [IgnoreAutoChangeNotification]
        public int AssignedWeaknesses => CountWeaknesses();

        [IgnoreAutoChangeNotification]
        public int FullyMitigatedWeaknesses => _weaknesses?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) >= 100).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int PartiallyMitigatedWeaknesses
        {
            get
            {
                var result = 0;

                var weaknesses = _weaknesses?.ToArray();
                if (weaknesses?.Any() ?? false)
                {
                    foreach (var weakness in weaknesses)
                    {
                        var totalStrength = weakness.Mitigations?.Sum(x => x.StrengthId) ?? 0;
                        if (totalStrength > 0 && totalStrength < 100)
                            result++;
                    }
                }

                return result;
            }
        }

        [IgnoreAutoChangeNotification]
        public int NotMitigatedWeaknesses => _weaknesses?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) == 0).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int TotalVulnerabilities => CountVulnerabilities();

        [IgnoreAutoChangeNotification]
        public int FullyMitigatedVulnerabilities => GetVulnerabilities()?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) >= 100).Count() ?? 0;

        [IgnoreAutoChangeNotification]
        public int PartiallyMitigatedVulnerabilities
        {
            get
            {
                var result = 0;

                var vulnerabilities = GetVulnerabilities()?.ToArray();
                if (vulnerabilities?.Any() ?? false)
                {
                    foreach (var vulnerability in vulnerabilities)
                    {
                        var totalStrength = vulnerability.Mitigations?.Sum(x => x.StrengthId) ?? 0;
                        if (totalStrength > 0 && totalStrength < 100)
                            result++;
                    }
                }

                return result;
            }
        }

        [IgnoreAutoChangeNotification]
        public int NotMitigatedVulnerabilities => GetVulnerabilities()?
            .Where(x => (x.Mitigations?.Sum(y => y.StrengthId) ?? 0) == 0).Count() ?? 0;

        public int CountVulnerabilities([NotNull]  ISeverity severity)
        {
            return CountVulnerabilities(severity.Id);
        }

        public int CountVulnerabilities(int severityId)
        {
            return (_vulnerabilities?
                       .Where(x => x.Severity != null && x.SeverityId == severityId).Count() ?? 0) +
                   (_entities?.Sum(x => x.Vulnerabilities?
                       .Where(y => y.Severity != null && y.SeverityId == severityId)
                       .Count() ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.Vulnerabilities?
                       .Where(y => y.Severity != null && y.SeverityId == severityId)
                       .Count() ?? 0) ?? 0);
        }

        public int CountVulnerabilitiesByType([NotNull] ISeverity severity)
        {
            return CountVulnerabilitiesByType(severity.Id);
        }

        public int CountVulnerabilitiesByType(int severityId)
        {
            int result = 0;

            var types = _weaknesses?.ToArray();

            if (types?.Any() ?? false)
            {
                foreach (var type in types)
                {
                    var severity = type.GetTopSeverity();
                    if (severity != null && severity.Id == severityId)
                        result++;
                }
            }

            return result;
        }

        public IEnumerable<IVulnerability> GetVulnerabilities()
        {
            return GetVulnerabilities(null);
        }

        public IEnumerable<IVulnerability> GetVulnerabilities(IWeakness weakness)
        {
            var vulnerabilities = new List<IVulnerability>();

            GetVulnerabilities(this, weakness, vulnerabilities);
            GetVulnerabilities(_entities, weakness, vulnerabilities);
            GetVulnerabilities(_flows, weakness, vulnerabilities);

            return vulnerabilities;
        }

        public IEnumerable<IVulnerabilityMitigation> GetVulnerabilityMitigations()
        {
            return GetVulnerabilityMitigations(null);
        }

        public IEnumerable<IVulnerabilityMitigation> GetVulnerabilityMitigations(IMitigation mitigation)
        {
            var result = new List<IVulnerabilityMitigation>();

            GetMitigations(this, mitigation, result);
            GetMitigations(_entities, mitigation, result);
            GetMitigations(_flows, mitigation, result);

            return result;
        }

        private int CountWeaknesses()
        {
            var weaknesses = new List<Guid>();

            GetWeaknesses(_vulnerabilities, weaknesses);
            GetWeaknesses(_entities?.Select(x => x.Vulnerabilities), weaknesses);
            GetWeaknesses(_flows?.Select(x => x.Vulnerabilities), weaknesses);

            return weaknesses.Count();
        }

        private static void GetWeaknesses(
            IEnumerable<IEnumerable<IVulnerability>> arraysOfEnumerables, List<Guid> weaknesses)
        {
            if (arraysOfEnumerables?.Any() ?? false)
            {
                foreach (var enumerables in arraysOfEnumerables)
                {
                    GetWeaknesses(enumerables, weaknesses);
                }
            }
        }

        private static void GetWeaknesses(IEnumerable<IVulnerability> enumerables, List<Guid> weaknesses)
        {
            if (enumerables?.Any() ?? false)
            {
                foreach (var enumerable in enumerables)
                {
                    if (!weaknesses.Contains(enumerable.WeaknessId))
                        weaknesses.Add(enumerable.WeaknessId);
                }
            }
        }

        private int CountVulnerabilities()
        {
            return (_vulnerabilities?.Count() ?? 0) +
                   (_entities?.Sum(x => x.Vulnerabilities?.Count() ?? 0) ?? 0) +
                   (_flows?.Sum(x => x.Vulnerabilities?.Count() ?? 0) ?? 0);
        }

        private void GetVulnerabilities(IEnumerable<IVulnerabilitiesContainer> containers,
            IWeakness reference,
            [NotNull] List<IVulnerability> list)
        {
            var cs = containers?.ToArray();
            if (cs?.Any() ?? false)
            {
                foreach (var container in cs)
                {
                    GetVulnerabilities(container, reference, list);
                }
            }
        }

        private void GetVulnerabilities(IVulnerabilitiesContainer container,
            IWeakness reference,
            [NotNull] List<IVulnerability> list)
        {
            var threats = container?.Vulnerabilities?
                .Where(x => (reference == null) || (x.WeaknessId == reference.Id)).ToArray();
            if (threats?.Any() ?? false)
            {
                list.AddRange(threats);
            }
        }

        private static void GetMitigations(
            IEnumerable<IEnumerable<IEnumerable<IVulnerabilityMitigation>>> arrayOfArraysOfEnumerables,
            [NotNull] List<IMitigation> mitigations)
        {
            if (arrayOfArraysOfEnumerables?.Any() ?? false)
            {
                foreach (var arrayOfEnumerables in arrayOfArraysOfEnumerables)
                {
                    GetMitigations(arrayOfEnumerables, mitigations);
                }
            }
        }

        private static void GetMitigations(IEnumerable<IEnumerable<IVulnerabilityMitigation>> arrayOfEnumerables,
            [NotNull] List<IMitigation> mitigations)
        {
            if (arrayOfEnumerables?.Any() ?? false)
            {
                foreach (var enumerable in arrayOfEnumerables)
                {
                    if (enumerable?.Any() ?? false)
                    {
                        foreach (var mitigation in enumerable)
                        {
                            if (!mitigations.Contains(mitigation.Mitigation))
                                mitigations.Add(mitigation.Mitigation);
                        }
                    }
                }
            }
        }

        private void GetMitigations(IEnumerable<IVulnerabilitiesContainer> containers, IMitigation reference,
            [NotNull] List<IVulnerabilityMitigation> list)
        {
            var cs = containers?.ToArray();
            if (cs?.Any() ?? false)
            {
                foreach (var container in cs)
                {
                    GetMitigations(container, reference, list);
                }
            }
        }

        private void GetMitigations(IVulnerabilitiesContainer container, IMitigation reference,
            [NotNull] List<IVulnerabilityMitigation> list)
        {
            var vulnerabilities = container?.Vulnerabilities?.ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                foreach (var vulnerability in vulnerabilities)
                {
                    var mitigations = vulnerability.Mitigations?
                        .Where(x => (reference == null) || x.MitigationId == reference.Id).ToArray();
                    if (mitigations?.Any() ?? false)
                        list.AddRange(mitigations);
                }
            }
        }
        #endregion

        #region Implementation of IPropertyFinder.
        public IProperty FindProperty(Guid id)
        {
            IProperty result = ThreatTypes?.Select(x => x.Properties)
                                   .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                                   .FirstOrDefault(x => x.Id == id) ?? Properties.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                result = DataFlows?.Select(x => x.Properties)
                    .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                    .FirstOrDefault(x => x.Id == id);
            }
            
            if (result == null)
            {
                result = Entities?.Select(x => x.Properties)
                    .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                    .FirstOrDefault(x => x.Id == id);
            }

            if (result == null)
            {
                result = Groups?.Select(x => x.Properties)
                    .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                    .FirstOrDefault(x => x.Id == id);
            }
            
            if (result == null)
            {
                result = Mitigations?.Select(x => x.Properties)
                    .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                    .FirstOrDefault(x => x.Id == id);
            }

            if (result == null)
            {
                result = Severities?.Select(x => x.Properties)
                    .FirstOrDefault(x => x?.Any(y => y.Id == id) ?? false)?
                    .FirstOrDefault(x => x.Id == id);
            }

            return result;
        }
        #endregion

        #region Implementation of Threat Model Duplication.
        public IThreatModel Duplicate([Required] string name, [NotNull] DuplicationDefinition def)
        {
            if (!Validate(def, out var reasons))
                throw new InvalidDuplicationDefinitionException(reasons);

            ThreatModel result;

            try
            {
                //SuspendDirty();
                result = new ThreatModel(name);
                if (def.Contributors)
                    DuplicateContributors(result);
                if (def.Assumptions)
                    DuplicateAssumptions(result);
                if (def.Dependencies)
                    DuplicateDependencies(result);
                DuplicateSeverities(result, def.AllSeverities ? Severities?.Select(x => x.Id) : def.Severities);
                DuplicateStrengths(result, def.AllStrengths ? Strengths?.Select(x => x.Id) : def.Strengths);
                DuplicatePropertySchemas(result,
                    def.AllPropertySchemas ? Schemas?.Select(x => x.Id) : def.PropertySchemas);
                DuplicateProperties(result, def.AllProperties ? Properties?.Select(x => x.Id) : def.Properties);
                DuplicateThreatActors(result, def.AllThreatActors ? ThreatActors?.Select(x => x.Id) : def.ThreatActors);
                DuplicateMitigations(result, def.AllMitigations ? Mitigations?.Select(x => x.Id) : def.Mitigations);
                DuplicateThreatTypes(result, def.AllThreatTypes ? ThreatTypes?.Select(x => x.Id) : def.ThreatTypes);
                DuplicateGroups(result, def.AllGroups ? Groups?.Select(x => x.Id) : def.Groups);
                DuplicateEntityTemplates(result, def.AllEntityTemplates ? EntityTemplates?.Select(x => x.Id) : def.EntityTemplates);
                DuplicateFlowTemplates(result, def.AllFlowTemplates ? FlowTemplates?.Select(x => x.Id) : def.FlowTemplates);
                DuplicateTrustBoundaryTemplates(result, def.AllTrustBoundaryTemplates ? TrustBoundaryTemplates?.Select(x => x.Id) : def.TrustBoundaryTemplates);
                DuplicateEntities(result, def.AllEntities ? Entities?.Select(x => x.Id) : def.Entities);
                DuplicateDataFlows(result, def.AllDataFlows ? DataFlows?.Select(x => x.Id) : def.DataFlows);
                DuplicateDiagrams(result, def.AllDiagrams ? Diagrams?.Select(x => x.Id) : def.Diagrams);
            }
            finally
            {
                //ResumeDirty();
            }

            return result;
        }

        private void DuplicateContributors([NotNull] ThreatModel dest)
        {
            var list = Contributors?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    dest.AddContributor(item);
                }
            }
        }

        private void DuplicateAssumptions([NotNull] ThreatModel dest)
        {
            var list = Assumptions?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    dest.AddAssumption(item);
                }
            }
        }

        private void DuplicateDependencies([NotNull] ThreatModel dest)
        {
            var list = ExternalDependencies?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    dest.AddDependency(item);
                }
            }
        }

        private void DuplicateSeverities([NotNull] ThreatModel dest, IEnumerable<int> list)
        {
            var severities = Severities?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (severities?.Any() ?? false)
            {
                foreach (var severity in severities)
                {
                   severity.Clone(dest);
                }
            }
        }

        private void DuplicateStrengths([NotNull] ThreatModel dest, IEnumerable<int> list)
        {
            var strengths = Strengths?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (strengths?.Any() ?? false)
            {
                foreach (var strength in strengths)
                {
                    strength.Clone(dest);
                }
            }
        }

        private void DuplicatePropertySchemas([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var schemas = Schemas?.Where(x => !x.NotExportable && (list?.Contains(x.Id) ?? false)).ToArray();
            if (schemas?.Any() ?? false)
            {
                foreach (var schema in schemas)
                {
                    schema.Clone(dest);
                }
            }
        }

        private void DuplicateProperties([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var properties = Properties?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    var propertyType = dest.GetPropertyType(property.PropertyTypeId);
                    if (propertyType != null)
                        dest.AddProperty(propertyType, property.StringValue);
                }
            }
        }

        private void DuplicateThreatActors([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var actors = ThreatActors?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (actors?.Any() ?? false)
            {
                foreach (var actor in actors)
                {
                    actor.Clone(dest);
                }
            }
        }

        private void DuplicateMitigations([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var mitigations = Mitigations?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    mitigation.Clone(dest);
                }
            }
        }

        private void DuplicateThreatTypes([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var threatTypes = ThreatTypes?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    threatType.Clone(dest);
                }
            }
        }

        private void DuplicateGroups([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var groups = Groups?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    group.Clone(dest);
                }
            }
        }

        private void DuplicateEntityTemplates([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var templates = EntityTemplates?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var template in templates)
                {
                    template.Clone(dest);
                }
            }
        }

        private void DuplicateFlowTemplates([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var templates = FlowTemplates?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var template in templates)
                {
                    template.Clone(dest);
                }
            }
        }

        private void DuplicateTrustBoundaryTemplates([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var templates = TrustBoundaryTemplates?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var template in templates)
                {
                    template.Clone(dest);
                }
            }
        }

        private void DuplicateEntities([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var entities = Entities?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    entity.Clone(dest);
                }
            }
        }

        private void DuplicateDataFlows([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var dataFlows = DataFlows?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    dataFlow.Clone(dest);
                }
            }
        }

        private void DuplicateDiagrams([NotNull] ThreatModel dest, IEnumerable<Guid> list)
        {
            var diagrams = Diagrams?.Where(x => list?.Contains(x.Id) ?? false).ToArray();
            if (diagrams?.Any() ?? false)
            {
                foreach (var diagram in diagrams)
                {
                    diagram.Clone(dest);
                }
            }
        }

        private bool Validate([NotNull] DuplicationDefinition def, out IEnumerable<string> reasons)
        {
            bool result = true;

            var r = new List<string>();

            var known = new List<Guid>();
            var knownSeverities = _severities?
                .Where(x => def.AllSeverities || (def.Severities?.Contains(x.Id) ?? false))
                .Select(x => x.Id).ToArray();
            var knownStrengths = _strengths?
                .Where(x => def.AllStrengths || (def.Strengths?.Contains(x.Id) ?? false))
                .Select(x => x.Id).ToArray();

            AddIdentities(known, def.AllPropertySchemas, def.PropertySchemas, _schemas);

            if (!Check(known, def.AllProperties, def.Properties, _properties))
            {
                result = false;
                r.Add("One or more Threat Model Properties are associated to a Property Type which has not been selected.");
            }

            if (!Check(known, knownSeverities, knownStrengths, def.AllThreatActors, def.ThreatActors, _actors))
            {
                result = false;
                r.Add("One or more Threat Actors are associated to a Property Type which has not been selected.");
            }
            AddIdentities(known, def.AllThreatActors, def.ThreatActors, _actors);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllMitigations, def.Mitigations, _mitigations))
            {
                result = false;
                r.Add("One or more Mitigations are associated to a Property Type which has not been selected.");
            }
            AddIdentities(known, def.AllMitigations, def.Mitigations, _mitigations);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllThreatTypes, def.ThreatTypes, _threatTypes))
            {
                result = false;
                r.Add("One or more Threat Types are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllThreatTypes, def.ThreatTypes, _threatTypes);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllGroups, def.Groups, _groups))
            {
                result = false;
                r.Add("One or more Groups are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllGroups, def.Groups, _groups);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllEntityTemplates, def.EntityTemplates, _entityTemplates))
            {
                result = false;
                r.Add("One or more Entity Templates are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllEntityTemplates, def.EntityTemplates, _entityTemplates);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllFlowTemplates, def.FlowTemplates, _flowTemplates))
            {
                result = false;
                r.Add("One or more Flow Templates are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllFlowTemplates, def.FlowTemplates, _flowTemplates);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllTrustBoundaryTemplates, def.TrustBoundaryTemplates, _trustBoundaryTemplates))
            {
                result = false;
                r.Add("One or more Trust Boundary Templates are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllTrustBoundaryTemplates, def.TrustBoundaryTemplates, _trustBoundaryTemplates);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllEntities, def.Entities, _entities))
            {
                result = false;
                r.Add("One or more Entities are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllEntities, def.Entities, _entities);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllDataFlows, def.DataFlows, _flows))
            {
                result = false;
                r.Add("One or more Flows are associated to an object which has not been selected.");
            }
            AddIdentities(known, def.AllDataFlows, def.DataFlows, _flows);              

            if (!Check(known, knownSeverities, knownStrengths, def.AllDiagrams, def.Diagrams, _diagrams))
            {
                result = false;
                r.Add("One or more Diagrams are associated to an object which has not been selected.");
            }

            reasons = r;

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownSeverities, 
            IEnumerable<int> knownStrengths, bool all, IEnumerable<Guid> selected, 
            IEnumerable<IIdentity> identities)
        {
            bool result = true;

            var list = identities?.Where(x => all || (selected?.Contains(x.Id) ?? false)).ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (item is IPropertiesContainer container && !Check(known, true, null, container.Properties))
                    {
                        result = false;
                        break;
                    }

                    if (item is IEntitiesContainer eContainer && 
                        !Check(known, knownSeverities, eContainer.Entities))
                    {
                        result = false;
                        break;
                    }

                    if (item is IDataFlowsContainer dfContainer && 
                        !Check(known, knownSeverities, dfContainer.DataFlows))
                    {
                        result = false;
                        break;
                    }

                    if (item is IThreatType threatType && threatType.Severity is ISeverity severity &&
                        !(knownSeverities?.Contains(severity.Id) ?? false))
                    {
                        result = false;
                        break;
                    }

                    if (item is IThreatTypeMitigationsContainer ttmContainer &&
                        !Check(known, knownStrengths, ttmContainer.Mitigations))
                    {
                        result = false;
                        break;
                    }

                    if (item is IThreatEventsContainer teContainer &&
                        !Check(known, knownStrengths, teContainer.ThreatEvents))
                    {
                        result = false;
                        break;
                    }

                    if (item is IThreatEventMitigationsContainer tteContainer &&
                        !Check(known, knownStrengths, tteContainer.Mitigations))
                    {
                        result = false;
                        break;
                    }

                    if (item is IEntityShapesContainer esContainer && !Check(known, esContainer.Entities))
                    {
                        result = false;
                        break;
                    }

                    if (item is IGroupShapesContainer gsContainer && !Check(known, gsContainer.Groups))
                    {
                        result = false;
                        break;
                    }

                    if (item is ILinksContainer lContainer && !Check(known, lContainer.Links))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        
        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownStrengths, 
            IEnumerable<IThreatTypeMitigation> mitigations)
        {
            bool result = true;

            var list = mitigations?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.MitigationId) || 
                        (item.Strength is IStrength strength &&
                          !(knownStrengths?.Contains(strength.Id) ?? false)))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownSeverities, 
            IEnumerable<IEntity> entities)
        {
            bool result = true;

            var list = entities?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.ParentId) || !Check(known, knownSeverities, item.ThreatEvents))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        
        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownSeverities, 
            IEnumerable<IDataFlow> dataFlows)
        {
            bool result = true;

            var list = dataFlows?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.SourceId) || !known.Contains(item.TargetId) || 
                        !Check(known, knownSeverities, item.ThreatEvents))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownSeverities, 
            IEnumerable<IThreatEvent> threatEvents)
        {
            bool result = true;

            var list = threatEvents?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.ThreatTypeId) || 
                        (item.Severity is ISeverity severity && 
                          !(knownSeverities?.Contains(severity.Id) ?? false)))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<int> knownStrengths,
            IEnumerable<IThreatEventMitigation> mitigations)
        {
            bool result = true;

            var list = mitigations?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.MitigationId) ||
                          !(knownStrengths?.Contains(item.StrengthId) ?? false))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<IEntityShape> container)
        {
            bool result = true;

            var list = container?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.AssociatedId))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<IGroupShape> container)
        {
            bool result = true;

            var list = container?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.AssociatedId))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, IEnumerable<ILink> container)
        {
            bool result = true;

            var list = container?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.AssociatedId))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool Check([NotNull] List<Guid> known, bool all, IEnumerable<Guid> selected, IEnumerable<IProperty> properties)
        {
            bool result = true;

            var list = properties?.Where(x => (all || (selected?.Contains(x.Id) ?? false)) && this.GetPropertyType(x.PropertyTypeId) != null).ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var property in list)
                {
                    var schema = this.GetSchema(property.PropertyType.SchemaId);

                    if (!known.Contains(property.PropertyTypeId) && !schema.NotExportable)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private void AddIdentities([NotNull] List<Guid> known, 
            bool all, IEnumerable<Guid> selected, IEnumerable<IIdentity> identities)
        {
            var list = identities?.Where(x => all || (selected?.Contains(x.Id) ?? false)).ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    if (!known.Contains(item.Id))
                    {
                        known.Add(item.Id);
                        if (item is IPropertySchema schema)
                        {
                            AddIdentities(known, true, null, schema.PropertyTypes);
                        }
                    }
                }
            }
        }
        #endregion

        #region Implementation of Threat Model Merge.
        public bool Merge([NotNull] IThreatModel source, [NotNull] DuplicationDefinition def)
        {
            bool result = false;

            if (source is ThreatModel sourceThreatModel)
            {
                if (sourceThreatModel.Validate(def, out var reasons))
                {
                    result = true;
                    MergeSchemas(source, def.AllPropertySchemas, def.PropertySchemas);
                    MergeEntityTemplates(source, def.AllEntityTemplates, def.EntityTemplates);
                    MergeFlowTemplates(source, def.AllFlowTemplates, def.FlowTemplates);
                    MergeTrustBoundaryTemplates(source, def.AllTrustBoundaryTemplates, def.TrustBoundaryTemplates);
                    MergeSeverities(source, def.AllSeverities, def.Severities);
                    MergeStrengths(source, def.AllSeverities, def.Severities);
                    MergeThreatActors(source, def.AllThreatActors, def.ThreatActors);
                    MergeMitigations(source, def.AllMitigations, def.Mitigations);
                    MergeThreatTypes(source, def.AllThreatTypes, def.ThreatTypes);
                }
            }

            return result;
        }

        private void MergeSchemas([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.Schemas?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var schema in selected)
                {
                    MergeSchema(schema);
                }
            }
        }

        private void MergeSchema([NotNull] IPropertySchema schema)
        {
            var existing = GetSchema(schema.Name, schema.Namespace);
            if (existing == null)
            {
                schema.Clone(this);
            }
            else
            {
                existing.MergePropertyTypes(schema);
            }

            if (schema.AutoApply)
            {
                ApplySchema(schema.Id);
            }
        }

        private void MergeEntityTemplates([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.EntityTemplates?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var entityTemplate in selected)
                {
                    MergeEntityTemplate(entityTemplate);
                }
            }
        }

        private void MergeEntityTemplate([NotNull] IEntityTemplate entityTemplate)
        {
            var existing = EntityTemplates?.FirstOrDefault(x => string.CompareOrdinal(x.Name, entityTemplate.Name) == 0);
            if (existing == null)
            {
                entityTemplate.Clone(this);
            }
            else
            {
                existing.MergeProperties(entityTemplate);
            }
        }

        private void MergeFlowTemplates([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.FlowTemplates?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var flowTemplate in selected)
                {
                    MergeFlowTemplate(flowTemplate);
                }
            }
        }

        private void MergeFlowTemplate([NotNull] IFlowTemplate flowTemplate)
        {
            var existing = FlowTemplates?.FirstOrDefault(x => string.CompareOrdinal(x.Name, flowTemplate.Name) == 0);
            if (existing == null)
            {
                flowTemplate.Clone(this);
            }
            else
            {
                existing.MergeProperties(flowTemplate);
            }
        }

        private void MergeTrustBoundaryTemplates([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.TrustBoundaryTemplates?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var trustBoundaryTemplate in selected)
                {
                    MergeTrustBoundaryTemplate(trustBoundaryTemplate);
                }
            }
        }

        private void MergeTrustBoundaryTemplate([NotNull] ITrustBoundaryTemplate trustBoundaryTemplate)
        {
            var existing = TrustBoundaryTemplates?.FirstOrDefault(x => string.CompareOrdinal(x.Name, trustBoundaryTemplate.Name) == 0);
            if (existing == null)
            {
                trustBoundaryTemplate.Clone(this);
            }
            else
            {
                existing.MergeProperties(trustBoundaryTemplate);
            }
        }

        private void MergeSeverities([NotNull] IThreatModel source, bool all, IEnumerable<int> ids)
        {
            var selected = source.Severities?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var severity in selected)
                {
                    MergeSeverity(severity);
                }
            }
        }

        private void MergeSeverity([NotNull] ISeverity severity)
        {
            var existing = Severities?.FirstOrDefault(x => x.Id == severity.Id);
            if (existing == null)
            {
                severity.Clone(this);
            }
            else
            {
                existing.MergeProperties(severity);
            }
        }

        private void MergeStrengths([NotNull] IThreatModel source, bool all, IEnumerable<int> ids)
        {
            var selected = source.Strengths?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var strength in selected)
                {
                    MergeStrength(strength);
                }
            }
        }

        private void MergeStrength([NotNull] IStrength strength)
        {
            var existing = Strengths?.FirstOrDefault(x => x.Id == strength.Id);
            if (existing == null)
            {
                strength.Clone(this);
            }
            else
            {
                existing.MergeProperties(strength);
            }
        }

        private void MergeThreatActors([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.ThreatActors?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var threatActor in selected)
                {
                    MergeThreatActor(threatActor);
                }
            }
        }

        private void MergeThreatActor([NotNull] IThreatActor threatActor)
        {
            var existing = ThreatActors?.FirstOrDefault(x => string.CompareOrdinal(x.Name, threatActor.Name) == 0);
            if (existing == null)
            {
                threatActor.Clone(this);
            }
            else
            {
                existing.MergeProperties(threatActor);
            }
        }

        private void MergeMitigations([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.Mitigations?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var mitigation in selected)
                {
                    MergeMitigation(mitigation);
                }
            }
        }

        private void MergeMitigation([NotNull] IMitigation mitigation)
        {
            var existing = Mitigations?.FirstOrDefault(x => string.CompareOrdinal(x.Name, mitigation.Name) == 0);
            if (existing == null)
            {
                mitigation.Clone(this);
            }
            else
            {
                existing.MergeProperties(mitigation);
            }
        }

        private void MergeThreatTypes([NotNull] IThreatModel source, bool all, IEnumerable<Guid> ids)
        {
            var selected = source.ThreatTypes?.Where(x => all || (ids?.Contains(x.Id) ?? false)).ToArray();
            if (selected?.Any() ?? false)
            {
                foreach (var threatType in selected)
                {
                    MergeThreatType(threatType);
                }
            }
        }

        private void MergeThreatType([NotNull] IThreatType threatType)
        {
            var existing = ThreatTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, threatType.Name) == 0);
            if (existing == null)
            {
                threatType.Clone(this);
            }
            else
            {
                existing.MergeProperties(threatType);

                var mitigations = threatType.Mitigations?.ToArray();
                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        var m = GetMitigation(mitigation.MitigationId) ??
                                Mitigations?.FirstOrDefault(x =>
                                    string.CompareOrdinal(x.Name, mitigation.Mitigation.Name) == 0);
                        var s = GetStrength(mitigation.StrengthId);
                        if (m != null && s != null)
                        {
                            var em = existing.Mitigations?.FirstOrDefault(x => x.MitigationId == m.Id) ?? 
                                     existing.AddMitigation(m, s);
                            em.MergeProperties(mitigation);
                        }
                    }
                }
            }
        }
        #endregion

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }
        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            return null;
        }
        public bool RemoveProperty(IPropertyType propertyType)
        {
            return false;
        }
        public bool RemoveProperty(Guid propertyTypeId)
        {
            return false;
        }

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }

        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IThreatEvent> ThreatEvents { get; }
        public IThreatEvent GetThreatEvent(Guid id)
        {
            return null;
        }

        public IThreatEvent GetThreatEventByThreatType(Guid threatTypeId)
        {
            return null;
        }

        public void Add(IThreatEvent threatEvent)
        {
        }

        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            return null;
        }

        public bool RemoveThreatEvent(Guid id)
        {
            return false;
        }

        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved;
        [Reference]
        [field:NotRecorded]
        public IEnumerable<IVulnerability> Vulnerabilities { get; }
        public IVulnerability GetVulnerability(Guid id)
        {
            return null;
        }

        public IVulnerability GetVulnerabilityByWeakness(Guid weaknessId)
        {
            return null;
        }

        public void Add(IVulnerability vulnerability)
        {
        }

        public IVulnerability AddVulnerability(IWeakness weakness)
        {
            return null;
        }

        public bool RemoveVulnerability(Guid id)
        {
            return false;
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("id", Order = 1)]
        protected Guid _id { get; set; }
        [JsonProperty("name", Order = 2)]
        protected string _name { get; set; }
        [JsonProperty("description", Order = 3)]
        protected string _description { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects, Order = 10)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [Child]
        [JsonProperty("threatEvents", Order = 11)]
        private AdvisableCollection<ThreatEvent> _threatEvents { get; set; }
        [Child]
        [JsonProperty("vulnerabilities", Order = 12)]
        private AdvisableCollection<Vulnerability> _vulnerabilities { get; set; }
        #endregion
    }
}
