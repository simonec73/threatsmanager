using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("1DCB5FE6-C79B-4B65-B61A-988D2CE01AB6", "Copy Context Aware Action", 10, ExecutionMode.Simplified)]
    public class Copy : IShapesContextAwareAction, 
        IShapeContextAwareAction, ICommandsBarContextAwareAction
    {
        public Scope Scope => Scope.Entity | Scope.Group;
        public string Label => "Copy";
        public string Group => "Copy&Paste";
        public Bitmap Icon => Properties.Resources.copy_big;
        public Bitmap SmallIcon => Properties.Resources.copy;
        public Shortcut Shortcut => Shortcut.CtrlC;

        public bool Execute([NotNull] object item)
        {
            return false;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IShape shape)
        {
            return Execute(new[] {shape}, null);
        }

        public bool Execute(IEnumerable<IShape> shapes, IEnumerable<ILink> links)
        {
            ShapesInfo shapesInfo = new ShapesInfo
            {
                Shapes = shapes != null ? new List<IShape>(shapes.ToArray()) : null,
                Links = links != null ? new List<ILink>(links.ToArray()) : null
            };
            var serialized = JsonConvert.SerializeObject(shapesInfo, Formatting.Indented, new JsonSerializerSettings()
            {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                TypeNameHandling = TypeNameHandling.All
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
            });

            DataObject dataObject = new DataObject();
            dataObject.SetData("ShapesInfo", serialized);
            Clipboard.SetDataObject(dataObject);
            return true;
        }

        public ICommandsBarDefinition CommandsBar => new CommandsBarDefinition(Group, Label, new IActionDefinition[]
        {
            new ActionDefinition(new Guid(this.GetExtensionId()), Label, Label, Icon, SmallIcon, false, Shortcut)
            {
                Tag = this
            }
        });
    }
}