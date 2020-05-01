using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Extensions that are executed after a Threat Model has been loaded.
    /// </summary>
    /// <remarks>This can be used to execute tasks like connect automatically to some external system.</remarks>
    public interface IPostLoadProcessor
    {
        /// <summary>
        /// Process the Threat Model that has just been loaded.
        /// </summary>
        /// <param name="model">Threat Model to be processed.</param>
        void Process(IThreatModel model);
    }
}