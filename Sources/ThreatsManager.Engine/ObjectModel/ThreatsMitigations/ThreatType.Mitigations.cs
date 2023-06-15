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

        [Child]
        [JsonProperty("mitigations")]
        private AdvisableCollection<ThreatTypeMitigation> _mitigations { get; set; }

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IEnumerable<IThreatTypeMitigation> Mitigations => _mitigations?.AsEnumerable();

        [InitializationRequired]
        public IThreatTypeMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [InitializationRequired]
        public void Add([NotNull] IThreatTypeMitigation mitigation)
        {
            if (mitigation is ThreatTypeMitigation ttm)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Mitigation to Threat Type"))
                {
                    if (_mitigations == null)
                        _mitigations = new AdvisableCollection<ThreatTypeMitigation>();

                    _mitigations.Add(ttm);
                    UndoRedoManager.Attach(ttm);
                    scope.Complete();
                }
            }
            else
                throw new ArgumentException(nameof(mitigation));
        }

        [InitializationRequired]
        public IThreatTypeMitigation AddMitigation([NotNull] IMitigation mitigation, IStrength strength)
        {
            IThreatTypeMitigation result = null;

            if (GetMitigation(mitigation.Id) == null)
            {
                result = new ThreatTypeMitigation(this, mitigation, strength);
                Add(result);
                _threatTypeMitigationAdded?.Invoke(this, result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveMitigation(Guid mitigationId)
        {
            bool result = false;

            var mitigation = GetMitigation(mitigationId) as ThreatTypeMitigation;
            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation from Threat Type"))
                {
                    result = _mitigations.Remove(mitigation);
                    if (result)
                    {
                        UndoRedoManager.Detach(mitigation);
                        scope.Complete();

                        _threatTypeMitigationRemoved?.Invoke(this, mitigation);
                    }
                }
            }

            return result;
        }
    }
}