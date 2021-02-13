using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.PropertySchemasUpdaters
{
    [Extension("A075DC09-FCB5-43E8-9406-34C021AFB8D2", "Questions Property Schema Updater", 25, ExecutionMode.Simplified)]
    public class QuestionsPropertySchemasUpdater : IPropertySchemasUpdater
    {
        public bool HasPropertySchema(IThreatModel model, string schemaName, string nsName)
        {
            bool result = false;

            var questions = (new QuestionsPropertySchemaManager(model)).GetQuestions()?.ToArray();
            if (questions?.Any() ?? false)
            {
                foreach (var question in questions)
                {
                    var rule = question.Rule;
                    result = rule?.Root?.HasSchema(schemaName, nsName) ?? false;
                    if (result)
                        break;
                }
            }

            return result;
        }

        public bool HasPropertyType(IThreatModel model, string schemaName, string nsName, string propertyName)
        {
            bool result = false;

            var questions = (new QuestionsPropertySchemaManager(model)).GetQuestions()?.ToArray();
            if (questions?.Any() ?? false)
            {
                foreach (var question in questions)
                {
                    var rule = question.Rule;
                    result = rule?.Root?.HasPropertyType(schemaName, nsName, propertyName) ?? false;
                    if (result)
                        break;
                }
            }

            return result;
        }

        public bool UpdateSchemaName([NotNull] IThreatModel model,
            [Required] string oldName, [Required] string oldNamespace, 
            [Required] string newName, [Required] string newNamespace)
        {
            bool result = false;

            var questions = (new QuestionsPropertySchemaManager(model)).GetQuestions()?.ToArray();
            if (questions?.Any() ?? false)
            {
                foreach (var question in questions)
                {
                    var rule = question.Rule;
                    result |= rule?.Root?.UpdateSchema(oldName, oldNamespace, newName, newNamespace) ?? false;
                }
            }

            return result;
        }

        public bool UpdatePropertyTypeName([NotNull] IThreatModel model,
            [Required] string schemaName, [Required] string schemaNamespace, 
            [Required] string oldPropertyTypeName, [Required] string newPropertyTypeName)
        {
            bool result = false;

            var questions = (new QuestionsPropertySchemaManager(model)).GetQuestions()?.ToArray();
            if (questions?.Any() ?? false)
            {
                foreach (var question in questions)
                {
                    var rule = question.Rule;
                    result |= rule?.Root?.UpdateSchema(schemaName, schemaNamespace, oldPropertyTypeName, newPropertyTypeName) ?? false;
                }
            }

            return result;
        }
    }
}