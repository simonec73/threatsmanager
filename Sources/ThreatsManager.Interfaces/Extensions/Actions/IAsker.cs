using System;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Auxiliary interface for Actions to ask for feedback from the user.
    /// </summary>
    public interface IAsker
    {
        /// <summary>
        /// Ask a question to the user.
        /// </summary>
        /// <remarks>The parameters are:<list type="bullet">
        /// <item><description>The IAsker which should receive the answer.</description></item>
        /// <item><description>An object which provides the context and that must be returned as is to method <see cref="Answer"/>.</description></item>
        /// <item><description>The caption of the request to be shown to the user.</description></item>
        /// <item><description>The text of the request to be shown to the user.</description></item>
        /// <item><description>A boolean, which is positive if the request is a warning and false if it is informational only.</description></item>
        /// <item><description>The available options for the answer.</description></item>
        /// </list></remarks>
        event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        /// <summary>
        /// Return the answer from the user.
        /// </summary>
        /// <param name="context">Context of the request.</param>
        /// <param name="answer">Received answer.</param>
        void Answer(object context, AnswerType answer);
    }
}
