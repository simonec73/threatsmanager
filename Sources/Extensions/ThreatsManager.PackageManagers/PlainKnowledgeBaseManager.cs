﻿using Newtonsoft.Json;
using System.IO;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.PackageManagers.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.PackageManagers.Packaging;
using System;

namespace ThreatsManager.PackageManagers
{
    [Extension("E8C13CB7-0618-4C0A-958D-2955316C6951", "Plain Knowledge Base Manager", 10, ExecutionMode.Simplified)]
    public class PlainKnowledgeBaseManager : BaseKnowledgeBaseManager
    {
        public PlainKnowledgeBaseManager() 
        {
            Extension = ".tmt";
            PackageType = "Knowledge Base";
        }

        public override IThreatModel Load(LocationType locationType, string location, bool strict = true, Guid? newThreatModelId = null)
        {
            IThreatModel result = null;

            if (locationType == LocationType.FileSystem && File.Exists(location))
            {
                var package = new Package(location);
                var bytes = package.Read(Resources.ThreatModelTemplateFile);

                if (bytes != null)
                {
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

        public override bool Export([NotNull] IThreatModel model, [NotNull] DuplicationDefinition definition,
            string name, string description, LocationType locationType, [Required] string location)
        {
            var result = false;

            if (locationType == LocationType.FileSystem)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = model.Name ?? "Not defined";
                }

                if (string.IsNullOrEmpty(description))
                {
                    description = model.Description;
                }

                var newModel = model.Duplicate(name, definition);
                newModel.Description = description;
                newModel.AddVersion();
                var serialization = ThreatModelManager.Serialize(newModel);

                var package = Package.Create(location);
                package.Add(Resources.ThreatModelTemplateFile, serialization);
                package.Save();
            }

            return result;
        }
    }
}
