using System;
using System.IO;
using ThreatsManager.Packaging;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities.Properties;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Template Manager.
    /// </summary>
    /// <remarks>Utility class to manage serialization and deserialization of Threat Model templates.</remarks>
    public static class TemplateManager
    {
        /// <summary>
        /// Save the Threat Model passed as argument as Template.
        /// </summary>
        /// <param name="model">Threat Model to be saved as Template.</param>
        /// <param name="definition">Definition of the information to export.</param>
        /// <param name="name">Name of the Template.</param>
        /// <param name="description">Description of the Template.</param>
        /// <param name="path">Path to the Template to be created.</param>
        public static void SaveTemplate(this IThreatModel model, [NotNull] DuplicationDefinition definition, 
            [Required] string name, string description, [Required] string path)
        {
            var newModel = model.Duplicate(name, definition);
            newModel.Description = description;
            var serialization = ThreatModelManager.Serialize(newModel);

            var extension = Path.GetExtension(path)?.ToLower();
            switch (extension)
            {
                case ".tmt":
                    var package = Package.Create(path);
                    package.Add(Resources.ThreatModelTemplateFile, serialization);
                    package.Save();
                    break;
                case ".tmk":
                    if (File.Exists(path))
                        File.Delete(path);

                    using (var file = File.OpenWrite(path))
                    {
                        using (var writer = new BinaryWriter(file))
                        {
                            writer.Write(serialization);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Open an existing Template.
        /// </summary>
        /// <param name="path">Location of the Template, in the file system.</param>
        /// <returns>The Threat Model representing the Template.</returns>
        public static IThreatModel OpenTemplate([Required] string path)
        {
            IThreatModel result = null;

            byte[] bytes;
            var extension = Path.GetExtension(path)?.ToLower();
            switch (extension)
            {
                case ".tmt":
                    var package = new Package(path);
                    bytes = package.Read(Resources.ThreatModelTemplateFile);
                    break;
                case ".tmk":
                    using (var file = File.OpenRead(path))
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    }
                    break;
                default:
                    bytes = null;
                    break;
            }

            if (bytes != null)
                result = ThreatModelManager.Deserialize(bytes, true);

            return result;
        }

        /// <summary>
        /// Close a Template that has been opened.
        /// </summary>
        /// <param name="id">Identifier of the Template to be closed.</param>
        public static void CloseTemplate(Guid id)
        {
            ThreatModelManager.Remove(id);
        }
    }
}