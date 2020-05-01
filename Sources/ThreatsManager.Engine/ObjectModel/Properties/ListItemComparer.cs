using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    public class ListItemComparer : IEqualityComparer<IListItem>
    {
        public bool Equals(IListItem x, IListItem y)
        {
            return (x == null && y == null) || (x != null && y != null && string.CompareOrdinal(x.Id, y.Id) == 0);
        }

        public int GetHashCode([NotNull] IListItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}