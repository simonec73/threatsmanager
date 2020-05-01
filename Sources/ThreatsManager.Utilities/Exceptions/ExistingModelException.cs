using System;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Exceptions
{
    [Serializable]
    public class ExistingModelException : Exception
    {
        public ExistingModelException(IThreatModel model) : base($"Threat Model {model.Name} is already known")
        {
        }
    }
}
