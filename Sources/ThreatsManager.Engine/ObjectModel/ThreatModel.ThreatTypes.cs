using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("threatTypes", Order = 40)]
        private AdvisableCollection<ThreatType> _threatTypes { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IThreatType> ThreatTypes => _threatTypes?.AsEnumerable();

        [InitializationRequired]
        public IEnumerable<IThreatType> SearchThreatTypes(string filter)
        {
            IEnumerable<IThreatType> result = null;

            Dictionary<Guid, int> points = new Dictionary<Guid, int>();
            var threats = _threatTypes?.ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        points[threat.Id] = Matches(threat, filter);
                }

                result = points.Where(x => x.Value > 0).OrderByDescending(x => x.Value)
                    .Select(x => threats.FirstOrDefault(y => y.Id == x.Key));
            }

            return result;
        }

        private int Matches([NotNull] IThreatType threatType, [Required] string filter)
        {
            int result = 0;

            if ((threatType.Name?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;
            if ((threatType.Description?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;

            var properties = threatType.Properties?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if ((property.StringValue?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                        result++;

                    if (property is IPropertyTokens propertyTokens)
                    {
                        var values = propertyTokens.Value?.ToArray();
                        if (values?.Any() ?? false)
                        {
                            foreach (var value in values)
                            {
                                if (string.Compare(filter, value, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    result += 10;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public IThreatType GetThreatType(Guid id)
        {
            return _threatTypes?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IThreatType GetThreatType([Required] string name)
        {
            return _threatTypes?.FirstOrDefault(x => name.IsEqual(x.Name));
        }

        [InitializationRequired]
        public void Add([NotNull] IThreatType threatType)
        {
            if (threatType is ThreatType tt)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Threat Type"))
                {
                    if (_threatTypes == null)
                        _threatTypes = new AdvisableCollection<ThreatType>();

                    _threatTypes.Add(tt);
                    UndoRedoManager.Attach(tt);
                    scope.Complete();

                    ChildCreated?.Invoke(tt);
                }
            }
            else
                throw new ArgumentException(nameof(threatType));
        }

        [InitializationRequired]
        public IThreatType AddThreatType([Required] string name, [NotNull] ISeverity severity)
        {
            IThreatType result = null;

            if (GetThreatType(name) == null)
            {
                result = new ThreatType(name, severity);
                Add(result);
                RegisterEvents(result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveThreatType(Guid id, bool force = false)
        {
            bool result = false;

            var threatType = GetThreatType(id) as ThreatType;

            if (threatType != null && (force || !IsUsed(threatType)))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Threat Type"))
                {
                    RemoveRelated(threatType);

                    result = _threatTypes.Remove(threatType);
                    if (result)
                    {
                        UndoRedoManager.Detach(threatType);
                        UnregisterEvents(threatType);
                        ChildRemoved?.Invoke(threatType);
                    }

                    scope.Complete();
                }
            }

            return result;
        }
 
        private bool IsUsed([NotNull] IThreatType threatType)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false) ?? false) ||
                   (_flows?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false) ?? false) || 
                   (ThreatEvents?.Any(x => x.ThreatTypeId == threatType.Id) ?? false);
        }

        private void RemoveRelated([NotNull] IThreatType threatType)
        {
            RemoveRelated(threatType, _entities);
            RemoveRelated(threatType, _flows);
            RemoveRelated(threatType, this);
        }

        private void RemoveRelated([NotNull] IThreatType threatType, IEnumerable<IThreatEventsContainer> containers)
        {
            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                {
                    RemoveRelated(threatType, container);
                }
            }
        }

        private void RemoveRelated([NotNull] IThreatType threatType, IThreatEventsContainer container)
        {
            var events = container?.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
            if (events?.Any() ?? false)
            {
                foreach (var threatEvent in events)
                {
                    container.RemoveThreatEvent(threatEvent.Id);
                }
            }
        }
    }
}
