﻿using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Wrapper for the Recording Scope, to keep track of the recording scopes created.
    /// </summary>
    public class UndoRedoScope : IDisposable
    {
        private static readonly List<RecordingScope> _scopes = new List<RecordingScope>();
        private RecordingScope _scope;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scope"></param>
        public UndoRedoScope(RecordingScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Read-only property returning true if there is any open Recording Scope.
        /// </summary>
        public static bool HasPendingRecordingScopes => _scopes.Count > 0;

        /// <summary>
        /// Mark the Recording Scope as complete.
        /// </summary>
        public void Complete()
        {
            _scope?.Complete();
        }

        /// <summary>
        /// Dispose the Recording Scope.
        /// </summary>
        public void Dispose()
        {
            if (_scope != null)
            {
                try
                {
                    _scopes.Remove(_scope);
                    _scope.Dispose();
                }
                catch (NotSupportedException)
                {
                    // Ignore eventual not supported exceptions.
                }
                finally
                {
                    _scope = null;
                }
            }
        }
    }

}
