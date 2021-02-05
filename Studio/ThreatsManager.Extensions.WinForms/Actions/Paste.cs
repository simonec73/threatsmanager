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
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("640E5B69-FC04-4FA5-96FF-AD7B0EAC0413", "Paste Context Aware Action", 10, ExecutionMode.Simplified)]
    public class Paste : IIdentityContextAwareAction, IIdentityAddingRequiredAction, 
        IDataFlowAddingRequiredAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Diagram;
        public string Label => "Paste";
        public string Group => "Copy&Paste";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.CtrlV;

        public event Action<IDiagram, IIdentity, PointF, SizeF> IdentityAddingRequired; 
        public event Action<IDiagram, IDataFlow> DataFlowAddingRequired;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (identity is IDiagram diagram)
            {
                if (Clipboard.GetDataObject() is DataObject dataObject && 
                    dataObject.GetDataPresent("ShapesInfo") &&
                    dataObject.GetData("ShapesInfo") is string shapesInfo)
                {
                    var deserialized = JsonConvert.DeserializeObject<ShapesInfo>(shapesInfo, new JsonSerializerSettings()
                    {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                        TypeNameHandling = TypeNameHandling.All
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                    });

                    var shapes = deserialized?.Shapes?
                        .Where(x => (x.Identity as IThreatModelChild)?.Model == diagram.Model)
                        .ToArray();
                    if (shapes?.Any() ?? false)
                    {
                        float x = shapes.Min(t => t.Position.X);
                        float y = shapes.Min(t => t.Position.Y);

                        var groups = new List<IGroup>();

                        foreach (var shape in shapes)
                        {
                            if (shape is IEntityShape && shape.Identity is IEntity entity &&
                                diagram.GetShape(entity) == null)
                            {
                                result = true;
                                
                                IdentityAddingRequired?.Invoke(diagram, entity, 
                                    new PointF(shape.Position.X - x, shape.Position.Y - y), SizeF.Empty);

                                RecursivelyAddParents(entity, groups);
                            }
                        }

                        foreach (var shape in shapes)
                        {
                            if (shape is IGroupShape groupShape && shape.Identity is IGroup group &&
                                !groups.Contains(group))
                            {
                                result = true;

                                IdentityAddingRequired?.Invoke(diagram, group,
                                    new PointF(shape.Position.X - x, shape.Position.Y - y), groupShape.Size);
                            }
                        }

                        var links = deserialized?.Links?
                            .Where(l => l.DataFlow?.Model == diagram.Model)
                            .ToArray();
                        if (links?.Any() ?? false)
                        {
                            foreach (var link in links)
                            {
                                result = true;
                                DataFlowAddingRequired?.Invoke(diagram, link.DataFlow);
                            }
                        }
                    }
                    else
                    {
                        ShowMessage?.Invoke("There is nothing to paste.");
                    }
                }
            } 

            return result;
        }

        private void RecursivelyAddParents([NotNull] IEntity entity, [NotNull] List<IGroup> groups)
        {
            var parent = entity.Parent;
            if (parent != null && !groups.Contains(parent))
            {
                groups.Add(parent);
                RecursivelyAddParents(parent, groups);
            }
        }

        private void RecursivelyAddParents([NotNull] IGroup group, [NotNull] List<IGroup> groups)
        {
            if (group is IGroupElement child)
            {
                var parent = child.Parent;
                if (parent != null && !groups.Contains(parent))
                {
                    groups.Add(parent);
                    RecursivelyAddParents(parent, groups);
                }
            }
        }
    }
}