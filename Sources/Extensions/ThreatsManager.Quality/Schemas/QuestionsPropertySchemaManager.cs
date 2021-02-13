using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Properties;
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
            var schema = _model.GetSchema(Questions, Resources.DefaultNamespace) ?? _model.AddSchema(Questions, Resources.DefaultNamespace);
            schema.Description = Resources.QuestionsPropertySchemaDescription;
            schema.Visible = false;
            schema.AppliesTo = Scope.ThreatModel;
            schema.System = true;
            schema.AutoApply = false;
            schema.NotExportable = false;

            return schema;
        }

        #region Questions.
        [InitializationRequired]
        public IPropertyType GetQuestionsPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Questions) ?? schema.AddPropertyType(Questions, PropertyValueType.JsonSerializableObject);
                result.DoNotPrint = true;
                result.Visible = false;
                result.Description = Resources.QuestionsDescription;
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
                    }
                } else if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    if (!(jsonSerializableObject.Value is Questions questions))
                    {
                        questions = new Questions();
                        jsonSerializableObject.Value = questions;
                    }
                    questions.Add(question);
                }
            }
        }

        public void RemoveQuestion([NotNull] Question question)
        {
            var propertyType = GetQuestionsPropertyType();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Questions questions)
                {
                    questions.Remove(question);
                }
            }
        }
        #endregion
    }
}
