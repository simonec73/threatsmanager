using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.ImportersExporters
{
    internal static class Manager
    {
        public static IThreatModel OpenKB([Required] string pathName)
        {
            var kbManagers = ExtensionUtils.GetExtensions<IKnowledgeBaseManager>()?.ToArray();
            var kbManager = kbManagers?
                .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, pathName));
            return kbManager?.Load(LocationType.FileSystem, pathName, false);
        }

        public static bool SaveKB(this IThreatModel model, [Required] string pathName) 
        {
            var kbManagers = ExtensionUtils.GetExtensions<IKnowledgeBaseManager>()?.ToArray();
            var kbManager = kbManagers?
                .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, pathName));
            return kbManager.Export(model, new DuplicationDefinition()
            {
                AllEntityTemplates = true,
                AllFlowTemplates = true,
                AllMitigations = true,
                AllProperties = true,
                AllPropertySchemas = true,
                AllThreatTypes = true,
                AllTrustBoundaryTemplates = true,
                AllSeverities = true,
                AllStrengths = true
            }, model.Name, model.Description, LocationType.FileSystem, pathName);
        }

        public static bool Save(this IThreatModel model, [Required] string pathName)
        {
            var kbManagers = ExtensionUtils.GetExtensions<IPackageManager>()?.ToArray();
            var kbManager = kbManagers?
                .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, pathName));
            return kbManager.Save(model, LocationType.FileSystem, pathName, false, null, out var newLocation);
        }

        public static void Close(Guid id)
        {
            ThreatModelManager.Remove(id);
        }
    }
}
