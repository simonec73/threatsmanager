using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("trustBoundaryTemplates", Order = 47)]
        private AdvisableCollection<TrustBoundaryTemplate> _trustBoundaryTemplates { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<ITrustBoundaryTemplate> TrustBoundaryTemplates => _trustBoundaryTemplates?.AsEnumerable();

        [InitializationRequired]
        public ITrustBoundaryTemplate GetTrustBoundaryTemplate(Guid id)
        {
            return _trustBoundaryTemplates?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public void Add([NotNull] ITrustBoundaryTemplate trustBoundaryTemplate)
        {
            if (trustBoundaryTemplate is TrustBoundaryTemplate tbt)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Trust Boundary Template"))
                {
                    if (_trustBoundaryTemplates == null)
                        _trustBoundaryTemplates = new AdvisableCollection<TrustBoundaryTemplate>();

                    _trustBoundaryTemplates.Add(tbt);
                    UndoRedoManager.Attach(tbt);
                    scope?.Complete();

                    ChildCreated?.Invoke(trustBoundaryTemplate);
                }
            }
            else
                throw new ArgumentException(nameof(trustBoundaryTemplate));
        }

        [InitializationRequired]
        public ITrustBoundaryTemplate AddTrustBoundaryTemplate([Required] string name, string description, ITrustBoundary source = null)
        {
            var result = new TrustBoundaryTemplate(name)
            {
                Description = description,
            };
            source.CloneProperties(result);
            Add(result);

            return result;
        }

        [InitializationRequired(false)]
        public bool RemoveTrustBoundaryTemplate(Guid id)
        {
            bool result = false;

            var template = GetTrustBoundaryTemplate(id) as TrustBoundaryTemplate;

            if (template != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Trust Boundary Template"))
                {
                    result = _trustBoundaryTemplates.Remove(template);
                    if (result)
                    {
                        UndoRedoManager.Detach(template);
                        scope?.Complete();

                        ChildRemoved?.Invoke(template);
                    }
                }
            }

            return result;
        }
    }
}