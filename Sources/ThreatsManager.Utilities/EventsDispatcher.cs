using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Class used to dispatch events.
    /// </summary>
    public static class EventsDispatcher
    {
        private static readonly Dictionary<string, List<Action<object>>> _handlers = new Dictionary<string, List<Action<object>>>();

        /// <summary>
        /// Register an event handler.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handler">Handler for the events.</param>
        /// <returns>True if the registration succeeds, false otherwise.</returns>
        public static bool Register([Required] string eventName, [NotNull] Action<object> handler)
        {
            bool result = false;

            List<Action<object>> handlers;
            if (_handlers.ContainsKey(eventName))
                handlers = _handlers[eventName];
            else
            {
                handlers = new List<Action<object>>();
                _handlers.Add(eventName, handlers);
            }

            if (!handlers.Contains(handler))
            {
                handlers.Add(handler);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Deregister an event handler.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handler">Handler for the events.</param>
        /// <returns>True if the deregistration succeeds, false otherwise.</returns>
        public static bool Deregister([Required] string eventName, [NotNull] Action<object> handler)
        {
            bool result = false;

            if (_handlers.ContainsKey(eventName))
            {
                result = _handlers[eventName]?.Remove(handler) ?? false;
            }

            return result;
        }

        /// <summary>
        /// Clear all event handlers registration for a given event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <returns>True if the deregistration succeeds, false otherwise.</returns>
        public static bool Clear([Required] string eventName)
        {
            return _handlers.Remove(eventName);
        }

        /// <summary>
        /// Provides a list of the events registered with the dispatcher.
        /// </summary>
        public static IEnumerable<string> Events => _handlers.Keys.ToList();

        /// <summary>
        /// Raise an event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="data">Data to be passed as argument for the event.</param>
        /// <returns>True if at least a handler received and processed the object.</returns>
        public static bool RaiseEvent([Required] string eventName, object data)
        {
            bool result = false;

            if (_handlers.TryGetValue(eventName, out var handlers) && (handlers?.Any() ?? false))
            {
                foreach (var handler in handlers)
                {
                    try
                    {
                        handler?.Invoke(data);
                        result |= true;
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }
    }
}
