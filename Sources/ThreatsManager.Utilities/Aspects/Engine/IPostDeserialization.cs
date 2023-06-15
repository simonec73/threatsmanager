using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Interface used to identify classes that contain a post-deserialization activity.
    /// </summary>
    /// <remarks>It is called by the PropertiesContainerAspect, which does post-deserialization, to allow executing
    /// more code after the deserialization.</remarks>
    public interface IPostDeserialization
    {
        /// <summary>
        /// Method to be executed after the deserialization.
        /// </summary>
        void ExecutePostDeserialization();
    }
}
