namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// A stage part of an universal pipeline to process a Threat Model.
    /// </summary>
    [ExtensionDescription("Stage for Universal Pipelines")]
    public interface IUniversalStage : IExtension
    {
        /// <summary>
        /// Direction of the stage.
        /// </summary>
        StageDirection Direction { get; }

        /// <summary>
        /// Identifier of the Extension implementing the stage linked to the current one.
        /// </summary>
        string LinkedStageId { get; }
        
        /// <summary>
        /// Process the input.
        /// </summary>
        /// <param name="input">Input text to be processed.</param>
        /// <returns>Result of the stage processing.</returns>
        string Execute(string input);
    }
}
