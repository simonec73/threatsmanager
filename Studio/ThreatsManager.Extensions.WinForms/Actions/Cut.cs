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
    [Extension("F9EF814D-4373-4694-97AD-3C0AD0E9DD7C", "Cut Context Aware Action", 11, ExecutionMode.Simplified)]
    public class Cut : IShapesContextAwareAction, 
        IShapeContextAwareAction, ICommandsBarContextAwareAction, 
        IEntityGroupRemovingRequiredAction, IDataFlowRemovingRequiredAction
    {
        public Scope Scope => Scope.Entity | Scope.Group;
        public string Label => "Cut";
        public string Group => "Copy&Paste";
        public Bitmap Icon => Properties.Resources.cut_big;
        public Bitmap SmallIcon => Properties.Resources.cut;
        public Shortcut Shortcut => Shortcut.CtrlX;

        public event Action<IShape> EntityGroupRemovingRequired;
        public event Action<ILink> DataFlowRemovingRequired;

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
            var shapesArray = shapes?.ToArray();
            var linksArray = links?.ToArray();

            ShapesInfo shapesInfo = new ShapesInfo
            {
                Shapes = shapesArray != null ? new List<IShape>(shapesArray) : null,
                Links = linksArray != null ? new List<ILink>(linksArray) : null
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

            if (linksArray?.Any() ?? false)
            {
                foreach (var link in linksArray)
                {
                    DataFlowRemovingRequired?.Invoke(link);
                }
            }

            if (shapesArray?.Any() ?? false)
            {
                foreach (var shape in shapesArray)
                {
                    EntityGroupRemovingRequired?.Invoke(shape);
                }
            }

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