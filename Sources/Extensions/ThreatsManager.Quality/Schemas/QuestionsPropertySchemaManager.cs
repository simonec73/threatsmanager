using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Schemas
{
    public class QuestionsPropertySchemaManager : IInitializableObject
    {
        private readonly IThreatModel _model;
        private static string Questions = "Questions";

        public QuestionsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public bool IsInitialized => _model != null;

        [InitializationRequired]
        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Questions}' schema"))
            {
                result = _model.GetSchema(Questions, Resources.DefaultNamespace) ?? _model.AddSchema(Questions, Resources.DefaultNamespace);
                result.Description = Resources.QuestionsPropertySchemaDescription;
                result.Visible = false;
                result.AppliesTo = Scope.ThreatModel;
                result.System = true;
                result.AutoApply = false;
                result.NotExportable = false;
                scope?.Complete();
            }

            return result;
        }

        #region Questions.
        [InitializationRequired]
        public IPropertyType GetQuestionsPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope($"Get {Questions} property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType(Questions) ?? schema.AddPropertyType(Questions, PropertyValueType.JsonSerializableObject);
                    result.DoNotPrint = true;
                    result.Visible = false;
                    result.Description = Resources.QuestionsDescription;
                    scope?.Complete();
                }
            }

            return result;
        }

        public IEnumerable<Question> GetQuestions()
        {
            IEnumerable<Question> result = null;

            var propertyType = GetQuestionsPropertyType();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Questions questions)
                {
                    result = questions.Items;
                }
            }

            return result;
        }

        public void AddQuestion([NotNull] Question question)
        {
            using (var scope = UndoRedoManager.OpenScope("Add Question"))
            {
                var propertyType = GetQuestionsPropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property == null)
                    {
                        property = _model.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                        {
                            var questions = new Questions();
                            questions.Add(question);
                            jsonSerializableObject.Value = questions;
                            scope?.Complete();
                        }
                    }
                    else if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                    {
                        if (!(jsonSerializableObject.Value is Questions questions))
                        {
                            questions = new Questions();
                            jsonSerializableObject.Value = questions;
                        }
                        questions.Add(question);
                        scope?.Complete();
                    }
                }
            }
        }

        public void RemoveQuestion([NotNull] Question question)
        {
            using (var scope = UndoRedoManager.OpenScope("Remove Question"))
            {
                var propertyType = GetQuestionsPropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Questions questions)
                    {
                        questions.Remove(question);
                        scope?.Complete();
                    }
                }
            }
        }
        #endregion
    }
}
