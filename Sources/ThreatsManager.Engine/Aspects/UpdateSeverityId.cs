﻿using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    /// <summary>
    /// Attribute assigned to a Severity property to automatically assign the corresponding ID property.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class UpdateSeverityIdAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing &&
                args.Value is ISeverity severity &&
                args.Instance is ISeverityIdChanger target)
            {
                var oldValue = target.GetSeverityId();
                var newValue = severity.Id;
                if (oldValue != newValue)
                    target.SetSeverityId(newValue);
            }

            base.OnSetValue(args);
        }
    }
}
