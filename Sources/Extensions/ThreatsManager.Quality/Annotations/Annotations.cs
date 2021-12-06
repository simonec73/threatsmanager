using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Quality.Annotations
{
    /// <summary>
    /// Annotations object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Annotations
    {
        /// <summary>
        /// Enumeration of the Annotations.
        /// </summary>
        [JsonProperty("annotations")] 
        private List<Annotation> _annotations { get; set; }

        public event Action<Annotation> AnnotationAdded;

        public event Action<Annotation> AnnotationRemoved;
            
        public IEnumerable<Annotation> Items => _annotations?.AsReadOnly();

        public void Add([NotNull] Annotation annotation)
        {
            if (_annotations == null)
                _annotations = new List<Annotation>();

            if (!_annotations.Contains(annotation))
            {
                _annotations.Add(annotation);
                AnnotationAdded?.Invoke(annotation);
            }
        }

        public void Remove([NotNull] Annotation annotation)
        {
            if (_annotations?.Contains(annotation) ?? false)
            {
                _annotations.Remove(annotation);
                AnnotationRemoved?.Invoke(annotation);
            }
        }

        public void Clear()
        {
            var annotations = Items?.ToArray();
            if (annotations?.Any() ?? false)
            {
                foreach (var annotation in annotations)
                {
                    AnnotationRemoved?.Invoke(annotation);
                }

                _annotations.Clear();
            }
        }
    }
}