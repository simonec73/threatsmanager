using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public class ThreatTypeComparer : IEqualityComparer<IThreatType>
    {
        public bool Equals(IThreatType x, IThreatType y)
        {
            return (x == null && y == null) || (x != null && y != null && x.Id == y.Id);
        }

        public int GetHashCode(IThreatType obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}