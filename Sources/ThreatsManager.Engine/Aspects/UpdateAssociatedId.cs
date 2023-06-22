using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Model;
using PostSharp.Serialization;
using System;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Aspects
{
    /// <summary>
    /// Attribute assigned to an Associated Identity property to automatically assign the corresponding ID property.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(NotifyPropertyChangedAttribute))]
    public class UpdateAssociatedIdAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing &&
                args.Value is IIdentity identity &&
                args.Instance is IAssociatedIdChanger target)
            {
                var oldValue = target.GetAssociatedId();
                var newValue = identity.Id;
                if (oldValue != newValue)
                    target.SetAssociatedId(newValue);
            }

            base.OnSetValue(args);
       }
    }
}
