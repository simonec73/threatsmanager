using System;
using System.Collections.Generic;
using System.Text;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    class FactComparer : IEqualityComparer<Fact>
    {
        public bool Equals(Fact x, Fact y)
        {
            if (x.Id == y.Id) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Fact obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
