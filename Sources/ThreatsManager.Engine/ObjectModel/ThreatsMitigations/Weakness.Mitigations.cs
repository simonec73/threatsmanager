using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
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

        [Child]
        [JsonProperty("mitigations")]
        private AdvisableCollection<WeaknessMitigation> _mitigations { get; set; }

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IEnumerable<IWeaknessMitigation> Mitigations => _mitigations?.AsEnumerable();

        [InitializationRequired]
        public IWeaknessMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [InitializationRequired]
        public void Add([NotNull] IWeaknessMitigation mitigation)
        {
            if (mitigation is WeaknessMitigation wm)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Mitigation to Weakness"))
                {
                    if (_mitigations == null)
                        _mitigations = new AdvisableCollection<WeaknessMitigation>();

                    _mitigations.Add(wm);
                    UndoRedoManager.Attach(wm);
                    scope.Complete();
                }
            }
            else
                throw new ArgumentException(nameof(mitigation));
        }

        [InitializationRequired]
        public IWeaknessMitigation AddMitigation([NotNull] IMitigation mitigation, IStrength strength)
        {
            IWeaknessMitigation result = null;

            if (GetMitigation(mitigation.Id) == null)
            {
                result = new WeaknessMitigation(Model, this, mitigation, strength);
                Add(result);
                _weaknessMitigationAdded?.Invoke(this, result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveMitigation(Guid mitigationId)
        {
            bool result = false;

            var mitigation = GetMitigation(mitigationId) as WeaknessMitigation;
            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation from Weakness"))
                {
                    result = _mitigations.Remove(mitigation);
                    if (result)
                    {
                        UndoRedoManager.Detach(mitigation);
                        scope.Complete();

                        _weaknessMitigationRemoved?.Invoke(this, mitigation);
                    }
                }
            }

            return result;
        }
    }
}