using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Engine
{
    class ExtensionMetadataEqualityComparer<T> : IEqualityComparer<Lazy<T, IExtensionMetadata>>
    {
        public bool Equals(Lazy<T, IExtensionMetadata> x, Lazy<T, IExtensionMetadata> y)
        {
            bool result = false;

            if (x == null)
            {
                if (y == null)
                    result = true;
            }
            else if (y != null)
            {
                result = string.CompareOrdinal(x.Metadata?.Id, y.Metadata?.Id) == 0;
            }

            return result;
        }

        public int GetHashCode(Lazy<T, IExtensionMetadata> obj)
        {
            return obj.GetHashCode();
        }
    }
}