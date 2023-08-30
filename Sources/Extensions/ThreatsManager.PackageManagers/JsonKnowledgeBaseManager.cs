using Newtonsoft.Json;
using System.IO;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using PostSharp.Patterns.Contracts;
using System;

namespace ThreatsManager.PackageManagers
{
    [Extension("63F9A8B2-E4AB-4CAE-A8E3-30D76AD2F62A", "Json Knowledge Base Manager", 15, ExecutionMode.Simplified)]
    public class JsonKnowledgeBaseManager : BaseKnowledgeBaseManager, IKnowledgeBaseManager
    {
        public JsonKnowledgeBaseManager() 
        {
            Extension = ".tmj";
            PackageType = "Json Knowledge Base";
        }

        public LocationType SupportedLocations => BaseSupportedLocations;

        public string GetFilter(LocationType locationType)
        {
            return BaseGetFilter(locationType);
        }

        public bool CanHandle(LocationType locationType, string location)
        {
            return BaseCanHandle(locationType, location);
        }

        public IThreatModel Load(LocationType locationType, string location, bool strict = true, Guid? newThreatModelId = null)
        {
            IThreatModel result = null;

            if (locationType == LocationType.FileSystem && File.Exists(location))
            {
                byte[] bytes = null;
                using (var file = File.OpenRead(location))
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                }

                if (bytes != null)
                {
                    result = null;

                    try
                    {
                        result = ThreatModelManager.Deserialize(bytes, !strict, newThreatModelId);
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

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

        public bool Export([NotNull] IThreatModel model, [NotNull] DuplicationDefinition definition,
            string name, string description, LocationType locationType, [Required] string location)
        {
            var result = false;

            if (locationType == LocationType.FileSystem)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = model.Name;
                }

                if (string.IsNullOrEmpty(description))
                {
                    description = model.Description;
                }

                var newModel = model.Duplicate(name, definition);
                newModel.Description = description;
                var serialization = ThreatModelManager.Serialize(newModel);

                if (File.Exists(location))
                    File.Delete(location);

                using (var file = File.OpenWrite(location))
                {
                    using (var writer = new BinaryWriter(file))
                    {
                        writer.Write(serialization);
                    }
                }
            }

            return result;
        }
    }
}
