﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("strengths", Order = 52)]
        private AdvisableCollection<StrengthDefinition> _strengths { get; set; }

        private Action<IStrength> _strengthCreated;
        public event Action<IStrength> StrengthCreated
        {
            add
            {
                if (_strengthCreated == null || !_strengthCreated.GetInvocationList().Contains(value))
                {
                    _strengthCreated += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_strengthCreated != null) _strengthCreated -= value;
            }
        }

        private Action<IStrength> _strengthRemoved;
        public event Action<IStrength> StrengthRemoved
        {
            add
            {
                if (_strengthRemoved == null || !_strengthRemoved.GetInvocationList().Contains(value))
                {
                    _strengthRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_strengthRemoved != null) _strengthRemoved -= value;
            }
        }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IStrength> Strengths => _strengths?.OrderByDescending(x => x.Id);

        [InitializationRequired]
        public IStrength GetStrength(int id)
        {
            return _strengths?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IStrength GetMappedStrength(int id)
        {
            IStrength result = null;

            if (_strengths?.Any() ?? false)
            {
                var strengths = _strengths?.OrderBy(x => x.Id).ToArray();
                foreach (var strength in strengths)
                {
                    if (strength.Id >= id)
                    {
                        result = strength;
                        break;
                    }
                }

                if (result == null)
                    result = strengths.LastOrDefault();
            }
            
            return result;
        }
        
        [InitializationRequired]
        public void Add([NotNull] IStrength strength)
        {
            if (strength is StrengthDefinition sd)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Strength"))
                {
                    if (_strengths == null)
                        _strengths = new AdvisableCollection<StrengthDefinition>();

                    UndoRedoManager.Attach(sd, this);
                    _strengths.Add(sd);
                    scope?.Complete();
                }

                _strengthCreated?.Invoke(sd);
            }
            else
                throw new ArgumentException(nameof(strength));
        }

        [InitializationRequired]
        public IStrength AddStrength([Range(0, 100)] int id, [Required] string name)
        {
            IStrength result = null;

            if (!(_strengths?.Any(x => x.Id == id) ?? false))
            {
                result = new StrengthDefinition(id, name);
                Add(result);
            }

            return result;
        }

        [InitializationRequired]
        public IStrength AddStrength(DefaultStrength defaultStrength)
        {
            var strength = AddStrength((int) defaultStrength, defaultStrength.GetEnumLabel());
            if (strength != null)
            {
                strength.Description = defaultStrength.GetEnumDescription();
                strength.Visible = defaultStrength.IsUIVisible();
            }

            return strength;
        }

        [InitializationRequired]
        public bool RemoveStrength(int id)
        {
            bool result = false;

            var definition = GetStrength(id) as StrengthDefinition;
            if (definition != null && !IsUsed(definition))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Strength"))
                {
                    result = _strengths.Remove(definition);
                    if (result)
                    {
                        UndoRedoManager.Detach(definition);
                        scope?.Complete();
                    }
                }

                if (result)
                    _strengthRemoved?.Invoke(definition);
            }

            return result;
        }

        [InitializationRequired]
        public void InitializeStandardStrengths()
        {
            using (var scope = UndoRedoManager.OpenScope("Initialize Standard Strengths"))
            {
                if (_strengths == null)
                    _strengths = new AdvisableCollection<StrengthDefinition>();

                var values = Enum.GetValues(typeof(DefaultStrength));
                foreach (var value in values)
                {
                    AddStrength((DefaultStrength)value);
                }

                scope?.Complete();
            }
        }
        
        private bool IsUsed([NotNull] IStrength strength)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false) ?? false) ||
                   (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Vulnerabilities?.Any(z => z.Mitigations?.Any(t => t.StrengthId == strength.Id) ?? false) ?? false) ?? false) ?? false) || 
                   (_entities?.Any(x => x.Vulnerabilities?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false) ?? false) ||
                   (_flows?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false) ?? false) ||
                   (_flows?.Any(x => x.ThreatEvents?.Any(y => y.Vulnerabilities?.Any(z => z.Mitigations?.Any(t => t.StrengthId == strength.Id) ?? false) ?? false) ?? false) ?? false) ||
                   (_flows?.Any(x => x.Vulnerabilities?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false) ?? false) ||
                   (_threatEvents?.Any(x => x.Mitigations?.Any(y => y.StrengthId == strength.Id) ?? false) ?? false) ||
                   (_threatEvents?.Any(x => x.Vulnerabilities?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false) ?? false) ||
                   (_vulnerabilities?.Any(x => x.Mitigations?.Any(y => y.StrengthId == strength.Id) ?? false) ?? false) ||
                   (_threatTypes?.Any(x => x.Mitigations?.Any(y => y.StrengthId == strength.Id) ?? false) ?? false) ||
                   (_weaknesses?.Any(x => x.Mitigations?.Any(y => y.StrengthId == strength.Id) ?? false) ?? false);
        }
    }
}
