namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface implemented by the static panels, that is panels that execute an Action with no direct reference to the Model.
    /// </summary>
    public interface IStaticPanel : IPanel
    {
        /// <summary>
        /// Action to be executed.
        /// </summary>
        /// <param name="parameter">Parameter of the specific action.</param>
        void SetAction(string parameter);
    }
}