using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Relationships
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RelationshipDetails
    {
        [JsonProperty("main")]
        public Guid Main { get; set; }

        [Child]
        [JsonProperty("related")]
        private AdvisableCollection<RecordableGuid> _mitigationIds { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<Guid> MitigationIds => _mitigationIds?.Select(x => x.Value).AsEnumerable();

        public bool AddMitigation([NotNull] IMitigation mitigation)
        {
            bool result = false;

            if (!(_mitigationIds?.Any(x => mitigation.Id == x.Value) ?? false))
            {
                if (_mitigationIds == null)
                    _mitigationIds = new AdvisableCollection<RecordableGuid>();

                _mitigationIds.Add(new RecordableGuid(mitigation.Id));
                result = true;
            }

            return result;
        }

        public bool RemoveMitigation(Guid id)
        {
            bool result = false;

            var existing = _mitigationIds?.FirstOrDefault(x => x.Value == id);
            if (existing != null)
            {
                result = _mitigationIds.Remove(existing);
            }

            return result;
        }
    }
}
