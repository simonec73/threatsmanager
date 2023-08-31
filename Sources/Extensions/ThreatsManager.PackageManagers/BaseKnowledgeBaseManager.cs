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
    public abstract class BaseKnowledgeBaseManager : IKnowledgeBaseManager
    {
        protected const string ThreatModelFile = "threatmodel.json";
        protected string Extension = ".undefined";
        protected string PackageType = "Undefined";

        public LocationType SupportedLocations => LocationType.FileSystem;

        public bool CanHandle(LocationType locationType, [Required] string location)
        {
            return SupportedLocations.HasFlag(locationType) &&
                   !location.StartsWith(@"\\") &&
                   string.Compare(Path.GetExtension(location), Extension, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public string GetFilter(LocationType locationType)
        {
            string result = null;

            if (locationType == LocationType.FileSystem)
                result = $"{PackageType} ({Extension})|{Extension}";

            return result;
        }

        public abstract IThreatModel Load(LocationType locationType, string location, 
            bool strict = true, Guid? newThreatModelId = null);


        public bool Import(IThreatModel target, DuplicationDefinition definition,
            LocationType locationType, string location, bool strict = true,
            Guid? newThreatModelId = null)
        {
            var result = false;

            IThreatModel kb = null;

            try
            {
                kb = Load(locationType, location, strict, newThreatModelId);
                if (kb != null)
                {
                    result = target.Merge(kb, definition);
                }
            }
            catch
            {
            }
            finally
            {
                if (kb != null)
                {
                    ThreatModelManager.Remove(kb.Id);
                }
            }

            return result;
        }

        public abstract bool Export(IThreatModel model, DuplicationDefinition definition,
            string name, string description, LocationType locationType, string location);
    }
}
