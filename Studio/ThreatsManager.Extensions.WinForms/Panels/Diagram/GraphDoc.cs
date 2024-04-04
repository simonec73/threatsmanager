using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    /// <summary>
    /// Summary description for GraphDoc.
    /// </summary>
    [Serializable]
    public class GraphDoc : GoDocument
    {
        public GraphDoc()
        {
            LinksLayer = Layers.CreateNewLayerAfter(Layers.Default);
            LinksLayer.Identifier = "Links";
            LinksLayer.AllowSelect = true;
            Layers.CreateNewLayerBefore(Layers.Default).Identifier = "bottom";
        }

        #region Public methods.
        public IEnumerable<GraphGroup> GetGroups()
        {
            IEnumerable<GraphGroup> result = null;

            var groups = this.OfType<GraphGroup>()?.ToArray();

            if (groups?.Any() ?? false)
            {
                var list = new List<GraphGroup>();

                foreach (var group in groups)
                {
                    var children = GetChildren(group)?.ToArray();
                    if (children?.Any() ?? false)
                        list.AddRange(children);
                    list.Add(group);
                }

                if (list.Any())
                    result = list;
            }

            return result;
        }

        public GraphGroup GetGroup(Guid groupId)
        {
            GraphGroup result = null;

            var groups = this.OfType<GraphGroup>()?.ToArray();
            
            if (groups?.Any() ?? false)
            {
                result = groups
                    .FirstOrDefault(x => x.GroupShape?.AssociatedId == groupId);

                if (result == null)
                {
                    foreach (var group in groups)
                    {
                        result = RecursiveSearch(group, groupId);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        public IEnumerable<GraphEntity> GetEntities()
        {
            IEnumerable<GraphEntity> result = null;

            List<GraphEntity> list = new List<GraphEntity>();

            var entities = this.OfType<GraphEntity>()?.ToArray();
            if (entities?.Any() ?? false)
            {
                list.AddRange(entities);
            }

            var groups = this.OfType<GraphGroup>()?.ToArray();

            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    entities = GetEntities(group)?.ToArray();
                    if (entities?.Any() ?? false)
                    {
                        list.AddRange(entities);
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        public GraphEntity GetEntity(Guid entityId)
        {
            GraphEntity result = this.OfType<GraphEntity>()?
                .FirstOrDefault(x => (x.EntityShape?.AssociatedId ?? Guid.Empty) == entityId);

            if (result == null)
            {
                var groups = this.OfType<GraphGroup>()?.ToArray();

                if (groups?.Any() ?? false)
                {
                    foreach (var group in groups)
                    {
                        result = RecursiveEntitySearch(group, entityId);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Private auxiliary methods.
        private IEnumerable<GraphGroup> GetChildren(GraphGroup group)
        {
            IEnumerable<GraphGroup> result = null;

            var children = group.OfType<GraphGroup>()?.ToArray();
            if (children?.Any() ?? false)
            {
                var list = new List<GraphGroup>();

                foreach (var child in children)
                {
                    var grandchildren = GetChildren(child)?.ToArray();
                    if (grandchildren?.Any() ?? false)
                        list.AddRange(grandchildren);
                    list.Add(child);
                }

                if (list.Any())
                    result = list;
            }

            return result;
        }

        private GraphGroup RecursiveSearch(GraphGroup group, Guid groupId)
        {
            GraphGroup result = null;

            var children = group.OfType<GraphGroup>()?.ToArray();
            if (children?.Any() ?? false)
            {
                result = children
                   .FirstOrDefault(x => x.GroupShape?.AssociatedId == groupId);

                if (result == null)
                {
                    foreach (var child in children)
                    {
                        result = RecursiveSearch(child, groupId);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        private IEnumerable<GraphEntity> GetEntities(GraphGroup group)
        {
            IEnumerable<GraphEntity> result = null;

            List<GraphEntity> list = new List<GraphEntity>();

            var entities = group.OfType<GraphEntity>()?.ToArray();
            if (entities?.Any() ?? false)
            {
                list.AddRange(entities);
            }

            var groups = group.OfType<GraphGroup>()?.ToArray();

            if (groups?.Any() ?? false)
            {
                foreach (var child in groups)
                {
                    entities = GetEntities(child)?.ToArray();
                    if (entities?.Any() ?? false)
                    {
                        list.AddRange(entities);
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private GraphEntity RecursiveEntitySearch(GraphGroup group, Guid entityId)
        {
            GraphEntity result = group.OfType<GraphEntity>()?
                .FirstOrDefault(x => (x.EntityShape?.AssociatedId ?? Guid.Empty) == entityId);

            if (result == null)
            {
                var children = group.OfType<GraphGroup>()?.ToArray();

                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        result = RecursiveEntitySearch(child, entityId);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}