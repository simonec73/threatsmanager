using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace SimpleThreatModelAnalyzer
{
    class IdentityChecker
    {
        private readonly Dictionary<Guid, List<IIdentity>> _identities = new Dictionary<Guid, List<IIdentity>>();

        public void AddIdentities(IEnumerable<IIdentity> identities)
        {
            var items = identities?.ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    var list = _identities.Where(x => x.Key == item.Id)
                        .Select(x => x.Value).FirstOrDefault();
                    if (list == null)
                    {
                        list = new List<IIdentity>();
                        _identities.Add(item.Id, list);
                    }
                    list.Add(item);
                }
            }
        }

        public int CountDuplicates => _identities.Count(x => x.Value.Count > 1);

        public IEnumerable<List<IIdentity>> Duplicates => _identities
            .Select(x => x.Value)
            .Where(x => x.Count > 1);
    }
}
