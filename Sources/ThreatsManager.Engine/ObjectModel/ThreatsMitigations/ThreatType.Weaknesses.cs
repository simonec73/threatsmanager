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
        private Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> _threatTypeWeaknessAdded;
        public event Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> ThreatTypeWeaknessAdded
        {
            add
            {
                if (_threatTypeWeaknessAdded == null || !_threatTypeWeaknessAdded.GetInvocationList().Contains(value))
                {
                    _threatTypeWeaknessAdded += value;
                }
            }
            remove
            {
                _threatTypeWeaknessAdded -= value;
            }
        }

        private Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> _threatTypeWeaknessRemoved;
        public event Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> ThreatTypeWeaknessRemoved
        {
            add
            {
                if (_threatTypeWeaknessRemoved == null || !_threatTypeWeaknessRemoved.GetInvocationList().Contains(value))
                {
                    _threatTypeWeaknessRemoved += value;
                }
            }
            remove
            {
                _threatTypeWeaknessRemoved -= value;
            }
        }

        [Child]
        [JsonProperty("weaknesses")]
        private AdvisableCollection<ThreatTypeWeakness> _weaknesses { get; set; }

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IEnumerable<IThreatTypeWeakness> Weaknesses => _weaknesses?.AsEnumerable();

        [InitializationRequired]
        public IThreatTypeWeakness GetWeakness(Guid weaknessId)
        {
            return _weaknesses?.FirstOrDefault(x => x.WeaknessId == weaknessId);
        }

        [InitializationRequired]
        public void Add([NotNull] IThreatTypeWeakness weakness)
        {
            if (weakness is ThreatTypeWeakness ttw)
            { 
                using (var scope = UndoRedoManager.OpenScope("Add Weakness to Threat Type"))
                {
                    if (_weaknesses == null)
                        _weaknesses = new AdvisableCollection<ThreatTypeWeakness>();

                    UndoRedoManager.Attach(ttw, Model);
                    _weaknesses.Add(ttw);
                    scope?.Complete();
                }
            }
            else
                throw new ArgumentException(nameof(weakness));
        }

        [InitializationRequired]
        public IThreatTypeWeakness AddWeakness([NotNull] IWeakness weakness)
        {
            IThreatTypeWeakness result = null;

            if (GetMitigation(weakness.Id) == null)
            {
                result = new ThreatTypeWeakness(this, weakness);
                Add(result);
                _threatTypeWeaknessAdded?.Invoke(this, result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveWeakness(Guid weaknessId)
        {
            bool result = false;

            var weakness = GetWeakness(weaknessId) as ThreatTypeWeakness;
            if (weakness != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Weakness from Threat Type"))
                {
                    result = _weaknesses.Remove(weakness);
                    if (result)
                    {
                        UndoRedoManager.Detach(weakness);
                        scope?.Complete();
                    }
                }

                if (result)
                    _threatTypeWeaknessRemoved?.Invoke(this, weakness);
            }

            return result;
        }
    }
}