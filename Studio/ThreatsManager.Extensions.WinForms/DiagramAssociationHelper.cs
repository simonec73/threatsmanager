using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions
{
    static class DiagramAssociationHelper
    {
        internal static event Action<IEntity, IDiagram> DiagramAssociated;
        internal static event Action<IEntity> DiagramDisassociated;

        internal static void NotifyDiagramAssociation([NotNull] IEntity entity, [NotNull] IDiagram diagram)
        {
            DiagramAssociated?.Invoke(entity, diagram);
        }

        internal static void NotifyDiagramDisassociation([NotNull] IEntity entity)
        {
            DiagramDisassociated?.Invoke(entity);
        }
    }
}
