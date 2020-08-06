using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
    public partial class ThreatType
    {
        private Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> _threatTypeMitigationAdded;
        public event Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> ThreatTypeMitigationAdded
        {
            add
            {
                if (_threatTypeMitigationAdded == null || !_threatTypeMitigationAdded.GetInvocationList().Contains(value))
                {
                    _threatTypeMitigationAdded += value;
                }
            }
            remove
            {
                _threatTypeMitigationAdded -= value;
            }
        }

        private Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> _threatTypeMitigationRemoved;
        public event Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> ThreatTypeMitigationRemoved
        {
            add
            {
                if (_threatTypeMitigationRemoved == null || !_threatTypeMitigationRemoved.GetInvocationList().Contains(value))
                {
                    _threatTypeMitigationRemoved += value;
                }
            }
            remove
            {
                _threatTypeMitigationRemoved -= value;
            }
        }

        [JsonProperty("mitigations")]
        private List<IThreatTypeMitigation> _mitigations;

        [InitializationRequired]
        public IEnumerable<IThreatTypeMitigation> Mitigations => _mitigations?.AsReadOnly();

        [InitializationRequired]
        public IThreatTypeMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [InitializationRequired]
        public IThreatTypeMitigation AddMitigation([NotNull] IMitigation mitigation, IStrength strength)
        {
            IThreatTypeMitigation result = null;

            if (GetMitigation(mitigation.Id) == null)
            {
                result = new ThreatTypeMitigation(Model, this, mitigation, strength);
                if (_mitigations == null)
                    _mitigations = new List<IThreatTypeMitigation>();
                _mitigations.Add(result);
                SetDirty();
                _threatTypeMitigationAdded?.Invoke(this, result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveMitigation(Guid mitigationId)
        {
            bool result = false;

            var mitigation = GetMitigation(mitigationId);
            if (mitigation != null)
            {
                result = _mitigations.Remove(mitigation);
                if (result)
                {
                    SetDirty();
                    _threatTypeMitigationRemoved?.Invoke(this, mitigation);
                }
            }

            return result;
        }
    }
}