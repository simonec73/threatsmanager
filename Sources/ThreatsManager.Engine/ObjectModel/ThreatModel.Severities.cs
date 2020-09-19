using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [JsonProperty("severities")]
        private List<ISeverity> _severities;

        private Action<ISeverity> _severityCreated;
        public event Action<ISeverity> SeverityCreated
        {
            add
            {
                if (_severityCreated == null || !_severityCreated.GetInvocationList().Contains(value))
                {
                    _severityCreated += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_severityCreated != null) _severityCreated -= value;
            }
        }

        private Action<ISeverity> _severityRemoved;
        public event Action<ISeverity> SeverityRemoved
        {
            add
            {
                if (_severityRemoved == null || !_severityRemoved.GetInvocationList().Contains(value))
                {
                    _severityRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_severityRemoved != null) _severityRemoved -= value;
            }
        }

        public IEnumerable<ISeverity> Severities => _severities?.OrderByDescending(x => x.Id);

        [InitializationRequired]
        public ISeverity GetSeverity(int id)
        {
            return _severities?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public ISeverity GetMappedSeverity(int id)
        {
            ISeverity result = null;

            if (_severities?.Any() ?? false)
            {
                var severities = _severities?.Where(x => x.Visible).OrderBy(x => x.Id).ToArray();
                foreach (var severity in severities)
                {
                    if (severity.Id >= id)
                    {
                        result = severity;
                        break;
                    }
                }

                if (result == null)
                    result = severities.LastOrDefault();
            }
            
            return result;
        }

        [InitializationRequired]
        public void Add([NotNull] ISeverity severity)
        {
            if (_severities == null)
                _severities = new List<ISeverity>();

            _severities.Add(severity);

            SetDirty();
            _severityCreated?.Invoke(severity);
        }

        [InitializationRequired]
        public ISeverity AddSeverity([Range(0, 100)] int id, [Required] string name)
        {
            ISeverity result = null;

            if (!(_severities?.Any(x => x.Id == id) ?? false))
            {
                result = new SeverityDefinition(this, id, name);
                Add(result);
                RegisterEvents(result);
            }

            return result;
        }

        [InitializationRequired]
        public ISeverity AddSeverity(DefaultSeverity defaultSeverity)
        {
            var severity = AddSeverity((int) defaultSeverity, defaultSeverity.GetEnumLabel());
            if (severity != null)
            {
                severity.Description = defaultSeverity.GetEnumDescription();
                severity.Visible = defaultSeverity.IsUIVisible();
            }

            return severity;
        }

        [InitializationRequired]
        public bool RemoveSeverity(int id)
        {
            bool result = false;

            var definition = GetSeverity(id);
            if (definition != null && !IsUsed(definition))
            {
                result = _severities.Remove(definition);
                if (result)
                {
                    UnregisterEvents(definition);
                    SetDirty();
                    _severityRemoved?.Invoke(definition);
                }
            }

            return result;
        }

        [InitializationRequired]
        public void InitializeStandardSeverities()
        {
            var values = Enum.GetValues(typeof(DefaultSeverity));
            foreach (var value in values)
            {
                AddSeverity((DefaultSeverity) value);
            }
        }
        
        private bool IsUsed([NotNull] ISeverity severity)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Severity == severity) ?? false) ?? false) ||
                   (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Scenarios?.Any(z => z.Severity == severity) ?? false) ?? false) ?? false) ||
                   (_dataFlows?.Any(x => x.ThreatEvents?.Any(y => y.Severity == severity) ?? false) ?? false) ||
                   (_dataFlows?.Any(x => x.ThreatEvents?.Any(y => y.Scenarios?.Any(z => z.Severity == severity) ?? false) ?? false) ?? false) ||
                   (_threatTypes?.Any(x => x.Severity == severity) ?? false);
        }
    }
}
