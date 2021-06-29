namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the Extensions that may change their behavior based on the Execution Mode.
    /// </summary>
    public interface IExecutionModeSupport
    {
        /// <summary>
        /// Set the Execution Mode.
        /// </summary>
        void SetExecutionMode(ExecutionMode mode);
    }
}
