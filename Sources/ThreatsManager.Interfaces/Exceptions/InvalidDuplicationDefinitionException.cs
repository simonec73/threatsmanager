using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// Exception to be raised when an the Package Manager requires encryption.
    /// </summary>
    [Serializable]
    public class InvalidDuplicationDefinitionException : Exception
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        public InvalidDuplicationDefinitionException() : base("Threat Model duplication definition is not valid.")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rules">List of the rules which have been violated.</param>
        public InvalidDuplicationDefinitionException([NotNull] IEnumerable<string> rules) : this()
        {
            Rules = rules;
        }

        /// <summary>
        /// Rules which have been violated.
        /// </summary>
        public IEnumerable<string> Rules {get; private set;}
    }
}
