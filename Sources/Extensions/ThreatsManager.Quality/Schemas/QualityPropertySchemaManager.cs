using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Schemas
{
    public class QualityPropertySchemaManager : IInitializableObject
    {
        private readonly IThreatModel _model;
        private static string FalsePositive = "FalsePositive";

        public QualityPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public bool IsInitialized => _model != null;

        [InitializationRequired]
        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Resources.SchemaName}' schema"))
            {
                result = _model.GetSchema(Resources.SchemaName, Resources.DefaultNamespace) ??
                         _model.AddSchema(Resources.SchemaName, Resources.DefaultNamespace);
                result.Description = Resources.QualityPropertySchemaDescription;
                result.Visible = false;
                result.AppliesTo = Scope.All;
                result.System = true;
                result.AutoApply = false;
                result.NotExportable = true;

                scope?.Complete();
            }

            return result;
        }

        #region False Positive.
        [InitializationRequired]
        public IPropertyType GetFalsePositivePropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get FalsePositive property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType(FalsePositive) ?? schema.AddPropertyType(FalsePositive, PropertyValueType.JsonSerializableObject);
                    result.DoNotPrint = true;
                    result.Visible = false;
                    scope?.Complete();
                }
            }

            return result;
        }

        public bool IsFalsePositive([NotNull] IPropertiesContainer container,  
            [NotNull] IQualityAnalyzer qualityAnalyzer)
        {
            bool result = false;

            var propertyType = GetFalsePositivePropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
                if (property?.Value is FalsePositiveList list)
                {
                    result = list.FalsePositives?
                        .Any(x =>
                            string.CompareOrdinal(x.QualityInitializerId, qualityAnalyzer.GetExtensionId()) == 0) ?? false;
                }
            }

            return result;
        }

        public static bool IsFalsePositive([NotNull] IQualityAnalyzer qualityAnalyzer, [NotNull] IPropertiesContainer container)
        {
            bool result = false;

            if (container is IThreatModelChild threatModelChild &&
                threatModelChild.Model != null)
            {
                result = new QualityPropertySchemaManager(threatModelChild.Model).IsFalsePositive(container, qualityAnalyzer);
            }

            return result;
        }

        public void SetFalsePositive([NotNull] IPropertiesContainer container, 
            [NotNull] IQualityAnalyzer analyzer,
            [Required] string reason)
        {
            SetFalsePositive(container, analyzer.GetExtensionId(), reason);
        }

        public void SetFalsePositive([NotNull] IPropertiesContainer container, 
            [Required] string analyzerId,
            [Required] string reason)
        {
            using (var scope = UndoRedoManager.OpenScope("Set FalsePositive"))
            {
                var propertyType = GetFalsePositivePropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
                    if (property == null)
                    {
                        var list = new FalsePositiveList
                        {
                            FalsePositives = new List<FalsePositiveInfo> { CreateInfo(analyzerId, reason) }
                        };
                        property = container.AddProperty(propertyType, null) as IPropertyJsonSerializableObject;
                        if (property != null)
                        {
                            property.Value = list;
                            scope?.Complete();
                        }
                    }
                    else
                    {
                        if (property.Value is FalsePositiveList list &&
                            !(list.FalsePositives?.Any(x => string.CompareOrdinal(x.QualityInitializerId, analyzerId) == 0) ?? false))
                        {
                            if (list.FalsePositives == null)
                                list.FalsePositives = new List<FalsePositiveInfo>();
                            list.FalsePositives.Add(CreateInfo(analyzerId, reason));
                            scope?.Complete();
                        }
                    }
                }
            }
        }

        public void ResetFalsePositive([NotNull] IPropertiesContainer container, 
            [NotNull] IQualityAnalyzer analyzer)
        {
            using (var scope = UndoRedoManager.OpenScope("Reset FalsePositive"))
            {
                var propertyType = GetFalsePositivePropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
                    if (property?.Value is FalsePositiveList list)
                    {
                        var item = list.FalsePositives?.FirstOrDefault(x =>
                            string.CompareOrdinal(x.QualityInitializerId, analyzer.GetExtensionId()) == 0);
                        if (item != null)
                        {
                            list.FalsePositives.Remove(item);
                            scope?.Complete();
                        }
                    }
                }
            }
        }

        public string GetReason([NotNull] IPropertiesContainer container,
            [NotNull] IQualityAnalyzer analyzer)
        {
            string result = null;

            var propertyType = GetFalsePositivePropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
                if (property?.Value is FalsePositiveList list)
                {
                    result = list.FalsePositives?.FirstOrDefault(x =>
                        string.CompareOrdinal(x.QualityInitializerId, analyzer.GetExtensionId()) == 0)?.Reason;
                }
            }

            return result;
        }

        private FalsePositiveInfo CreateInfo([Required] string analyzerId, [Required] string reason)
        {
            string userName;
            try
            {
                userName = UserName.GetDisplayName();
            }
            catch
            {
                userName = Environment.UserName;
            }

            return new FalsePositiveInfo()
            {
                QualityInitializerId = analyzerId,
                Reason = reason,
                Author = userName,
                Timestamp = DateTime.Now
            };
        }
        #endregion
    }
}
