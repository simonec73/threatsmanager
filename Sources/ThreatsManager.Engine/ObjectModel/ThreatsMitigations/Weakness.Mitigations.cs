using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
    public partial class Weakness
    {
        private Action<IWeaknessMitigationsContainer, IWeaknessMitigation> _weaknessMitigationAdded;
        public event Action<IWeaknessMitigationsContainer, IWeaknessMitigation> WeaknessMitigationAdded
        {
            add
            {
                if (_weaknessMitigationAdded == null || !_weaknessMitigationAdded.GetInvocationList().Contains(value))
                {
                    _weaknessMitigationAdded += value;
                }
            }
            remove
            {
                _weaknessMitigationAdded -= value;
            }
        }

        private Action<IWeaknessMitigationsContainer, IWeaknessMitigation> _weaknessMitigationRemoved;
        public event Action<IWeaknessMitigationsContainer, IWeaknessMitigation> WeaknessMitigationRemoved
        {
            add
            {
                if (_weaknessMitigationRemoved == null || !_weaknessMitigationRemoved.GetInvocationList().Contains(value))
                {
                    _weaknessMitigationRemoved += value;
                }
            }
            remove
            {
                _weaknessMitigationRemoved -= value;
            }
        }

        [JsonProperty("mitigations")]
        private List<IWeaknessMitigation> _mitigations;

        [InitializationRequired]
        public IEnumerable<IWeaknessMitigation> Mitigations => _mitigations?.AsReadOnly();

        [InitializationRequired]
        public IWeaknessMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [InitializationRequired]
        public IWeaknessMitigation AddMitigation([NotNull] IMitigation mitigation, IStrength strength)
        {
            IWeaknessMitigation result = null;

            if (GetMitigation(mitigation.Id) == null)
            {
                result = new WeaknessMitigation(Model, this, mitigation, strength);
                if (_mitigations == null)
                    _mitigations = new List<IWeaknessMitigation>();
                _mitigations.Add(result);
                SetDirty();
                _weaknessMitigationAdded?.Invoke(this, result);
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
                    _weaknessMitigationRemoved?.Invoke(this, mitigation);
                }
            }

            return result;
        }
    }
}