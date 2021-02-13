using System.Collections.Generic;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal class PlaceholderComparer : EqualityComparer<Placeholder>
    {
        public override bool Equals(Placeholder x, Placeholder y)
        {
            return (x == null && y == null) ||
                   (x != null && y != null && x.Section == y.Section && string.CompareOrdinal(x.Code, y.Code) == 0);
        }

        public override int GetHashCode(Placeholder obj)
        {
            return obj == null ? 0 : $"{obj.Section.ToString()}:{obj.Code}".GetHashCode();
        }
    }
}