using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.PackageManagers
{
    public abstract class BaseKnowledgeBaseManager
    {
        protected const string ThreatModelFile = "threatmodel.json";
        protected string Extension = ".undefined";
        protected string PackageType = "Undefined";

        public LocationType BaseSupportedLocations => LocationType.FileSystem;

        public bool BaseCanHandle(LocationType locationType, [Required] string location)
        {
            return BaseSupportedLocations.HasFlag(locationType) &&
                   !location.StartsWith(@"\\") &&
                   string.Compare(Path.GetExtension(location), Extension, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public string BaseGetFilter(LocationType locationType)
        {
            string result = null;

            if (locationType == LocationType.FileSystem)
                result = $"{PackageType} ({Extension})|{Extension}";

            return result;
        }
    }
}
