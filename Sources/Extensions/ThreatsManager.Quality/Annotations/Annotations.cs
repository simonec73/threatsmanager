using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    /// <summary>
    /// Annotations object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Annotations : IPostDeserialization
    {
        /// <summary>
        /// Enumeration of the Annotations.
        /// </summary>
        [JsonProperty("annotations")]
        [Reference]
        [field:NotRecorded]
        private List<Annotation> _legacyAnnotations { get; set; }

        [JsonProperty("items")]
        [Child]
        private AdvisableCollection<Annotation> _annotations { get; set; }

        public event Action<Annotation> AnnotationAdded;

        public event Action<Annotation> AnnotationRemoved;

        [property:NotRecorded]
        public IEnumerable<Annotation> Items => _annotations?.AsEnumerable();

        public void Add([NotNull] Annotation annotation)
        {
            if (_annotations == null)
                _annotations = new AdvisableCollection<Annotation>();

            if (!_annotations.Contains(annotation))
            {
                _annotations.Add(annotation);
                UndoRedoManager.Attach(annotation);
                AnnotationAdded?.Invoke(annotation);
            }
        }

        public void Remove([NotNull] Annotation annotation)
        {
            if (_annotations?.Contains(annotation) ?? false)
            {
                _annotations.Remove(annotation);
                UndoRedoManager.Detach(annotation);
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
                    UndoRedoManager.Detach(annotation);
                    AnnotationRemoved?.Invoke(annotation);
                }

                _annotations.Clear();
            }
        }

        public void ExecutePostDeserialization()
        {
            if (_legacyAnnotations?.Any() ?? false)
            {
                if (_annotations == null)
                    _annotations = new AdvisableCollection<Annotation>();

                foreach (var ann in _legacyAnnotations)
                {
                    _annotations.Add(ann);
                    UndoRedoManager.Attach(ann);
                }

                _legacyAnnotations.Clear();
            }
        }
    }
}