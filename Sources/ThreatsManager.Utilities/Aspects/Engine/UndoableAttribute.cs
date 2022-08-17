using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    [PSerializable]
    [IntroduceInterface(typeof(IUndoable))]
    public class UndoableAttribute : InstanceLevelAspect, IUndoable
    {
        public bool IsUndoEnabled { get; set; }
    }
}
