using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Model;
using PostSharp.Serialization;
using System;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    /// <summary>
    /// Attribute assigned to a Parent property to automatically assign the corresponding ID property.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(AutoApplySchemasAttribute))]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class UpdateParentIdAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing &&
                args.Value is IIdentity identity &&
                args.Instance is IParentIdChanger target)
            {
                var oldValue = target.GetParentId();
                var newValue = identity.Id;
                if (oldValue != newValue)
                    target.SetParentId(newValue);
            }

            base.OnSetValue(args);
        }
    }
}
