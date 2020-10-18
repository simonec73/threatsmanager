using System.Collections.Generic;

namespace ThreatsManager.PackageManagers
{
    public class ExtensionInfoComparer : IEqualityComparer<ExtensionInfo>
    {
        public bool Equals(ExtensionInfo x, ExtensionInfo y)
        {
            return (x == null && y == null) || 
                   (x != null && y != null && string.CompareOrdinal(x.Id, y.Id) == 0);
        }

        public int GetHashCode(ExtensionInfo obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}